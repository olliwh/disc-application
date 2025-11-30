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
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IConfiguration config,
            IGenericRepository<User> userRepository,
            ILogger<AuthService> logger)
        {
            _config = config;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> Login(LoginDto dto)
        {
            try
            {
                var user = await _userRepository.Query()
                    .Include(u => u.Employee)
                    .Include(u => u.UserRole)
                    .FirstOrDefaultAsync(u => u.Username == dto.Username);

                if (user == null)
                {
                    _logger.LogWarning("Login attempt failed: User not found - {Username}", dto.Username);
                    return null;
                }

                bool isPasswordValid;
                try
                {
                    isPasswordValid = Argon2.Verify(user.PasswordHash, dto.Password);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Password verification failed for user {Username}", dto.Username);
                    return null; 
                }

                if (!isPasswordValid)
                {
                    _logger.LogWarning("Login attempt failed: Invalid password - {Username} pa - {Password}", dto.Username, dto.Password);
                    return null;
                }

                // Generate JWT
                var secretKey = _config["API_SECRET_KEY"]
                    ?? throw new InvalidOperationException("API_SECRET_KEY is not configured");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, user.Username),
                    new Claim("employeeId", user.EmployeeId.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole?.Name ?? "User")
                    ]),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new LoginResponseDto
                {
                    Token = tokenHandler.WriteToken(token),
                    ExpiresAt = tokenDescriptor.Expires!.Value
                };
            }
            catch (InvalidOperationException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user {Username}", dto.Username);
                return null;
            }
        }
    }
}
