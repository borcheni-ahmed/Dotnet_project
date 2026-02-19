
using System.ComponentModel.DataAnnotations;

namespace Dotnet_Project.DTOs
{
    public class OrderDto
    {
        public int? OrderID { get; set; }

        [Required(ErrorMessage = "Le CustomerID est requis")]
        public int CustomerID { get; set; }

        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "La date de commande est requise")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "La date de livraison prévue est requise")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [StringLength(100)]
        public string? CustomerPurchaseOrderNumber { get; set; }

        public string? Comments { get; set; }

        public string? DeliveryInstructions { get; set; }

        public bool IsUndersupplyBackordered { get; set; }

        public DateTime? PickingCompletedWhen { get; set; }

        // Informations complémentaires
        public string? Status { get; set; } // "En attente", "Livrée", "Annulée"
    }
}