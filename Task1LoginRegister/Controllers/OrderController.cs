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
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await context.Carts
                .Where(c => c.UserId == userId && c.IsActive)
                .Include(p => p.Product).ThenInclude(p => p.ProductImages)
                    .Include(c => c.Product.Subcategory)
                    .ThenInclude(s => s.Taxes)
                .Include(c => c.ProductVariant).ThenInclude(pv => pv.VariantAttributeValues).ThenInclude(vav => vav.ProductAttributeValue).ThenInclude(pav => pav.Attribute)
                .Include(c => c.ProductVariant.ProductImages)
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
                    .ThenInclude(s => s.Taxes)
                .Include(c => c.ProductVariant).ThenInclude(pv => pv.VariantAttributeValues).ThenInclude(vav => vav.ProductAttributeValue).ThenInclude(pav => pav.Attribute)
                .Include(c => c.ProductVariant.ProductImages)
                .ToListAsync();

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
                // update stock quantity based on whether a variant was purchased
                if (item.ProductVariantId != null && item.ProductVariant != null)
                {
                    var variant = await context.ProductVariants.FindAsync(item.ProductVariantId);
                    if (variant != null)
                    {
                        variant.StockQuantity -= item.Quantity;
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var product = await context.Products.Include(p => p.Subcategory).ThenInclude(s => s.Taxes).FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                    }
                    await context.SaveChangesAsync();
                }

                decimal sellingPrice, basePrice, discountPercetage;
                string productName, productSku, variantDetails;
                if (item.ProductVariantId != null && item.ProductVariant != null)
                {
                    sellingPrice = item.ProductVariant.FinalSellingPrice;
                    basePrice = item.ProductVariant.BasePrice;
                    discountPercetage = item.ProductVariant.DiscountPercentage;
                    productName = item.ProductVariant.VarinatName;
                    productSku = item.ProductVariant.SKU ?? item.Product.SKU;

                    //creating and passing variant details string
                    if (item.ProductVariant.VariantAttributeValues != null && item.ProductVariant.VariantAttributeValues.Any())
                    {
                        var attribute = item.ProductVariant.VariantAttributeValues
                            .Select(v => v.ProductAttributeValue).Where(v => v != null)
                            .Select(v => $"{v.Attribute.Name}: {v.Value}");
                        variantDetails = string.Join(", ", attribute);
                    }
                }
                else
                {
                    sellingPrice = item.Product.CalculatedSellingPrice;
                    discountPercetage = item.Product.SellingPricePercent;
                    basePrice = item.Product.Price;
                    productName = item.Product.Name;
                    productSku = item.Product.SKU;
                }
                // Calculating GST dynamically
                var gstPercentage = (item.Product.Subcategory?.Taxes?.CGST ?? 0) +
                                    (item.Product.Subcategory?.Taxes?.SGST ?? 0);
                var gstAmount = Math.Round((sellingPrice * gstPercentage) / 100m, 2);

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    BasePrice = basePrice,
                    SnapshotDiscountPercentage = discountPercetage,
                    SnapshotPrice = sellingPrice,
                    ProductSKU = productSku,
                    ProductName = productName,
                    SnapshotCostPrice = item.ProductVariantId != null && item.ProductVariant != null ? item.ProductVariant.CostPrice : item.Product.CostPrice,
                    SnapshotProfitPercentage = item.ProductVariantId != null && item.ProductVariant != null ? item.ProductVariant.ProfitPercentage : item.Product.SellingPricePercent,
                    SnapshotCGSTPercentage = item.Product?.Subcategory?.Taxes?.CGST ?? 0,
                    SnapshotSGSTPercentage = item.Product?.Subcategory?.Taxes?.SGST ?? 0,
                    SnapshotGSTAmount = gstAmount * item.Quantity,
                    DeliveryCharge = item.Product?.DeliveryCharge ?? 0,
                    ProductVariantId=item.ProductVariantId,
                    VariantDetails= item.SelectedAttributes,
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
                return RedirectToAction("ProcessPayment", "Payment", new { orderId = order.OrderId });
            }

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
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
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(p => p.VariantAttributeValues)
                            .ThenInclude(s => s.ProductAttributeValue).ThenInclude(pav=>pav.Attribute)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> OrderDetails(int orderId)
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
                        .ThenInclude(p => p.Subcategory)
                            .ThenInclude(s => s.Taxes)
                 .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(p => p.VariantAttributeValues)
                            .ThenInclude(s => s.ProductAttributeValue).ThenInclude(pav => pav.Attribute)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Refund = await context.RefundDetails
               .FirstOrDefaultAsync(r => r.OrderId == orderId);

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


        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
            if (order == null) return NotFound();

            // checking if order is eligible for cancellation
            if (order.OrderStatus == "Delivered" || order.OrderStatus == "Shipped" || order.OrderStatus == "Cancelled" || order.OrderStatus == "Cancellation Requested")
            {
                TempData["ErrorMessage"] = $"Order cannot be cancelled as it is already {order.OrderStatus}.";
                return RedirectToAction("OrderDetails", new { orderId = orderId });
            }

            // Restore stock if order is cancelled
            foreach (var item in order.OrderItems)
            {
                if (item.ProductVariantId != null && item.ProductVariant != null)
                {
                    var variant = await context.ProductVariants.FindAsync(item.ProductVariantId);
                    if (variant != null)
                    {
                        variant.StockQuantity += item.Quantity;
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var product = await context.Products.Include(p => p.Subcategory).ThenInclude(s => s.Taxes).FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                    if (product != null)
                    {
                        product.StockQuantity += item.Quantity;
                    }
                    await context.SaveChangesAsync();
                }
            }

            // If paid online, redirect to refund
            if (order.PaymentMethod == "Online Payment" && order.PaymentStatus == "Paid")
            {
                return RedirectToAction("RequestRefund", "Refund", new { orderId = orderId });
            }
            else
            {
                order.OrderStatus = "Cancelled";
                if (order.PaymentStatus == "Pending")
                {
                    order.PaymentStatus = "Failed";
                }
                await context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your order has been cancelled successfully.";
                return RedirectToAction("OrderDetails", new { orderId = orderId });
            }


        }
    }
}