using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SkiaSharp;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;
using Task1LoginRegister.Services;

namespace Task1LoginRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly WebMobiTask1DbContext context;
        private readonly PdfReportService pdfReportService;

        public ReportController(WebMobiTask1DbContext context, PdfReportService pdfReportService)
        {
            this.context = context;
            this.pdfReportService = pdfReportService;
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

        public async Task<IActionResult> CustomerReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue) startDate = DateTime.Now.AddDays(-30);
            if (!endDate.HasValue) endDate = DateTime.Now;

            var orders = await context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate
                       && o.OrderStatus == "Delivered" && o.PaymentStatus == "Paid")
                .Include(o => o.User)
                .ToListAsync();

            var customerReport = orders.GroupBy(o => o.UserId)
                .Select(g => new CustomerReportDto
                {
                    UserId = g.Key,
                    CustomerName = $"{g.First().User.FirstName} {g.First().User.LastName}",
                    Phone = g.First().User.Phone,
                    Email = g.First().User.Email,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Sum(o => o.TotalAmount),
                    AverageOrdeValue = g.Average(o => o.TotalAmount),
                    FisrtPurchaseDate = g.Min(o => o.OrderDate),
                    LastPurchaseDate = g.Max(o => o.OrderDate)
                })
                .OrderByDescending(c => c.TotalSpent).ToList();

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            return View(customerReport);
        }

        public async Task<IActionResult> ExportSalesReport(DateTime? startDate, DateTime? endDate)
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
              .Include(o => o.DeliveryAddress)
              .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
              .ToListAsync();

            // configure report
            var reportConfig = new ReportConfig<Order>
            {
                ReportTitle = "Sales Report",
                StartDate = startDate,
                EndDate = endDate,
                Data = orders,
                Columns = new List<ReportColumn<Order>>
                {
                    new ReportColumn<Order>
                    {
                        HeaderText ="Order #",
                        ValueSelector= o=>o.OrderNumber,
                Width=150
                    },
                    new ReportColumn<Order>
                    {
                        HeaderText = "Customer",
                        ValueSelector = o => o.DeliveryAddress.FullName
                    },
                    new ReportColumn<Order>
                    {
                        HeaderText = "Date",
                        ValueSelector = o => o.OrderDate.ToShortDateString()
                    },
                    new ReportColumn<Order>
                    {
                        HeaderText = "Amount (₹)",
                        ValueSelector = o => o.TotalAmount.ToString("N2")
                    },
                    new ReportColumn<Order>
                    {
                        HeaderText = "Payment Status",
                        ValueSelector = o => o.PaymentStatus
                    }
                },
                SummaryItems = new List<string>
                {
                    $"Total Orders: {orders.Count()}",
                    $"Total Sales: ₹{orders.Sum(o => o.TotalAmount):N2}",
                    $"Average Order Value: ₹{orders.Average(o => o.TotalAmount):N2}"
                }
            };

            // Generate PDF
            byte[] pdfBytes = pdfReportService.GenerateReport(reportConfig);

            return File(
                pdfBytes,
                "application/pdf",
                $"SalesReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
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
                .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate && oi.Order.PaymentStatus == "Paid")
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