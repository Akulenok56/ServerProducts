using Products24Backend.DTO;
using Products24Backend.Models;

namespace Products24Backend.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
