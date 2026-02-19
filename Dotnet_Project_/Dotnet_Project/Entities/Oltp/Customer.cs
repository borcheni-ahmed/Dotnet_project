using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.Oltp
{
    [Table("Customers", Schema = "Sales")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        public int? BuyingGroupID { get; set; }

        public int CustomerCategoryID { get; set; }

        public int? PrimaryContactPersonID { get; set; }

        public int? DeliveryMethodID { get; set; }

        public int DeliveryCityID { get; set; }

        public int PostalCityID { get; set; }

      
        public DateTime? AccountOpenedDate { get; set; }

        public decimal StandardDiscountPercentage { get; set; }

        public bool IsStatementSent { get; set; }

        public bool IsOnCreditHold { get; set; }

        public int PaymentDays { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string? FaxNumber { get; set; }

        public string? DeliveryRun { get; set; }

        public string? RunPosition { get; set; }

        [StringLength(256)]
        public string? WebsiteURL { get; set; }

        [StringLength(60)]
        public string DeliveryAddressLine1 { get; set; }

        [StringLength(60)]
        public string? DeliveryAddressLine2 { get; set; }

        [StringLength(10)]
        public string? DeliveryPostalCode { get; set; }

        [StringLength(60)]
        public string PostalAddressLine1 { get; set; }

        public int BillToCustomerID { get; set; }

        [StringLength(60)]
        public string? PostalAddressLine2 { get; set; }

        [StringLength(10)]
        public string? PostalPostalCode { get; set; }

        public int LastEditedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ValidFrom { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ValidTo { get; set; }

        public decimal? CreditLimit { get; set; }
        public int? AlternateContactPersonID { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}