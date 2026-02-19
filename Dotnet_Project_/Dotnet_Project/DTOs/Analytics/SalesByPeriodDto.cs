namespace Dotnet_Project.DTOs.Analytics
{
    public class SalesByPeriodDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Quarter { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal TotalProfit { get; set; }
    }
}