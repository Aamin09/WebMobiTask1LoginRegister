using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Controllers
{
    public class RefundController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly UserService userService;
        private readonly RazorPayService razorPayService;

        public RefundController(WebMobiTask1DbContext context, UserService userService, RazorPayService razorPayService)
        {
            this.context = context;
            this.userService = userService;
            this.razorPayService = razorPayService;
        }

        [Authorize]
        public async Task<IActionResult> RequestRefund(int orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Login", "Account");

            var order = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
            if (order == null) return NotFound();

            if (order.PaymentStatus != "Paid" || order.PaymentMethod != "Online Payment")
            {
                TempData["ErrorMessage"] = "Only paid online orders are eligble for refund.";
                return RedirectToAction("OrderDetails", "Order", new { orderId = orderId });
            }

            // validating refund eligibility
            if (string.IsNullOrEmpty(order.PaymentId))
            {
                TempData["ErrorMessage"] = "Payment Id not found for this order.";
                return RedirectToAction("OrderDetails", "Order", new { orderId = orderId });
            }

            // checking if refund already exisits
            var existingRefund = await context.RefundDetails.FirstOrDefaultAsync(r => r.OrderId == orderId);
            if (existingRefund != null)
            {
                TempData["ErrorMessage"] = $"A refund request already exists for this order with status: {existingRefund.Status}";
                return RedirectToAction("OrderDetails", "Order", new { orderId = orderId });
            }

            var refundModel = new RefundModel
            {
                OrderId = orderId,
                PaymentId = order.PaymentId,
                RefundAmount = order.TotalAmount,
                RefundReason = "Order Cancellation by Customer"
            };

            return View(refundModel); 
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RequestRefund(RefundModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Login", "Account");

            var order = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == model.OrderId && o.UserId == userId);
            if (order == null) return NotFound();

            if (order.PaymentStatus != "Paid" || order.PaymentMethod != "Online Payment")
            {
                TempData["ErrorMessage"] = "Only paid online orders are eligble for refund.";
                return RedirectToAction("OrderDetails", "Order", new { orderId = model.OrderId });
            }

            try
            {
                // submitting refund request
                var refund = new RefundDetailsModel
                {
                    RefundId = Guid.NewGuid().ToString("N"),
                    PaymentId = model.PaymentId,
                    Amount=model.RefundAmount,
                    Status="Pending",
                    CreatedAt=DateTime.Now,
                    OrderId=model.OrderId,
                    Notes=model.RefundReason,
                    SpeedCode=GenerateRefundCode()
                };

                context.RefundDetails.Add(refund);

                order.OrderStatus = "Cancellation Requested";

                await context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Refund request submitted successfully. Your refund code is: " + refund.SpeedCode;
                return RedirectToAction("OrderDetails", "Order", new { orderId = model.OrderId });  
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error processing refund request: " + ex.Message);
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> RefundStatus(int  orderId)
        {
            var userId = await userService.GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Login", "Account");

            var refund = await context.RefundDetails
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r=>r.OrderId == orderId && r.Order.UserId == userId);
            
            if(refund == null)
            {
                TempData["ErrorMessage"] = "No refund found for this order.";
                return RedirectToAction("OrderDetails", "Order", new { orderId = orderId });
            }

            return View(refund);
        }

        // Admin Side - Refund Management
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ManageRefunds()
        {
            var refunds= await context.RefundDetails
                .Include(r=>r.Order)
                .OrderByDescending(r=>r.CreatedAt).ToListAsync();

            return View(refunds);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessRefund(string refundId)
        {
            var refund= await context.RefundDetails.Include(r=>r.Order)
                .FirstOrDefaultAsync(r=>r.RefundId == refundId);

            if (refund == null)  return NotFound();

            // Check if already processed
            if (refund.Status != "Pending")
            {
                TempData["ErrorMessage"] = $"Refund already processed with status: {refund.Status}";
                return RedirectToAction("ManageRefunds");
            }

            return View(refund);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessRefund(string refundId, string action)
        {
            var refund = await context.RefundDetails.Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.RefundId == refundId);

            if (refund == null) return NotFound();

            // Check if already processed
            if (refund.Status != "Pending")
            {
                TempData["ErrorMessage"] = $"Refund already processed with status: {refund.Status}";
                return RedirectToAction("ManageRefunds");
            }

            try
            {
                if(action == "approve")
                {
                    // process refund through Razorpay
                    var razorpayRefund = razorPayService.ProcessRefund(refund.PaymentId, refund.Amount, refund.Notes);

                    refund.Status = "Completed";

                    refund.Order.OrderStatus = "Cancelled";
                    refund.Order.PaymentStatus = "Refunded";

                    TempData["SuccessMessage"] = "Refund processed successfully.";
                }else if(action == "reject"){
                    refund.Status = "Rejected";
                    refund.Order.OrderStatus = "Processing";
                    TempData["SuccessMessage"] = "Refund request rejected.";
                }

                await context.SaveChangesAsync();
                return RedirectToAction("ManageRefunds");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error processing refund: " + ex.Message;
                return RedirectToAction("ProcessRefund", new { refundId = refundId });
            }
          
        }
        private string GenerateRefundCode()
        {
            const string chars = "MOFHSVAJEAGVVD775AND54123ADAFGETHHJE";
            var random= new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());  
        }
    }
}
