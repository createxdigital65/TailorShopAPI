namespace TailorShop.API.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
}
