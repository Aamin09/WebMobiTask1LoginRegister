using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Task1LoginRegister.Services
{
    public class FinancialReportingService
    {
        private readonly WebMobiTask1DbContext context;

        public FinancialReportingService(WebMobiTask1DbContext context)
        {
            this.context = context;
        }

        // common method to get orders within a date range 
        public async Task<List<Order>> GetOrdersForReportAsync(DateTime startDate, DateTime endDate, bool includeRefunds = false, bool paidOnly = false)
        {
            var data = context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Subcategory)
                    .ThenInclude(s => s.Taxes)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.User)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate);
          
            if (paidOnly)
            {
                data = data.Where(o => o.PaymentStatus == "Paid" || o.PaymentStatus == "Refunded");
            }

            if (includeRefunds == true)
            {
                data = data.Include(o => o.RefundDetails);
            }

            return await data.OrderByDescending(o => o.OrderDate).ToListAsync();
        }

        // common method to get previous period order for comparison
        public async Task<List<Order>> GetPreviousPeriodOrdersAsync(DateTime startDate, DateTime endDate)
        {
            var previousPeriodLength = (endDate - startDate).TotalDays;
            var previousPeriodStartDate = startDate.AddDays(-previousPeriodLength);
            var previousPeriodEndDate = startDate.AddDays(-1);

            return await GetOrdersForReportAsync(previousPeriodStartDate, previousPeriodEndDate);
        }

        // get top selling produccts for report
        public async Task<List<TopSellingProductDTO>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int count)
        {
            return await context.OrderItems
                .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate && oi.Order.PaymentStatus == "Paid")
                .GroupBy(oi => new { oi.ProductId, oi.ProductName })
                .Select(g => new TopSellingProductDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.SnapshotPrice * oi.Quantity),
                    Profit = g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(count).ToListAsync();
        }

        // calculate profit loss report data
        public ProfitLossReportDto CalculateProfitLossReport(List<Order> orders, List<Order> previousPeriodOrders)
        {
            // Excluding canceled orders for financial calculations
            var validOrders = orders.Where(o => o.OrderStatus != "Cancelled" && o.PaymentStatus == "Paid").ToList();
            var validPreviousPeriodOrders = previousPeriodOrders.Where(o => o.OrderStatus != "Cancelled" && o.PaymentStatus == "Paid").ToList();

            // Geting only completed refunds
            var completedRefunds = orders.SelectMany(o => o.RefundDetails ?? new List<RefundDetailsModel>())
                                       .Where(r => r.Status == "Completed").ToList();
            var previousCompletedRefunds = previousPeriodOrders.SelectMany(o => o.RefundDetails ?? new List<RefundDetailsModel>())
                                                             .Where(r => r.Status == "Completed").ToList();

            var report = new ProfitLossReportDto
            {
                // Financial calculations for only valid orders
                ProductRevenue = validOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotPrice * oi.Quantity)),
                DeliveryChargeRevenue = validOrders.Sum(o => o.OrderItems.Sum(oi => oi.DeliveryCharge)),

                TotalProductCost = validOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity)),
                CGSTCollected = validOrders.Sum(o => o.OrderItems.Sum(oi => (oi.SnapshotCGSTPercentage / 100) * oi.SnapshotPrice * oi.Quantity)),
                SGSTCollected = validOrders.Sum(o => o.OrderItems.Sum(oi => (oi.SnapshotSGSTPercentage / 100) * oi.SnapshotPrice * oi.Quantity)),

                // orders count
                TotalOrders = orders.Count,
                CompletedOrders = orders.Count(o => o.OrderStatus == "Delivered"),
                PendingOrders = orders.Count(o => o.OrderStatus != "Delivered" && o.OrderStatus != "Cancelled"),
                CancelledOrders = orders.Count(o => o.OrderStatus == "Cancelled"),

                // Refunds
                TotalRefunds = completedRefunds.Sum(r => r.Amount),
                RefundCount = completedRefunds.Count
            };

            // Calculate derived metrics
            report.TotalRevenue = report.ProductRevenue + report.DeliveryChargeRevenue;
            report.TaxesCollected = report.CGSTCollected + report.SGSTCollected;

            // Profit calculations
            report.GrossProfit = report.ProductRevenue - report.TotalProductCost;
            report.OperatingProfit = report.GrossProfit + report.DeliveryChargeRevenue;
            report.NetProfit = report.OperatingProfit - report.TotalRefunds;

            // Profit margins (calculated against revenue before taxes)
            decimal marginBase = report.TotalRevenue;
            report.GrossProfitMargin = marginBase > 0 ? (report.GrossProfit / marginBase) * 100 : 0;
            report.OperatingProfitMargin = marginBase > 0 ? (report.OperatingProfit / marginBase) * 100 : 0;
            report.NetProfitMargin = marginBase > 0 ? (report.NetProfit / marginBase) * 100 : 0;

            // Payment method breakdown
            report.RevenueByPaymentMethod = validOrders.GroupBy(o => o.PaymentMethod ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotPrice * oi.Quantity + oi.DeliveryCharge)));

            // Previous period comparison
            decimal prevProductRevenue = validPreviousPeriodOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotPrice * oi.Quantity));
            decimal prevDeliveryRevenue = validPreviousPeriodOrders.Sum(o => o.OrderItems.Sum(oi => oi.DeliveryCharge));
            decimal prevProductCost = validPreviousPeriodOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity));
            decimal prevRefunds = previousCompletedRefunds.Sum(r => r.Amount);

            report.PreviousPeriodNetProfit = (prevProductRevenue + prevDeliveryRevenue) - prevProductCost - prevRefunds;

            // Growth rate calculation (handle division by zero and negative values)
            decimal profitDifference = report.NetProfit - report.PreviousPeriodNetProfit;
            decimal absPreviousProfit = Math.Abs(report.PreviousPeriodNetProfit);
            report.ProfitGrowthRate = absPreviousProfit > 0 ? (profitDifference / absPreviousProfit) * 100 :
                                      profitDifference > 0 ? 100 :
                                      profitDifference < 0 ? -100 : 0;

            // Category metrics
            report.RevenueByCategoryId = validOrders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.SnapshotPrice * oi.Quantity));

            report.ProfitByCategoryId = validOrders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity));

            // Subcategory metrics
            report.RevenueBySubcategoryId = validOrders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Subcategory.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.SnapshotPrice * oi.Quantity));

            report.ProfitBySubcategoryId = validOrders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Subcategory.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity));

            return report;
        }
        // calculate sales report metrics
        public void CalculateSalesReportMetrics(SalesReportViewModel report)
        {
            var validaorders = report.Orders.Where(o => o.OrderStatus != "Cancelled");
            var paidOrders = validaorders.Where(o => o.PaymentStatus == "Paid" );
            var failedOrders = validaorders.Where(o => o.PaymentStatus == "Failed");
            var cancelledOrders = report.Orders.Where(o => o.OrderStatus == "Cancelled");
           
            report.PaidOrders = paidOrders.Count();
            report.FailedOrders = failedOrders.Count();
            report.CancelledOrders = cancelledOrders.Count();

            report.TotalSales = paidOrders.Sum(o => o.TotalAmount);
            report.TotalCost = validaorders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity));

            report.GrossProfit = report.TotalSales - report.TotalCost;
            report.ProfitMargin = report.TotalSales > 0 ? (report.GrossProfit / report.TotalSales) * 100 : 0;

            report.TotalOrders = validaorders.Count();
            report.AverageOrderValue = report.TotalOrders > 0 ? report.TotalSales / report.TotalOrders : 0;

            report.DailySales = validaorders.GroupBy(o => o.OrderDate.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"),
                g => g.Sum(o => o.TotalAmount));

            report.PaymentMethodStats = validaorders.GroupBy(o => o.PaymentMethod)
                .ToDictionary(g => g.Key, g => g.Count());

            report.OrderStatusDistribution = report.Orders
                .GroupBy(o => o.OrderStatus)
                .ToDictionary(g => g.Key, g => g.Count());

            report.PaymentFailureRate = report.TotalOrders > 0
                ? (decimal)report.FailedOrders / report.TotalOrders * 100 : 0;

            report.CancellationRate = report.TotalOrders > 0
              ? (decimal)report.CancelledOrders / report.Orders.Count() * 100 : 0;

            CalculateCustomerMetrics(report);
            CalculatePerformaceMetrics(report);
        }

        private void CalculateCustomerMetrics(SalesReportViewModel report)
        {
            var distinctCustomers = report.Orders.Select(o => o.UserId).Distinct();

            report.NewCustomers = report.Orders.GroupBy(o => o.UserId)
                .Count(g => g.Count() == 1);

            report.ReturningCustomers = distinctCustomers.Count() - report.NewCustomers;

            report.CustomerRetentionRate = report.TotalCustomers > 0
                ? (decimal)report.ReturningCustomers / report.TotalCustomers * 100 : 0;
        }

        private void CalculatePerformaceMetrics(SalesReportViewModel report)
        {
            report.RefundRate = report.Orders.Count(o => o.OrderStatus == "Cancelled" && o.PaymentStatus == "Refunded") /
                 (decimal)(report.TotalOrders > 0 ? report.TotalOrders : 1) * 100;
            report.RefundAmount = report.Orders.Sum(o => o.RefundDetails.Sum(r => r.Amount));
        }
        // Prepare chart data for profit loss reports
        public ProfitLossChartDataViewModel PrepareChartData(List<Order> orders, DateTime startDate, DateTime endDate)
        {
            var chartData = new ProfitLossChartDataViewModel
            {
                TimeLabels = new List<string>(),
                RevenueData = new List<decimal>(),
                CostData = new List<decimal>(),
                ProfitData = new List<decimal>(),
                RefundData = new List<decimal>()
            };

            // Determine whether to group by day, week, or month based on date range
            TimeSpan range = endDate - startDate;

            if (range.TotalDays <= 31)
            {
                // Daily data for periods up to a month
                PrepareChartDataByDays(chartData, orders, startDate, endDate);
            }
            else if (range.TotalDays <= 90)
            {
                // Weekly data for periods up to three months
                PrepareChartDataByWeeks(chartData, orders, startDate, endDate);
            }
            else
            {
                // Monthly data for longer periods
                PrepareChartDataByMonths(chartData, orders, startDate, endDate);
            }

            // Category breakdown
            var categoryData = orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .OrderByDescending(g => g.Sum(oi => oi.SnapshotPrice * oi.Quantity))
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.SnapshotPrice * oi.Quantity));

            var categoryProfitData = orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .OrderByDescending(g => g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity))
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity));

            chartData.CategoryLabels = categoryData.Keys.ToList();
            chartData.CategoryRevenueData = categoryData.Values.ToList();
            chartData.CategoryProfitData = categoryProfitData.Values.ToList();

            return chartData;
        }

        // Helper methods for chart data preparation
        private void PrepareChartDataByDays(ProfitLossChartDataViewModel chartData, List<Order> orders, DateTime startDate, DateTime endDate)
        {
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dailyOrders = orders.Where(o => o.OrderDate.Date == date.Date).ToList();

                chartData.TimeLabels.Add(date.ToString("MMM dd"));
                chartData.RevenueData.Add(dailyOrders.Sum(o => o.TotalAmount));
                chartData.CostData.Add(dailyOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity)));
                chartData.ProfitData.Add(chartData.RevenueData.Last() - chartData.CostData.Last());
                chartData.RefundData.Add(dailyOrders.Sum(o => o.RefundDetails?.Sum(r => r.Amount) ?? 0));
            }
        }

        private void PrepareChartDataByWeeks(ProfitLossChartDataViewModel chartData, List<Order> orders, DateTime startDate, DateTime endDate)
        {
            for (var date = startDate; date <= endDate; date = date.AddDays(7))
            {
                var weekEnd = date.AddDays(6) > endDate ? endDate : date.AddDays(6);
                var weeklyOrders = orders.Where(o => o.OrderDate.Date >= date.Date && o.OrderDate.Date <= weekEnd.Date).ToList();

                chartData.TimeLabels.Add($"{date:MMM dd} - {weekEnd:MMM dd}");
                chartData.RevenueData.Add(weeklyOrders.Sum(o => o.TotalAmount));
                chartData.CostData.Add(weeklyOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity)));
                chartData.ProfitData.Add(chartData.RevenueData.Last() - chartData.CostData.Last());
                chartData.RefundData.Add(weeklyOrders.Sum(o => o.RefundDetails?.Sum(r => r.Amount) ?? 0));
            }
        }

        private void PrepareChartDataByMonths(ProfitLossChartDataViewModel chartData, List<Order> orders, DateTime startDate, DateTime endDate)
        {
            var months = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1;

            for (int i = 0; i < months; i++)
            {
                var monthStart = startDate.AddMonths(i);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1) > endDate ? endDate : monthStart.AddMonths(1).AddDays(-1);
                var monthlyOrders = orders.Where(o => o.OrderDate.Date >= monthStart.Date && o.OrderDate.Date <= monthEnd.Date).ToList();

                chartData.TimeLabels.Add(monthStart.ToString("MMM yyyy"));
                chartData.RevenueData.Add(monthlyOrders.Sum(o => o.TotalAmount));
                chartData.CostData.Add(monthlyOrders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity)));
                chartData.ProfitData.Add(chartData.RevenueData.Last() - chartData.CostData.Last());
                chartData.RefundData.Add(monthlyOrders.Sum(o => o.RefundDetails?.Sum(r => r.Amount) ?? 0));
            }
        }


        // inventory report data
        public async Task<InventoryReportMainViewModel> GetInventoryReportAsync(DateTime startDate, DateTime endDate)
        {
            var products = await context.Products.Include(p => p.Category).Include(p => p.Subcategory)
                    .Include(p => p.OrderItems)
                    .ThenInclude(oi => oi.Order)
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
                            .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate && oi.Order.OrderStatus == "Delivered" && oi.Order.PaymentStatus == "Paid")
                            .Sum(oi => oi.Quantity),
                        ValueStatus = p.StockQuantity <= p.MinimumStockLevel ? "Low Stock" : "Adequate Stock"
                    }).ToListAsync();

            var top20Products = products.OrderByDescending(p => p.StockQuantity).Take(20).ToList();

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
                StartDate = startDate,
                EndDate = endDate,
                TotalProductCount = products.Count(),
                LowStockProductCount = lowStockCount,
            };

            return finalData;
        }


        // Generate dashboard data
        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var startDate = DateTime.Today.AddMonths(-12);
            var endDate = DateTime.Today;

            var orders = await context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();

            var paidOrders = orders.Where(o => o.PaymentStatus == "Paid").ToList();

            var dashboardData = new DashboardDto
            {
                TotalOrders = orders.Count(o => o.OrderStatus != "Cancelled"),
                TotalRevenue = paidOrders.Sum(o => o.TotalAmount),
                TotalCustomers = await context.Userlogins.Where(u => u.Role == "User").CountAsync(),
                TotalProducts = await context.Products.CountAsync(),
                MonthlySales = GetMonthlySalesData(orders, startDate, endDate),
                OrderStatusDistribution = GetOrderStatusDistribution(orders),
            };

            // Get top products
            var topProducts = await GetTopSellingProductsAsync(startDate, endDate, 5);
            dashboardData.TopProducts = topProducts.Select(p => new ChartDataPoint
            {
                Label = p.ProductName,
                Value = p.Revenue
            }).ToList();

            return dashboardData;
        }

        private List<ChartDataPoint> GetMonthlySalesData(List<Order> orders, DateTime startDate, DateTime endDate)
        {
            var monthlySales = new List<ChartDataPoint>();
            var currentDate = new DateTime(startDate.Year, startDate.Month, 1);

            while (currentDate <= endDate)
            {
                var monthEnd = currentDate.AddMonths(1).AddDays(-1);
                var monthlyOrders = orders.Where(o =>
                    o.OrderDate >= currentDate &&
                    o.OrderDate <= monthEnd &&
                    o.PaymentStatus == "Paid").ToList();

                monthlySales.Add(new ChartDataPoint
                {
                    Label = currentDate.ToString("MMM yyyy"),
                    Value = monthlyOrders.Sum(o => o.TotalAmount)
                });

                currentDate = currentDate.AddMonths(1);
            }

            return monthlySales;
        }

        private List<ChartDataPoint> GetOrderStatusDistribution(List<Order> orders)
        {
            return orders
                .GroupBy(o => o.OrderStatus)
                .Select(g => new ChartDataPoint
                {
                    Label = g.Key,
                    Value = g.Count()
                })
                .ToList();
        }
    }
}
