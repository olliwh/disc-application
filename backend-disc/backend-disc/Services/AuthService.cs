using backend_disc.Dtos.Auth;
using backend_disc.Factories;
using backend_disc.Repositories;
using class_library_disc.Models.Sql;
using Isopoh.Cryptography.Argon2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_disc.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly IGenericRepositoryFactory _factory;

        public AuthService(
            IConfiguration config,
            IGenericRepositoryFactory factory,
            ILogger<AuthService> logger)
        {
            _config = config;
            _logger = logger;
            _factory = factory;
        }

        public async Task<LoginResponseDto?> Login(LoginDto dto, string db)
        {
            var repo = _factory.GetRepository<User>(db);
            try
            {
                var _userRepository = repo as IUserRepository;
                if (_userRepository == null)
                {
                    return null;
                }
                var user = await _userRepository.GetUserByUsername(dto.Username);

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
