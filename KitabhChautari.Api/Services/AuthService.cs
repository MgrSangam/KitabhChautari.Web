using KitabhChautari.Api.Data;
using KitabhChautari.Api.Data.Entities;
using KitabhChautari.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KitabhChautari.Api.Services
{
    public class AuthService
    {
        private readonly KitabhChautariDBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(KitabhChautariDBContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == dto.Username );

            if (user == null)
            {
                return new AuthResponseDto(null, "Invalid Username");
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.PasswordHash);
            if(passwordResult == PasswordVerificationResult.Failed)
            {
                return new AuthResponseDto(null, "Incorrect Passsword");
            }

            //Generate JWT Token
            var jwt = GenerateJwtToken(user);
            return new AuthResponseDto(jwt);
        }

        private  string GenerateJwtToken(User user)
        {
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var secretKey = _configuration.GetValue<string>("Jwt:Secret");
            var symmetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            // Fixing CS0819 by separating the declaration of the JwtSecurityToken
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
