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
        private readonly FinancialReportingService financialReportingService;
        private readonly DateRangeService dateRangeService;

        public ReportController(WebMobiTask1DbContext context, PdfReportService pdfReportService, FinancialReportingService financialReportingService, DateRangeService dateRangeService)
        {
            this.context = context;
            this.pdfReportService = pdfReportService;
            this.financialReportingService = financialReportingService;
            this.dateRangeService = dateRangeService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalOrders = await context.Orders.Where(o => o.OrderStatus != "Cancelled").CountAsync();
            ViewBag.TotalRevenue = await context.Orders.Where(o => o.PaymentStatus == "Paid" && o.OrderStatus != "Cancelled").SumAsync(o => o.TotalAmount);
            ViewBag.TotalCustomers = await context.Userlogins.Where(u => u.Role == "User").CountAsync();
            ViewBag.TotalProducts = await context.Products.CountAsync();

            return View();
        }

        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);


            var orders = await financialReportingService.GetOrdersForReportAsync(dateRange.StartDate, dateRange.EndDate,true);

            ViewBag.StartDate = dateRange.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = dateRange.EndDate.ToString("yyyy-MM-dd");

            var finalData = new SalesReportViewModel
            {
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
                Orders = orders,
            };

            financialReportingService.CalculateSalesReportMetrics(finalData);
            return View(finalData);
        }

        public async Task<IActionResult> ExportSalesReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            if (dateRange.StartDate > dateRange.EndDate)
            {
                return BadRequest("Start date cannot be later than end date.");
            }

            var orders = await financialReportingService.GetOrdersForReportAsync(dateRange.StartDate, dateRange.EndDate);

            var finalData = new SalesReportViewModel
            {
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
                Orders = orders
            };

            financialReportingService.CalculateSalesReportMetrics(finalData);

            // Configure report
            var reportConfig = new ReportConfig<Order>
            {
                ReportTitle = "Sales Report",
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
                Data = orders,
                Columns = new List<ReportColumn<Order>>
            {
                new ReportColumn<Order>
                {
                    HeaderText = "Order #",
                    ValueSelector = o => o.OrderNumber,
                    Width = 150
                },
                new ReportColumn<Order>
                {
                    HeaderText = "Customer",
                    ValueSelector = o => o.DeliveryAddress?.FullName ?? "N/A"
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
                },
                new ReportColumn<Order>
                {
                    HeaderText = "Order Status",
                    ValueSelector = o => o.OrderStatus
                },
            },
                SummaryItems = new List<string>
            {
                // Order Overview
                $"Total Orders: {finalData.TotalOrders}",
                $"Paid Orders: {finalData.PaidOrders}",
                $"Failed Orders: {finalData.FailedOrders}",
                $"Cancelled Orders: {finalData.CancelledOrders}",

                // Financial Metrics
                $"Total Sales: ₹{finalData.TotalSales:N2}",
                $"Average Order Value: ₹{finalData.AverageOrderValue:N2}",

                // Payment Method Insights
                $"Payment Method Distribution: {string.Join(", ", finalData.PaymentMethodStats.Select(pm => $"{pm.Key}: {pm.Value}"))}",

                // Daily Sales Snapshot
                $"Daily Sales Snapshot: {string.Join(", ",
                    finalData.DailySales
                        .OrderBy(ds => DateTime.ParseExact(ds.Key, "yyyy-MM-dd", null))
                        .Take(3)
                        .Select(ds => $"{ds.Key}: ₹{ds.Value:N2}") +
                    (finalData.DailySales.Count > 3 ? $" ... and {finalData.DailySales.Count - 3} more days" : "")
                )}"
            }
            };

            // Generate PDF
            byte[] pdfBytes = pdfReportService.GenerateReport(reportConfig);
            return File(
                pdfBytes,
                "application/pdf",
                $"SalesReport_{dateRange.StartDate:yyyyMMdd}_{dateRange.EndDate:yyyyMMdd}.pdf");

        }

        public async Task<IActionResult> CustomerReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            var orders = await context.Orders
                .Where(o => o.OrderDate >= dateRange.StartDate && o.OrderDate <= dateRange.EndDate
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

            ViewBag.StartDate = dateRange.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = dateRange.EndDate.ToString("yyyy-MM-dd");

            return View(customerReport);
        }

        public async Task<IActionResult> ExportCustomerReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            var orders = await context.Orders
                .Where(o => o.OrderDate >= dateRange.StartDate && o.OrderDate <= dateRange.EndDate
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
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
                Data = customerReport,
                Columns = new List<ReportColumn<CustomerReportDto>>
                {
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText ="Customer Name",
                        ValueSelector= c=>c.CustomerName,
                        Width=100
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Email",
                        ValueSelector = c => c.Email,
                         Width=150
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Contact",
                        ValueSelector = c => c.Phone
                    },
                    new ReportColumn<CustomerReportDto>
                    {
                        HeaderText = "Total Orders",
                        ValueSelector = c => c.TotalOrders
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
                $"SalesReport_{dateRange.StartDate:yyyyMMdd}_{dateRange.EndDate:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> ProductPerformance(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);


            var orderItems = await context.OrderItems
                .Where(oi => oi.Order.OrderDate >= dateRange.StartDate && oi.Order.OrderDate <= dateRange.EndDate && oi.Order.PaymentStatus == "Paid")
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

            ViewBag.StartDate = dateRange.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = dateRange.EndDate.ToString("yyyy-MM-dd");

            return View(productPerformance);
        }
        public async Task<IActionResult> ExportProductPerformaceReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            var orderItems = await context.OrderItems
               .Where(oi => oi.Order.OrderDate >= dateRange.StartDate && oi.Order.OrderDate <= dateRange.EndDate && oi.Order.PaymentStatus == "Paid")
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
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
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
                $"Product_perfo_Report_{dateRange.StartDate:yyyyMMdd}_{dateRange.EndDate:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> InventoryReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);


            var inventoryReport = await financialReportingService.GetInventoryReportAsync(dateRange.StartDate, dateRange.EndDate);

            ViewBag.StartDate = dateRange.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = dateRange.EndDate.ToString("yyyy-MM-dd");

            return View(inventoryReport);
        }

        public async Task<IActionResult> ExportInventoryReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            if (dateRange.StartDate > dateRange.EndDate)
            {
                return BadRequest("Start date cannot be later than end date.");
            }
            var inventoryReport = await financialReportingService.GetInventoryReportAsync(dateRange.StartDate, dateRange.EndDate);

            // Configure report
            var reportConfig = new ReportConfig<InventoryReportDto>
            {
                ReportTitle = "Inventory Report",
                IsLandscape = true,
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
                Data = inventoryReport.Products,
                Columns = new List<ReportColumn<InventoryReportDto>>
        {
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Product",
                ValueSelector = p => p.ProductName,
                Width = 120
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "SKU",
                ValueSelector = p => p.SKU
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Category",
                ValueSelector = p => p.Category
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Stock",
                ValueSelector = p => p.StockQuantity.ToString()
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Min Level",
                ValueSelector = p => p.MinimumStockLevel.ToString()
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Price (₹)",
                ValueSelector = p => p.SellingPrice.ToString("N2")
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Sales",
                ValueSelector = p => p.TotalSales.ToString()
            },
            new ReportColumn<InventoryReportDto>
            {
                HeaderText = "Status",
                ValueSelector = p => p.ValueStatus
            }
        },
                SummaryItems = new List<string>
        {
            $"Total Products: {inventoryReport.TotalProductCount}",
            $"Low Stock Products: {inventoryReport.LowStockProductCount}",
            $"Date Range: {dateRange.StartDate:d} to {dateRange.EndDate:d}"
        }
            };

            // Generate PDF
            byte[] pdfBytes = pdfReportService.GenerateReport(reportConfig);
            return File(
                pdfBytes,
                "application/pdf",
                $"InventoryReport_{dateRange.StartDate:yyyyMMdd}_{dateRange.EndDate:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> ProfitLossReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange=dateRangeService.GetDatesRange(startDate, endDate);

            var orders = await financialReportingService.GetOrdersForReportAsync(dateRange.StartDate, dateRange.EndDate, true,true);

            var previousPeriodOrders = await financialReportingService.GetPreviousPeriodOrdersAsync(dateRange.StartDate, dateRange.EndDate);

            var reportData= financialReportingService.CalculateProfitLossReport(orders, previousPeriodOrders);
            reportData.StartDate = dateRange.StartDate;
            reportData.EndDate =dateRange.EndDate;

            // Prepare chart data
            var chartData = financialReportingService.PrepareChartData(orders, dateRange.StartDate, dateRange.EndDate);

            // Get top selling products
            var topProducts = await financialReportingService.GetTopSellingProductsAsync(dateRange.StartDate, dateRange.EndDate, 5);

            var viewModel = new ProfitLossReportMainViewModel
            {
                FinancialData = reportData,
                ChartData = chartData,
                TopSellingProducts = topProducts.Select(p => p.ProductName).ToList(),
                TopSellingProductsRevenue = topProducts.Select(p => p.Revenue).ToList(),
                TopSellingProductsProfit = topProducts.Select(p => p.Profit).ToList()
            };

            ViewBag.StartDate = dateRange.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate =  dateRange.EndDate.ToString("yyyy-MM-dd");

            return View(viewModel);
        }

        public async Task<IActionResult> ExportProfitLossReport(DateTime? startDate, DateTime? endDate)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            if (dateRange.StartDate > dateRange.EndDate)
            {
                return BadRequest("Start date cannot be later than end date.");
            }
            
            var orders = await financialReportingService.GetOrdersForReportAsync(dateRange.StartDate, dateRange.EndDate, true,true);

            var previousPeriodOrders = await financialReportingService.GetPreviousPeriodOrdersAsync(dateRange.StartDate, dateRange.EndDate);

            var reportData = financialReportingService.CalculateProfitLossReport(orders, previousPeriodOrders);

            // Configure report
            var reportConfig = new ReportConfig<Order>
            {
                ReportTitle = "Profit & Loss Report",
                StartDate = dateRange.StartDate,
                EndDate = dateRange.EndDate,
                Data = orders,
                Columns = new List<ReportColumn<Order>>
        {
            new ReportColumn<Order>
            {
                HeaderText = "Order #",
                ValueSelector = o => o.OrderNumber,
                Width = 100
            },
            new ReportColumn<Order>
            {
                HeaderText = "Date",
                ValueSelector = o => o.OrderDate.ToShortDateString()
            },
            new ReportColumn<Order>
            {
                HeaderText = "Revenue (₹)",
                ValueSelector = o => o.TotalAmount.ToString("N2")
            },
            new ReportColumn<Order>
            {
                HeaderText = "Cost (₹)",
                ValueSelector = o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity).ToString("N2")
            },
            new ReportColumn<Order>
            {
                HeaderText = "Profit (₹)",
                ValueSelector = o => (o.TotalAmount - o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity)).ToString("N2")
            },
            new ReportColumn<Order>
            {
                HeaderText = "Status",
                ValueSelector = o => o.OrderStatus,
            }
        },
                SummaryItems = new List<string>
        {
            // Financial Overview
            $"Total Revenue: ₹{reportData.TotalRevenue:N2}",
            $"Total Cost: ₹{reportData.TotalProductCost:N2}",
            $"Gross Profit: ₹{reportData.GrossProfit:N2}",
            $"Net Profit: ₹{reportData.NetProfit:N2}",
            
            // Profit Margins
            $"Gross Profit Margin: {reportData.GrossProfitMargin:N2}%",
            $"Net Profit Margin: {reportData.NetProfitMargin:N2}%",
            
            // Comparison
            $"Previous Period Net Profit: ₹{reportData.PreviousPeriodNetProfit:N2}",
            $"Profit Growth Rate: {reportData.ProfitGrowthRate:N2}%",
            
            // Taxes & Refunds
            $"Total Taxes Collected: ₹{reportData.TaxesCollected:N2}",
            $"Total Refunds: ₹{reportData.TotalRefunds:N2} ({reportData.RefundCount} refunds)"
        }
            };

            byte[] pdfBytes = pdfReportService.GenerateReport(reportConfig);
            return File(
                pdfBytes,
                "application/pdf",
                $"ProfitLossReport_{dateRange.StartDate:yyyyMMdd}_{dateRange.EndDate:yyyyMMdd}.pdf");
        }
        public async Task<IActionResult> TopSellingProducts(DateTime? startDate, DateTime? endDate, int count = 10)
        {
            var dateRange = dateRangeService.GetDatesRange(startDate, endDate);

            var topProducts = await financialReportingService.GetTopSellingProductsAsync(dateRange.StartDate, dateRange.EndDate, count);

            ViewBag.StartDate = dateRange.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = dateRange.EndDate.ToString("yyyy-MM-dd");
            ViewBag.Count = count;

            return View(topProducts);
        }
    
    }
}