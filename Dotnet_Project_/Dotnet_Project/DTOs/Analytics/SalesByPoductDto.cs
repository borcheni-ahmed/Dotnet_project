namespace Dotnet_Project.DTOs.Analytics
{
    public class SalesByProductDto
    {
        public string ProductName { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal AveragePrice { get; set; }
    }
}