using backend_disc.Dtos.Auth;

namespace backend_disc.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> Login(LoginDto dto);
    }
}