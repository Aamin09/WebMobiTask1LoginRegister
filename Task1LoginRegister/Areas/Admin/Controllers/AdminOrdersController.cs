using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly WebMobiTask1DbContext context;

        public AdminOrdersController(WebMobiTask1DbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await context.Orders
                .Include(u => u.User)
                .Include(d => d.DeliveryAddress)
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                 .OrderByDescending(o => o.OrderDate)
                 .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await context.Orders
              .Include(o => o.User)
              .Include(o => o.DeliveryAddress)
              .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.Product)
                      .ThenInclude(p => p.Subcategory)
                          .ThenInclude(s => s.Taxes)
              .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            string[] validStatus = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            if (!validStatus.Contains(status))
            {
                return BadRequest("Invalid status value");
            }

            order.OrderStatus = status;

            if (status == "Cancelled" && order.PaymentStatus == "Failed")
            {
                order.OrderStatus = "Cancelled";
            }
            await context.SaveChangesAsync();

            return RedirectToAction("OrderDetails", new { id });
        }

        public async Task<IActionResult> UpdatePaymentStatus(int id, string pstatus)
        {
            var order = await context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            string[] validStatus = { "Pending", "Paid", "Failed" };
            if (!validStatus.Contains(pstatus))
            {
                return BadRequest("Invalid status value");
            }

            order.PaymentStatus = pstatus;

            if (pstatus == "Failed" && order.OrderStatus == "Cancelled")
            {
                order.PaymentStatus = "Failed";
                order.OrderStatus = "Cancelled";
            }
            await context.SaveChangesAsync();

            return RedirectToAction("OrderDetails", new { id });
        }


        public async Task<IActionResult> CustomerOrders(int userId, string status, int page = 1)
        {
            var user = await context.Userlogins.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Customer = user;
            ViewBag.CurrentStatus = status;

            var orders = context.Orders
                .Include(o => o.User)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Subcategory.Taxes)
                .Where(o => o.UserId == userId)
                .AsQueryable();

            // Status filter for orders
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.OrderStatus == status);
            }

            int totalOrders = await orders.CountAsync();

            ViewBag.TotalOrders = totalOrders;
            return View(orders);
        }

    }
}
