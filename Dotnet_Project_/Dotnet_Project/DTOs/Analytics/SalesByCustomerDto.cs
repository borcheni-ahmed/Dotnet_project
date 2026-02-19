namespace Dotnet_Project.DTOs.Analytics
{
    public class SalesByCustomerDto
    {
        public string CustomerName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
    }
}