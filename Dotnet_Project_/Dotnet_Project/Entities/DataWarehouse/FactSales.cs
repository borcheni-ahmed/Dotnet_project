using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.DataWarehouse
{
    [Table("FactSales")]
    public class FactSales
    {
        [Key]
        public long SalesKey { get; set; }

        public int DateKey { get; set; }

        public int CustomerKey { get; set; }

        public int ProductKey { get; set; }

        public int CityKey { get; set; }

        public int InvoiceID { get; set; }

        public int InvoiceLineID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal TaxRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LineProfit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExtendedPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Navigation properties
        [ForeignKey("DateKey")]
        public virtual DimDate DimDate { get; set; }

        [ForeignKey("CustomerKey")]
        public virtual DimCustomer DimCustomer { get; set; }

        [ForeignKey("ProductKey")]
        public virtual DimProduct DimProduct { get; set; }

        [ForeignKey("CityKey")]
        public virtual DimCity DimCity { get; set; }
    }
}