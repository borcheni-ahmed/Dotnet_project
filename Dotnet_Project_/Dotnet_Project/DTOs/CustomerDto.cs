
using System.ComponentModel.DataAnnotations;

namespace Dotnet_Project.DTOs
{
    public class CustomerDto
    {
        public int? CustomerID { get; set; }

        [Required(ErrorMessage = "Le nom du client est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string CustomerName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(60)]
        public string? DeliveryAddressLine1 { get; set; }

        [StringLength(60)]
        public string? DeliveryAddressLine2 { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        public int? DeliveryCityID { get; set; }

        public string? CityName { get; set; }

        [Range(0, 100, ErrorMessage = "Le discount doit être entre 0 et 100")]
        public decimal StandardDiscountPercentage { get; set; }

        public bool IsOnCreditHold { get; set; }

        public int PaymentDays { get; set; }

        // Pour afficher le nombre de commandes
        public int? TotalOrders { get; set; }
    }
}