using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorShop.API.DTOs;
using TailorShop.API.Services;

namespace TailorShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var response = await _authService.RegisterAsync(dto);
            if (response == null)
                return BadRequest("Email already in use");

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null)
                return Unauthorized("Invalid credentials");

            return Ok(response);
        }
    }
}
