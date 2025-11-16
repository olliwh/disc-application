using backend_disc.Dtos.Auth;
using backend_disc.Dtos.Employees;
using backend_disc.Repositories;
using class_library_disc.Models.Sql;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_disc.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<User> _userRepository;

        public AuthService(IConfiguration config, IGenericRepository<User> userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }
        public async Task<LoginResponseDto?> Login(LoginDto dto)
        {
            var user = await _userRepository.Query()
                .Include(u => u.Employee)
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !Argon2.Verify(user.PasswordHash, dto.Password))
                return null;

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _config["API_SECRET_KEY"] ?? throw new InvalidOperationException("API_SECRET_KEY is not configured");

            var key = Encoding.UTF8.GetBytes(secretKey); 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                new Claim(ClaimTypes.Name, user.Username), 
                new Claim("employeeId", user.EmployeeId.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole?.Name ?? "User")            ]),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresAt = tokenDescriptor.Expires!.Value
            };
        }

    }
}
