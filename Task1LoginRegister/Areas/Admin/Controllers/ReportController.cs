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
              .OrderBy(o => o.OrderDate)
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
       
        public async Task<IActionResult> ExportCustomerReport(DateTime? startDate, DateTime? endDate)
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


            // configure report
            var reportConfig = new ReportConfig<CustomerReportDto>
            {
                ReportTitle = "Sales Report",
                StartDate = startDate,
                EndDate = endDate,
                Data = customerReport,
                Columns = new List<ReportColumn<CustomerReportDto>>
                {
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText ="Customer Name",
                        ValueSelector= c=>c.CustomerName,
                        Width=150
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Email",
                        ValueSelector = c => c.Email
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Contact",
                        ValueSelector = c => c.Phone
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Total Orders",
                        ValueSelector = c => c.TotalOrders.ToString("N2")
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Total Spent (₹)",
                        ValueSelector = c => c.TotalSpent.ToString("N2")
                    },
                      new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Avg. Order Value (₹)",
                        ValueSelector =  c => c.AverageOrdeValue.ToString("N2")
                    }
                },
                SummaryItems = new List<string>
                {
                    $"Total Customers: {customerReport.Count()}",
                    $"Overall Orders: ₹{customerReport.Sum(o => o.TotalOrders):N2}",
                    $"Overall Spent: ₹{customerReport.Sum(o => o.TotalSpent):N2}",
                    $"Overall Order Value: ₹{customerReport.Average(o => o.AverageOrdeValue):N2}"
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

        public async Task<IActionResult> ExportProductPerformaceReport(DateTime? startDate, DateTime? endDate)
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

            // configure report
            var reportConfig = new ReportConfig<ProductPerformanceDto>
            {
                ReportTitle = "Product Performance Report",
                StartDate = startDate,
                EndDate = endDate,
                Data = productPerformance,
                Columns = new List<ReportColumn<ProductPerformanceDto>>
                {
                    new ReportColumn<ProductPerformanceDto>
                    {
                        HeaderText ="Product Name",
                        ValueSelector= p=>p.ProductName,
                         Width=150
                    },
                    new ReportColumn<ProductPerformanceDto>
                    {
                        HeaderText = "Quantity Sold",
                        ValueSelector = p => p.TotalQuantitySold
                    },
                    new ReportColumn<ProductPerformanceDto>
                    {
                        HeaderText = "Total Revenue (₹)",
                        ValueSelector = p => p.TotalRevenue.ToString("N2")
                    },
                    new ReportColumn<ProductPerformanceDto>
                    {
                        HeaderText = "Orders",
                        ValueSelector = p => p.OrderCount
                    },
                    new ReportColumn<ProductPerformanceDto>
                    {
                        HeaderText = "Avg. Order Value (₹)",
                        ValueSelector = p => p.AverageOrderValue.ToString("N2")
                    }
                },
                SummaryItems = new List<string>
                {
                    $"Total Products: {productPerformance.Count()}",
                    $"Total Products Quantity Sold: {productPerformance.Sum(o => o.TotalQuantitySold):N2}",
                    $"Total Sales: ₹{productPerformance.Sum(o => o.TotalRevenue):N2}",
                    $"Average Order Value: ₹{productPerformance.Average(o => o.TotalRevenue):N2}"
                }
            };

            // Generate PDF
            byte[] pdfBytes = pdfReportService.GenerateReport(reportConfig);

            return File(
                pdfBytes,
                "application/pdf",
                $"Product_perfo_Report_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }


        public async Task<IActionResult> InventoryReport(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Now.AddDays(-30);
            endDate ??= DateTime.Now;

            var products = await context.Products
                .Include(p => p.Category).Include(p => p.Subcategory).Include(p => p.OrderItems).ThenInclude(oi => oi.Order)
                .Select(p => new InventoryReportDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    SKU = p.SKU,
                    Category = p.Category.Name,
                    Subcategory = p.Subcategory.Name,
                    Price = p.Price,
                    SellingPrice = p.CalculatedSellingPrice,
                    StockQuantity = p.StockQuantity,
                    MinimumStockLevel = p.MinimumStockLevel,
                    Status = p.Status,
                    TotalSales = p.OrderItems
    .Where(oi => oi.Order.OrderDate >= startDate &&
                 oi.Order.OrderDate <= endDate &&
                 oi.Order.OrderStatus == "Delivered" &&
                 oi.Order.PaymentStatus == "Paid")
    .Sum(oi => oi.Quantity),

                    ValueStatus = p.StockQuantity <= p.MinimumStockLevel ? "Low Stock" : "Adequate Stock"
                }).ToListAsync();

            // Get top 20 products by stock quantity
            var top20Products = products
                .OrderByDescending(p => p.StockQuantity)
                .Take(20)
                .ToList();

            // Data for stock status pie chart
            var lowStockCount = products.Count(p => p.ValueStatus == "Low Stock");
            var adequateStockCount = products.Count - lowStockCount;

            var chartData = new InventoryChartDataViewModel
            {
                ProductNames = top20Products.Select(p => p.ProductName).ToList(),
                StockQuantities = top20Products.Select(p => p.StockQuantity).ToList(),
                TotalSales = top20Products.Select(p => p.TotalSales).ToList(),
                StockStatusLabels = new List<string> { "Low Stock", "Adequate Stock" },
                StockStatusData = new List<int> { lowStockCount, adequateStockCount }
            };

            var finalData = new InventoryReportMainViewModel
            {
                Products = products,
                ChartData = chartData,
                StartDate = startDate.Value,
                EndDate = endDate.Value,
                TotalProductCount = products.Count(),
                LowStockProductCount = products.Count(p => p.StockQuantity <= p.MinimumStockLevel),
            };

            return View(finalData);
        }
    }
}