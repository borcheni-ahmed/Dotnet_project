using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.Oltp
{
    [Table("Orders", Schema = "Sales")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public int SalespersonPersonID { get; set; }

        public int? PickedByPersonID { get; set; }

        public int ContactPersonID { get; set; }

        public int? BackorderOrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        [StringLength(100)]
        public string? CustomerPurchaseOrderNumber { get; set; }

        public bool IsUndersupplyBackordered { get; set; }

        [StringLength(100)]
        public string? Comments { get; set; }

        [StringLength(100)]
        public string? DeliveryInstructions { get; set; }

        [StringLength(100)]
        public string? InternalComments { get; set; }

        public DateTime? PickingCompletedWhen { get; set; }

        public int LastEditedBy { get; set; }

        public DateTime LastEditedWhen { get; set; }

        // Navigation property
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
}