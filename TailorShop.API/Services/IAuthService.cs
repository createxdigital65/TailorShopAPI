using TailorShop.API.DTOs;

namespace TailorShop.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
    }
}
