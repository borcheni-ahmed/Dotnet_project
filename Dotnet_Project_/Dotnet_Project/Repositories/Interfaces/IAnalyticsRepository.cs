
using Dotnet_Project.DTOs.Analytics;


namespace Dotnet_Project.Repositories.Interfaces
{
    public interface IAnalyticsRepository
    {
        // Ventes par période
        Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync(
            int? year = null,
            int? month = null);

        // Ventes par produit
        Task<IEnumerable<SalesByProductDto>> GetSalesByProductAsync(int topN = 10);

        // Ventes par client
        Task<IEnumerable<SalesByCustomerDto>> GetSalesByCustomerAsync(int topN = 10);

        // KPIs globaux
        Task<KPIsDto> GetKPIsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        // Tendance des ventes
        Task<IEnumerable<SalesTrendDto>> GetSalesTrendAsync(int months = 12);

        // Ventes par pays
        Task<IEnumerable<SalesByCountryDto>> GetSalesByCountryAsync();
    }

    // DTO supplémentaire pour les ventes par pays
    public class SalesByCountryDto
    {
        public string Country { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }
}