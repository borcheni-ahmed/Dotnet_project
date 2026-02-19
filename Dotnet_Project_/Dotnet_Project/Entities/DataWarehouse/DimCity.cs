using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.DataWarehouse
{
    [Table("DimCity")]
    public class DimCity
    {
        [Key]
        public int CityKey { get; set; }

        public int CityID { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        [StringLength(50)]
        public string? StateProvinceName { get; set; }

        [StringLength(60)]
        public string? CountryName { get; set; }

        [StringLength(30)]
        public string? Continent { get; set; }

        [StringLength(50)]
        public string? SalesTerritory { get; set; }

        [StringLength(50)]
        public string? Region { get; set; }

        [StringLength(50)]
        public string? Subregion { get; set; }

        public long? LatestRecordedPopulation { get; set; }

        // Navigation property
        public virtual ICollection<FactSales> FactSales { get; set; }
    }
}