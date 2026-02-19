namespace Dotnet_Project.DTOs.Analytics
{
    public class KPIsDto
    {
        // Indicateurs principaux
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }

        // Moyennes
        public decimal AverageOrderValue { get; set; }
        public decimal ProfitMargin { get; set; } // En pourcentage

        // Comparaison avec période précédente
        public decimal RevenueGrowth { get; set; } // En pourcentage
        public decimal OrderGrowth { get; set; } // En pourcentage
        public decimal ProfitGrowth { get; set; } // En pourcentage

        // Période d'analyse
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}