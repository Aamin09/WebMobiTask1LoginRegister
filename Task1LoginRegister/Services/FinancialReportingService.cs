using Microsoft.EntityFrameworkCore;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Models;

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
        public async Task<List<Order>> GetOrdersForReportAsync(DateTime startDate, DateTime endDate, bool includeRefunds = false)
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

            if (includeRefunds)
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
            var report = new ProfitLossReportDto
            {
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                ProductRevenue = orders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotPrice * oi.Quantity)),
                DeliveryChargeRevenue = orders.Sum(o => o.OrderItems.Sum(oi => oi.DeliveryCharge)),

                TotalProductCost = orders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCostPrice * oi.Quantity)),
                CGSTCollected = orders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotCGSTPercentage * oi.SnapshotPrice * oi.Quantity / 100)),
                SGSTCollected = orders.Sum(o => o.OrderItems.Sum(oi => oi.SnapshotSGSTPercentage * oi.SnapshotPrice * oi.Quantity / 100)),

                TotalOrders = orders.Count,
                CompletedOrders = orders.Count(o => o.OrderStatus == "Completed"),
                PendingOrders = orders.Count(o => o.OrderStatus != "Completed")
            };

            report.TaxesCollected = report.CGSTCollected + report.SGSTCollected;

            report.TotalRefunds = orders.Sum(o => o.RefundDetails?.Sum(r => r.Amount) ?? 0);
            report.RefundCount = orders.Sum(o => o.RefundDetails?.Count ?? 0);

            report.RevenueByPaymentMethod = orders.GroupBy(o => o.PaymentMethod ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount));

            report.GrossProfit = report.ProductRevenue - report.TotalProductCost;
            report.OperatingProfit = report.GrossProfit - report.TotalRefunds;
            report.NetProfit = report.OperatingProfit;


            report.GrossProfitMargin = report.TotalRevenue > 0 ? (report.GrossProfit / report.TotalRevenue) * 100 : 0;
            report.OperatingProfitMargin = report.TotalRevenue > 0 ? (report.OperatingProfit / report.TotalRevenue) * 100 : 0;
            report.NetProfitMargin = report.TotalRevenue > 0 ? (report.NetProfit / report.TotalRevenue) * 100 : 0;

            report.ProfitGrowthRate = report.PreviousPeriodNetProfit > 0 ?
                ((report.NetProfit - report.PreviousPeriodNetProfit) / report.PreviousPeriodNetProfit) * 100 : 0;


            report.RevenueByCategoryId = orders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.SnapshotPrice * oi.Quantity));

            report.ProfitByCategoryId = orders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity));

            report.RevenueBySubcategoryId = orders.SelectMany(o => o.OrderItems)
             .GroupBy(oi => oi.Product.Subcategory.Name)
             .ToDictionary(g => g.Key, g => g.Sum(oi => oi.SnapshotPrice * oi.Quantity));

            report.ProfitBySubcategoryId = orders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Subcategory.Name)
                .ToDictionary(g => g.Key, g => g.Sum(oi => (oi.SnapshotPrice - oi.SnapshotCostPrice) * oi.Quantity));

            return report;
        }

        // calculate sales report metrics
        public void CalculateSalesReportMetrics(SalesReportViewModel report)
        {
            var validaorders = report.Orders.Where(o => o.OrderStatus != "Cancelled");
            var paidOrders = validaorders.Where(o => o.PaymentStatus == "Paid");
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
    }
}
