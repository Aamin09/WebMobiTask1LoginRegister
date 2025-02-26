using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    public class OrderController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly UserService userService;

        public OrderController(WebMobiTask1DbContext context, UserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var cartItems = await context.Carts
                .Where(c => c.UserId == userId && c.IsActive)
                .Include(p => p.Product).ThenInclude(p => p.ProductImages)
                .Include(c => c.Product.Subcategory)
                .ThenInclude(s => s.Taxes)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var addresses = await context.DeliveryAddresses
                .Where(a => a.UserId == userId).ToListAsync();

            var finalData = new CheckoutViewDto
            {
                CartItems = cartItems,
                DeliveryAddresses = addresses,
                NewDeliveryAddress = new DeliveryAddress()
            };

            return View(finalData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewDto checkoutView)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var cartItems = await context.Carts
                .Where(c => c.UserId == userId && c.IsActive)
                .Include(c => c.Product)
                .ThenInclude(p => p.Subcategory)
                .ThenInclude(s => s.Taxes).ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            int addressId;
            if (checkoutView.SelectedAddressId.HasValue)
            {
                addressId = checkoutView.SelectedAddressId.Value;
            }
            else
            {
                checkoutView.NewDeliveryAddress.UserId = userId.Value;
                context.DeliveryAddresses.Add(checkoutView.NewDeliveryAddress);
                await context.SaveChangesAsync();

                addressId = checkoutView.NewDeliveryAddress.AddressId;
                checkoutView.SelectedAddressId = addressId;

                if (checkoutView.NewDeliveryAddress.IsDefault)
                {
                    var otherAddress = await context.DeliveryAddresses
                        .Where(a => a.UserId == userId && a.AddressId != addressId)
                        .ToListAsync();

                    foreach (var address in otherAddress)
                    {
                        address.IsDefault = false;
                    }

                    await context.SaveChangesAsync();
                }
            }

            decimal totalAmount = cartItems.Sum(c => c.FinalPrice);

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId.Value,
                DeliveryAddressId = addressId,
                TotalAmount = totalAmount,
                PaymentMethod = checkoutView.PaymentMethod,
                OrderStatus = "Pending",
                PaymentStatus = checkoutView.PaymentMethod == "Cash on Delivery" ? "Pending" : "Processing"
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.CalculatedSellingPrice
                };

                context.OrderItems.Add(orderItem);
            }

            foreach (var item in cartItems)
            {
                item.IsActive = false;
            }

            await context.SaveChangesAsync();

            if (checkoutView.PaymentMethod == "Online Payment")
            {
                return RedirectToAction("ProcessPayment", new { orderId = order.OrderId });
            }

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var order = await context.Orders
                 .Include(o => o.DeliveryAddress)
                 .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Subcategory)
                            .ThenInclude(s => s.Taxes)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> UserOrders()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var orders = await context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductImages)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Invoice(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var order = await context.Orders
                 .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Subcategory)
                            .ThenInclude(s => s.Taxes)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromAddress(int addressId)
        {
            var addresses = await context.DeliveryAddresses.FindAsync(addressId);
            if (addresses != null)
            {
                context.DeliveryAddresses.Remove(addresses);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Checkout");
        }

        private string GenerateOrderNumber()
        {
            return "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
        }
    }
}