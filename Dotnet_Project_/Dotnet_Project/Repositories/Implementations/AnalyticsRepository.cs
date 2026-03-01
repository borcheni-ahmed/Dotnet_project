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
                .Join(_context.DimDates,
                    f => f.DateKey,
                    d => d.DateKey,
                    (f, d) => new { f, d })
                .AsQueryable();

            if (year.HasValue)
                query = query.Where(x => x.d.Year == year.Value);

            if (month.HasValue)
                query = query.Where(x => x.d.Month == month.Value);

            var result = await query
                .GroupBy(x => new
                {
                    x.d.Year,
                    x.d.Month,
                    x.d.MonthName,
                    x.d.Quarter
                })
                .Select(g => new SalesByPeriodDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = g.Key.MonthName,
                    Quarter = g.Key.Quarter,
                    TotalSales = g.Sum(x => x.f.TotalAmount),
                    TotalOrders = g.Count(),
                    AverageOrderValue = g.Average(x => x.f.TotalAmount),
                    TotalProfit = g.Sum(x => x.f.LineProfit ?? 0)
                })
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Month)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalesByProductDto>> GetSalesByProductAsync(int topN = 10)
        {
            
            var result = await _context.FactSales
                .Join(_context.DimProducts,
                    f => f.ProductKey,
                    p => p.ProductKey,
                    (f, p) => new { f, p })
                .Where(x => x.p.IsCurrent)
                .GroupBy(x => new
                {
                    x.p.ProductName,
                    x.p.Brand
                })
                .Select(g => new SalesByProductDto
                {
                    ProductName = g.Key.ProductName,
                    Brand = g.Key.Brand,
                    Category = "N/A",
                    QuantitySold = g.Sum(x => x.f.Quantity),
                    TotalRevenue = g.Sum(x => x.f.TotalAmount),
                    TotalProfit = g.Sum(x => x.f.LineProfit ?? 0),
                    AveragePrice = g.Average(x => x.f.UnitPrice)
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(topN)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalesByCustomerDto>> GetSalesByCustomerAsync(int topN = 10)
        {
            
            var result = await _context.FactSales
                .Join(_context.DimCustomers,
                    f => f.CustomerKey,
                    c => c.CustomerKey,
                    (f, c) => new { f, c })
                .Join(_context.DimDates,
                    x => x.f.DateKey,
                    d => d.DateKey,
                    (x, d) => new { x.f, x.c, d })
                .Where(x => x.c.IsCurrent)
                .GroupBy(x => new
                {
                    x.c.CustomerName,
                    x.c.CityName,
                    x.c.Country
                })
                .Select(g => new SalesByCustomerDto
                {
                    CustomerName = g.Key.CustomerName,
                    City = g.Key.CityName,
                    Country = g.Key.Country,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Sum(x => x.f.TotalAmount),
                    AverageOrderValue = g.Average(x => x.f.TotalAmount),
                    LastPurchaseDate = g.Max(x => x.d.FullDate)
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
           
            var query = _context.FactSales.AsQueryable();

            if (startDate.HasValue || endDate.HasValue)
            {
               
                var dateFilteredKeys = _context.DimDates.AsQueryable();

                if (startDate.HasValue)
                    dateFilteredKeys = dateFilteredKeys.Where(d => d.FullDate >= startDate.Value);

                if (endDate.HasValue)
                    dateFilteredKeys = dateFilteredKeys.Where(d => d.FullDate <= endDate.Value);

                var validDateKeys = dateFilteredKeys.Select(d => d.DateKey);
                query = query.Where(f => validDateKeys.Contains(f.DateKey));
            }

            var totalRevenue = await query.SumAsync(f => f.TotalAmount);
            var totalProfit = await query.SumAsync(f => f.LineProfit ?? 0);
            var totalOrders = await query.CountAsync();
            var totalCustomers = await query.Select(f => f.CustomerKey).Distinct().CountAsync();
            var totalProducts = await query.Select(f => f.ProductKey).Distinct().CountAsync();

            return new KPIsDto
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
        }

        public async Task<IEnumerable<SalesTrendDto>> GetSalesTrendAsync(int months = 12)
        {
          
            var raw = await _context.FactSales
                .Join(_context.DimDates,
                    f => f.DateKey,
                    d => d.DateKey,
                    (f, d) => new { f, d })
                .GroupBy(x => new
                {
                    x.d.Year,
                    x.d.Month,
                    x.d.MonthName
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.MonthName,
                    Revenue = g.Sum(x => x.f.TotalAmount),
                    Orders = g.Count(),
                    Profit = g.Sum(x => x.f.LineProfit ?? 0)
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
                .AsNoTracking()
                .Join(_context.DimCustomers,
                    f => f.CustomerKey,
                    c => c.CustomerKey,
                    (f, c) => new { f, c })
                .Where(x => x.c.IsCurrent)
                .GroupBy(x => x.c.Country)
                .Select(g => new SalesByCountryDto
                {
                    Country = g.Key ?? "Unknown",
                    TotalRevenue = g.Sum(x => x.f.TotalAmount),
                    TotalOrders = g.Count()
                })
                .OrderByDescending(c => c.TotalRevenue)
                //.Take(20)
                .ToListAsync();

            return result;
        }
    }
}