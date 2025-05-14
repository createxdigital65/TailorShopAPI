using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TailorShop.API.Data;
using TailorShop.API.DTOs;
using TailorShop.API.Models;
using BCrypt.Net;

namespace TailorShop.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly TailorDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(TailorDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto?> RegisterAsync(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return null;

            var user = new User
            {
                BusinessName = dto.BusinessName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreateAuthResponse(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return CreateAuthResponse(user);
        }

        private AuthResponseDto CreateAuthResponse(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiresInMinutes = jwtSettings["ExpiresInMinutes"];

            //  Debug print to check if values are loading properly
            Console.WriteLine($"Creating token with: issuer={issuer}, audience={audience}, expiresIn={expiresInMinutes}");

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(expiresInMinutes))
            {
                throw new InvalidOperationException("JWT settings are missing or incomplete.");
            }

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim("userId", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(expiresInMinutes)),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Email = user.Email,
                BusinessName = user.BusinessName,
                Token = tokenHandler.WriteToken(token)
            };
        }

    }
}
