using Task1LoginRegister.Models;

namespace Task1LoginRegister.DTOs
{

    public class SalesReportViewModel
    {
        // Date Range
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Core Order Data
        public IEnumerable<Order> Orders { get; set; }

        // Financial Metrics
        public decimal TotalSales { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }

        // Sales Breakdown
        public Dictionary<string, decimal> DailySales { get; set; }
        public Dictionary<string, int> PaymentMethodStats { get; set; }

        // Customer Insights
        public int NewCustomers { get; set; }
        public int ReturningCustomers { get; set; }
        public decimal CustomerRetentionRate { get; set; }

        // Order Status Analysis
        public Dictionary<string, int> OrderStatusDistribution { get; set; }

        // Performance Metrics
        public decimal RefundRate { get; set; }
        public decimal AverageTimeToShip { get; set; } // In days

        // Computed Properties
        public decimal TotalCustomers => NewCustomers + ReturningCustomers;
        public decimal PaymentFailureRate { get; set; }
        public decimal CancellationRate { get; set; }

        // Order Metrics
        public int PaidOrders { get; set; }
        public int FailedOrders { get; set; }
        public int CancelledOrders { get; set; }


       
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

    public class InventoryReportDto
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
        public int TotalSales { get; set; }
        public string ValueStatus { get; set; }
    }
    public class InventoryChartDataViewModel
    {
        public List<string> ProductNames { get; set; }
        public List<int> StockQuantities { get; set; }
        public List<int> TotalSales { get; set; }
        public List<string> StockStatusLabels { get; set; }
        public List<int> StockStatusData { get; set; }
    }


    public class InventoryReportMainViewModel
    {
        public List<InventoryReportDto> Products { get; set; }
        public InventoryChartDataViewModel ChartData { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalProductCount { get; set; }
        public int LowStockProductCount { get; set; }
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


    public class ProfitLossReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Revenue Breakdown
        public decimal TotalRevenue { get; set; }
        public decimal ProductRevenue { get; set; }
        public decimal DeliveryChargeRevenue { get; set; }

        // Cost Breakdown
        public decimal TotalProductCost { get; set; }
        public decimal TaxesCollected { get; set; }
        public decimal CGSTCollected { get; set;}
        public decimal SGSTCollected { get; set; }

        public decimal TotalRefunds { get; set; }
        public int RefundCount { get; set; }

        // Order Statistics
        public int TotalOrders { get; set; }
        public  int CompletedOrders  { get; set; }
        public int PendingOrders { get; set; }

        // Profit Calculations
        public decimal GrossProfit { get; set; }
        public decimal OperatingProfit { get; set; }
        public decimal NetProfit { get; set; }

        public decimal GrossProfitMargin { get; set; }
        public decimal OperatingProfitMargin { get; set; }
        public decimal NetProfitMargin { get; set; }


        public decimal PreviousPeriodNetProfit { get; set; }
        public decimal ProfitGrowthRate { get; set; }

        // Category and Subcategory Performance
        public Dictionary<string,decimal> RevenueByCategoryId { get; set; }
        public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; }

        public Dictionary<string, decimal> ProfitByCategoryId { get; set; }
        public Dictionary<string, decimal> RevenueBySubcategoryId { get; set; }
        public Dictionary<string, decimal> ProfitBySubcategoryId { get; set; }


    }

    public class TopSellingProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
    }
}
