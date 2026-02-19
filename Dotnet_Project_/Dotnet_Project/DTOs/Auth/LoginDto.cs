
using System.ComponentModel.DataAnnotations;

namespace Dotnet_Project.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string Password { get; set; }
    }
}