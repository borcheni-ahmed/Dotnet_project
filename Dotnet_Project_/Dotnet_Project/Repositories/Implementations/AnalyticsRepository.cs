
using Dotnet_Project.Data.DataWarehouse;
using Dotnet_Project.DTOs.Analytics;
using Dotnet_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Project.Repositories.Implementations
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly DwDbContext _context;

        public AnalyticsRepository(DwDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync(
            int? year = null,
            int? month = null)
        {
            var query = _context.FactSales
                .Include(f => f.DimDate)
                .AsQueryable();

            if (year.HasValue)
                query = query.Where(f => f.DimDate.Year == year.Value);

            if (month.HasValue)
                query = query.Where(f => f.DimDate.Month == month.Value);

            var result = await query
                .GroupBy(f => new
                {
                    f.DimDate.Year,
                    f.DimDate.Month,
                    f.DimDate.MonthName,
                    f.DimDate.Quarter
                })
                .Select(g => new SalesByPeriodDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = g.Key.MonthName,
                    Quarter = g.Key.Quarter,
                    TotalSales = g.Sum(f => f.TotalAmount),
                    TotalOrders = g.Count(),
                    AverageOrderValue = g.Average(f => f.TotalAmount),
                    TotalProfit = g.Sum(f => f.LineProfit ?? 0)
                })
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Month)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalesByProductDto>> GetSalesByProductAsync(int topN = 10)
        {
            var result = await _context.FactSales
                .Include(f => f.DimProduct)
                .Where(f => f.DimProduct.IsCurrent)
                .GroupBy(f => new
                {
                    f.DimProduct.ProductName,
                    f.DimProduct.Brand
                })
                .Select(g => new SalesByProductDto
                {
                    ProductName = g.Key.ProductName,
                    Brand = g.Key.Brand,
                    Category = "N/A", // Ajoutez si vous avez cette info
                    QuantitySold = g.Sum(f => f.Quantity),
                    TotalRevenue = g.Sum(f => f.TotalAmount),
                    TotalProfit = g.Sum(f => f.LineProfit ?? 0),
                    AveragePrice = g.Average(f => f.UnitPrice)
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(topN)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalesByCustomerDto>> GetSalesByCustomerAsync(int topN = 10)
        {
            var result = await _context.FactSales
                .Include(f => f.DimCustomer)
                .Include(f => f.DimDate)
                .Where(f => f.DimCustomer.IsCurrent)
                .GroupBy(f => new
                {
                    f.DimCustomer.CustomerName,
                    f.DimCustomer.CityName,
                    f.DimCustomer.Country
                })
                .Select(g => new SalesByCustomerDto
                {
                    CustomerName = g.Key.CustomerName,
                    City = g.Key.CityName,
                    Country = g.Key.Country,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Sum(f => f.TotalAmount),
                    AverageOrderValue = g.Average(f => f.TotalAmount),
                    LastPurchaseDate = g.Max(f => f.DimDate.FullDate)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(topN)
                .ToListAsync();

            return result;
        }

        public async Task<KPIsDto> GetKPIsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _context.FactSales
                .Include(f => f.DimDate)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(f => f.DimDate.FullDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(f => f.DimDate.FullDate <= endDate.Value);

            var totalRevenue = await query.SumAsync(f => f.TotalAmount);
            var totalProfit = await query.SumAsync(f => f.LineProfit ?? 0);
            var totalOrders = await query.CountAsync();
            var totalCustomers = await query
                .Select(f => f.CustomerKey)
                .Distinct()
                .CountAsync();
            var totalProducts = await query
                .Select(f => f.ProductKey)
                .Distinct()
                .CountAsync();

            var kpis = new KPIsDto
            {
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit,
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0,
                ProfitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue) * 100 : 0,
                StartDate = startDate,
                EndDate = endDate
            };

            return kpis;
        }

        public async Task<IEnumerable<SalesTrendDto>> GetSalesTrendAsync(int months = 12)
        {
            var raw = await _context.FactSales
                .Include(f => f.DimDate)
                .GroupBy(f => new
                {
                    f.DimDate.Year,
                    f.DimDate.Month,
                    f.DimDate.MonthName
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.MonthName,
                    Revenue = g.Sum(f => f.TotalAmount),
                    Orders = g.Count(),
                    Profit = g.Sum(f => f.LineProfit ?? 0)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Take(months)
                .ToListAsync();

            var result = raw
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .Select(x => new SalesTrendDto
                {
                    Date = new DateTime(x.Year, x.Month, 1),
                    Period = $"{x.Year}-{x.Month:00}",
                    Revenue = x.Revenue,
                    Orders = x.Orders,
                    Profit = x.Profit
                });

            return result;
        }

        public async Task<IEnumerable<SalesByCountryDto>> GetSalesByCountryAsync()
        {
            var result = await _context.FactSales
                .Include(f => f.DimCustomer)
                .Where(f => f.DimCustomer.IsCurrent)
                .GroupBy(f => f.DimCustomer.Country)
                .Select(g => new SalesByCountryDto
                {
                    Country = g.Key ?? "Unknown",
                    TotalRevenue = g.Sum(f => f.TotalAmount),
                    TotalOrders = g.Count()
                })
                .OrderByDescending(c => c.TotalRevenue)
                .ToListAsync();

            return result;
        }
    }
}