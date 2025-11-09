using backend_disc.Dtos.Auth;
using backend_disc.Dtos.Employees;

namespace backend_disc.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> Login(LoginDto dto);
    }
}