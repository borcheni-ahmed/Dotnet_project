using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.DataWarehouse
{
    [Table("DimCustomer")]
    public class DimCustomer
    {
        [Key]
        public int CustomerKey { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string? CustomerCategory { get; set; }

        [StringLength(50)]
        public string? BuyingGroup { get; set; }

        [StringLength(100)]
        public string? PrimaryContact { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string? CityName { get; set; }

        [StringLength(50)]
        public string? StateProvince { get; set; }

        [StringLength(60)]
        public string? Country { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        // Navigation property
        public virtual ICollection<FactSales> FactSales { get; set; }
    }
}