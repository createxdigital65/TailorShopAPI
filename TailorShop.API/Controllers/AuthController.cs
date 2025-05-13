using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorShop.API.DTOs;
using TailorShop.API.Services;

namespace TailorShop.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterAsync(dto);
            if (response == null)
            {
                _logger.LogWarning("Registration failed for email {Email}", dto.Email);
                return BadRequest("Email already in use.");
            }

            _logger.LogInformation("User registered: {Email}", dto.Email);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.LoginAsync(dto);
            if (response == null)
            {
                _logger.LogWarning("Login failed for email {Email}", dto.Email);
                return Unauthorized("Invalid credentials.");
            }

            _logger.LogInformation("User logged in: {Email}", dto.Email);
            return Ok(response);
        }
    }
}
