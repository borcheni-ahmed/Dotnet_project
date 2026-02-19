
using Dotnet_Project.DTOs.Auth;
using Dotnet_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Connexion d'un utilisateur
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
                return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect" });

            return Ok(result);
        }

        /// <summary>
        /// Inscription d'un nouvel utilisateur
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vérifier si l'utilisateur existe déjà
            if (await _authService.UserExistsAsync(registerDto.Username))
                return BadRequest(new { message = "Ce nom d'utilisateur existe déjà" });

            var result = await _authService.RegisterAsync(registerDto);

            if (result == null)
                return BadRequest(new { message = "Erreur lors de l'inscription" });

            return Ok(result);
        }

        /// <summary>
        /// Vérifier si le token est valide (endpoint de test)
        /// </summary>
        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = "Token valide",
                username = username,
                role = role
            });
        }
    }
}