using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.DataWarehouse
{
    [Table("DimProduct")]
    public class DimProduct
    {
        [Key]
        public int ProductKey { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(50)]
        public string? Brand { get; set; }

        [StringLength(20)]
        public string? Color { get; set; }

        [StringLength(20)]
        public string? Size { get; set; }

        [StringLength(100)]
        public string? SupplierName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RecommendedRetailPrice { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? TypicalWeightPerUnit { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        // Navigation property
        public virtual ICollection<FactSales> FactSales { get; set; }
    }
}