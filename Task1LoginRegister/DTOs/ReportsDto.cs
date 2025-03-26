using Task1LoginRegister.Models;

namespace Task1LoginRegister.DTOs
{

    public class SalesReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }

        // Sales by date for chart
        public Dictionary<string, decimal> DailySales
        {
            get
            {
                return Orders.GroupBy(o => o.OrderDate.Date)
                     .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Sum(o => o.TotalAmount));
            }
        }
        public Dictionary<string, int> PaymentMethodStats
        {
            get
            {
                return Orders.GroupBy(o => o.PaymentMethod)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
        }
    }

    public class ProductPerformanceDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue => OrderCount > 0 ? TotalRevenue / OrderCount : 0;
    }

    public class CustomerReportDto
    {
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrdeValue { get; set; }
        public DateTime FisrtPurchaseDate { get; set; }
        public DateTime LastPurchaseDate { get; set; }
    }

    public class InventoryStatusDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public decimal Price { get; set; }
        public decimal SellingPrice { get; set; }
        public bool Status { get; set; }
        public int StockQuantity { get; set; }
        public int MinimumStockLevel { get; set; }
    }

    public class DashboardDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public List<ChartDataPoint> MonthlySales { get; set; }
        public List<ChartDataPoint> TopProducts { get; set; }
        public List<ChartDataPoint> OrderStatusDistribution { get; set; }
    }

    public class ChartDataPoint
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }
}
