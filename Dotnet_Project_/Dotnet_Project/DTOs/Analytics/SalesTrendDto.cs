namespace Dotnet_Project.DTOs.Analytics
{
    public class SalesTrendDto
    {
        public DateTime Date { get; set; }
        public string Period { get; set; } // "2016-01", "2016-Q1", etc.
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public decimal Profit { get; set; }
    }
}