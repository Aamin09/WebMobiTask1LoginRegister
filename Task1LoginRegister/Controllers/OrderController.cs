using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly UserService userService;
        private readonly RazorPayService razorPayService;
        private readonly IConfiguration configuration;

        public OrderController(WebMobiTask1DbContext context, UserService userService, RazorPayService razorPayService, IConfiguration configuration)
        {
            this.context = context;
            this.userService = userService;
            this.razorPayService = razorPayService;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
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
                return RedirectToAction("Login", "Account");
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

            var highestDeliveryCharge = cartItems.Where(x => x.Product.DeliveryCharge > 0)
            .Max(x => (decimal?)x.Product.DeliveryCharge) ?? 0;

            decimal totalAmount = cartItems.Sum(c => c.TotalPrice + c.TotalGST) + highestDeliveryCharge;

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

        public async Task<IActionResult> ProcessPayment(int orderId)
        {
            var userId= await userService.GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Login", "Accont");

            var order= await context.Orders
                .Include(o=>o.DeliveryAddress)
                .FirstOrDefaultAsync(o=>o.OrderId == orderId && o.UserId == userId);

            if (order == null) return NotFound();

            try
            {
                // creating razorpay order
                var options = razorPayService.CreateOrder(
                    order.TotalAmount,
                    order.OrderNumber,
                    "INR");

                // creating model for the view
                var razorpayOrderModel = new RazorpayOrderModel
                {
                    OrderId = options["id"].ToString(),
                    Razorpaykey = configuration["Razorpay:KeyId"],
                    Amount = order.TotalAmount,
                    Currency = "INR",
                    Name = order.DeliveryAddress.FullName,
                    Email = order.DeliveryAddress.Email,
                    PhoneNumber = order.DeliveryAddress.PhoneNumber,
                    Address = $"{order.DeliveryAddress.Street}, {order.DeliveryAddress.City}, {order.DeliveryAddress.State} - {order.DeliveryAddress.ZipCode} , {order.DeliveryAddress.Country}",
                    Description = $"Order #{order.OrderNumber}",
                    ApplicationOrderId = order.OrderId
                };

                // save razorpay order id to order table
                order.RazorpayOrderId= options["id"].ToString();
                await context.SaveChangesAsync();   

                return View(razorpayOrderModel);    
            }
            catch (Exception ex)
            {
                // Log the exception
                return RedirectToAction("PaymentFailed", new { orderId = order.OrderId, errorMessage = ex.Message });

            }
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCallback(RazorpayCallbackModel callbackModel)
        {
            // verifying signature to confirm the payment response is from razorpay
            string generatedSignature = GenerateRazorpaySignature(
                callbackModel.razorpay_order_id,
                callbackModel.razorpay_payment_id,
                configuration["Razorpay:KeySecret"]
                );

            if(generatedSignature == callbackModel.razorpay_signature)
            {
                // finding the order
                var order = await context.Orders
                    .FirstOrDefaultAsync(o => o.RazorpayOrderId == callbackModel.razorpay_order_id);

                if(order != null)
                {
                    // updating payment details
                    order.PaymentId=callbackModel.razorpay_payment_id;
                    order.PaymentStatus = "Paid";
                    order.OrderStatus = "Processing";
                    await context.SaveChangesAsync();

                    return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
                }
            }

            return RedirectToAction("PaymentFailed", new { errorMessage = "Payment verification failed" }) ;
        }

        public IActionResult PaymentFailed(int orderId, string errorMessage)
        {
            ViewBag.ErrorMessage=errorMessage;
            ViewBag.OrderId= orderId;
            return View();
        }

        private string GenerateRazorpaySignature(string orderId, string paymentId, string secret)
        {
            string payload= $"{orderId}|{paymentId}";
            using(HMACSHA256 hmac= new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hasBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return BitConverter.ToString(hasBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
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

        public async Task<IActionResult> Invoice(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
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