
using Dotnet_Project.DTOs.Analytics;
using Dotnet_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        /// <summary>
        /// Obtenir les ventes par période
        /// </summary>
        /// <param name="year">Année (optionnel)</param>
        /// <param name="month">Mois (optionnel)</param>
        [HttpGet("sales/period")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<SalesByPeriodDto>>> GetSalesByPeriod(
            [FromQuery] int? year = null,
            [FromQuery] int? month = null)
        {
            var result = await _analyticsRepository.GetSalesByPeriodAsync(year, month);
            return Ok(result);
        }

        /// <summary>
        /// Obtenir les ventes par produit (top N)
        /// </summary>
        /// <param name="top">Nombre de produits à retourner (défaut: 10)</param>
        [HttpGet("sales/product")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<SalesByProductDto>>> GetSalesByProduct(
            [FromQuery] int top =10)
        {
            if (top < 1 || top > 100)
                return BadRequest(new { message = "Le paramètre 'top' doit être entre 1 et 100" });

            var result = await _analyticsRepository.GetSalesByProductAsync(top);
            return Ok(result);
        }

        /// <summary>
        /// Obtenir les ventes par client (top N)
        /// </summary>
        /// <param name="top">Nombre de clients à retourner (défaut: 10)</param>
        [HttpGet("sales/customer")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<SalesByCustomerDto>>> GetSalesByCustomer(
            [FromQuery] int top = 10)
        {
            if (top < 1 || top > 100)
                return BadRequest(new { message = "Le paramètre 'top' doit être entre 1 et 100" });

            var result = await _analyticsRepository.GetSalesByCustomerAsync(top);
            return Ok(result);
        }

        /// <summary>
        /// Obtenir les indicateurs clés (KPIs)
        /// </summary>
        /// <param name="startDate">Date de début (optionnel)</param>
        /// <param name="endDate">Date de fin (optionnel)</param>
        [HttpGet("kpis")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<KPIsDto>> GetKPIs(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                return BadRequest(new { message = "La date de début doit être antérieure à la date de fin" });

            var result = await _analyticsRepository.GetKPIsAsync(startDate, endDate);
            return Ok(result);
        }

        /// <summary>
        /// Obtenir la tendance des ventes sur N mois
        /// </summary>
        /// <param name="months">Nombre de mois (défaut: 12)</param>
        [HttpGet("sales/trend")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<SalesTrendDto>>> GetSalesTrend(
            [FromQuery] int months = 12)
        {
            if (months < 1 || months > 60)
                return BadRequest(new { message = "Le paramètre 'months' doit être entre 1 et 60" });

            var result = await _analyticsRepository.GetSalesTrendAsync(months);
            return Ok(result);
        }

        /// <summary>
        /// Obtenir les ventes par pays
        /// </summary>
        [HttpGet("sales/country")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<SalesByCountryDto>>> GetSalesByCountry()
        {
            var result = await _analyticsRepository.GetSalesByCountryAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtenir un rapport complet (dashboard)
        /// </summary>
        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var kpis = await _analyticsRepository.GetKPIsAsync();
            var salesByMonth = await _analyticsRepository.GetSalesByPeriodAsync();
            var topProducts = await _analyticsRepository.GetSalesByProductAsync(5);
            var topCustomers = await _analyticsRepository.GetSalesByCustomerAsync(5);
            var salesByCountry = await _analyticsRepository.GetSalesByCountryAsync();

            var dashboard = new
            {
                KPIs = kpis,
                SalesByMonth = salesByMonth.TakeLast(12),
                TopProducts = topProducts,
                TopCustomers = topCustomers,
                SalesByCountry = salesByCountry.Take(10)
            };

            return Ok(dashboard);
        }
    }
}