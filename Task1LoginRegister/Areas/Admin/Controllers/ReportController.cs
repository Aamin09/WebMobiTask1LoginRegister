using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScottPlot;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly WebMobiTask1DbContext context;

        public ReportController(WebMobiTask1DbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalOrders = await context.Orders.CountAsync();
            ViewBag.TotalRevenue = await context.Orders.Where(o => o.PaymentStatus == "Paid").SumAsync(o => o.TotalAmount);
            ViewBag.TotalCustomers = await context.Userlogins.Where(u => u.Role == "User").CountAsync();
            ViewBag.TotalProducts = await context.Products.CountAsync();

            return View();
        }

        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.AddDays(-30);
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            var orders = await context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.DeliveryAddress)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.PaymentStatus == "Paid")
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            var finalData = new SalesReportViewModel
            {
                StartDate = startDate.Value,
                EndDate = endDate.Value,
                Orders = orders,
                TotalSales = orders.Sum(o => o.TotalAmount),
                TotalOrders = orders.Count(),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0
            };
            return View(finalData);
        }

        
        public async Task<IActionResult> ProductPerformance(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.AddDays(-30);
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            var orderItems = await context.OrderItems
                .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate)
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .ToListAsync();

            var productPerformance = orderItems.GroupBy(oi => oi.ProductId)
                .Select(g => new ProductPerformanceDto
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalQuantitySold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.TotalPrice),
                    OrderCount = g.Select(oi => oi.OrderId).Distinct().Count()
                })
                .OrderByDescending(p => p.TotalRevenue)
                .ToList();

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            return View(productPerformance);
        }
    }
}
