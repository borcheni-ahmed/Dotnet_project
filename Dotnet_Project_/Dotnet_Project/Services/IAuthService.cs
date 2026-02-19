
using Dotnet_Project.DTOs.Auth;


namespace Dotnet_Project.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<bool> UserExistsAsync(string username);
    }
}