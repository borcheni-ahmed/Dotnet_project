
using System.ComponentModel.DataAnnotations;

namespace Dotnet_Project.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 100 caractères")]
        public string Username { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Le rôle est requis")]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Le rôle doit être 'Admin' ou 'User'")]
        public string Role { get; set; }
    }
}