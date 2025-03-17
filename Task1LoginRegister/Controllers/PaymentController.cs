using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    public class PaymentController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly UserService userService;
        private readonly RazorPayService razorPayService;
        private readonly IConfiguration configuration;


        public PaymentController(WebMobiTask1DbContext context, UserService userService, RazorPayService razorPayService, IConfiguration configuration)
        {
            this.context = context;
            this.userService = userService;
            this.razorPayService = razorPayService;
            this.configuration = configuration;
        }

        public async Task<IActionResult> ProcessPayment(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Login", "Accont");

            var order = await context.Orders
                .Include(o => o.DeliveryAddress)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
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
                order.RazorpayOrderId = options["id"].ToString();
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
            // If we have a payment_id, it was successful
            if (!string.IsNullOrEmpty(callbackModel.razorpay_payment_id))
            {
                // Verify signature
                string generatedSignature = GenerateRazorpaySignature(
                    callbackModel.razorpay_order_id,
                    callbackModel.razorpay_payment_id,
                    configuration["Razorpay:KeySecret"]
                );

                if (generatedSignature == callbackModel.razorpay_signature)
                {
                    // Finding the order
                    var order = await context.Orders
                        .FirstOrDefaultAsync(o => o.RazorpayOrderId == callbackModel.razorpay_order_id);

                    if (order != null)
                    {
                        // Updating payment details for success
                        order.PaymentId = callbackModel.razorpay_payment_id;
                        order.PaymentStatus = "Paid";
                        order.PaymentMethod = "Online Payment";
                        order.OrderStatus = "Processing";
                        await context.SaveChangesAsync();

                        return RedirectToAction("OrderConfirmation", "Order", new { orderId = order.OrderId });
                    }
                }
            }
            else
            {
                // No payment_id means payment failed or was abandoned
                var order = await context.Orders
                    .FirstOrDefaultAsync(o => o.RazorpayOrderId == callbackModel.razorpay_order_id);

                if (order != null)
                {
                    // Update payment status as failed
                    order.PaymentStatus = "Failed";
                    await context.SaveChangesAsync();

                    return RedirectToAction("PaymentFailed", new { orderId = order.OrderId, errorMessage = "Payment was not completed" });
                }
            }

            return RedirectToAction("PaymentFailed", new { errorMessage = "Payment verification failed" });
        }

        public async Task<IActionResult> PaymentCancelled(string razorpay_order_id)
        {
            var order = await context.Orders
                .FirstOrDefaultAsync(o => o.RazorpayOrderId == razorpay_order_id);

            if (order != null)
            {
                order.PaymentStatus = "Failed";
                await context.SaveChangesAsync();

                return RedirectToAction("PaymentFailed", new { orderId = order.OrderId, errorMessage = "Payment was cancelled" });
            }

            return RedirectToAction("PaymentFailed", new { errorMessage = "Order not found" });
        }

        public IActionResult PaymentFailed(int orderId, string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.OrderId = orderId;
            return View();
        }

        private string GenerateRazorpaySignature(string orderId, string paymentId, string secret)
        {
            string payload = $"{orderId}|{paymentId}";
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hasBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return BitConverter.ToString(hasBytes).Replace("-", "").ToLower();
            }
        }

    }
}
