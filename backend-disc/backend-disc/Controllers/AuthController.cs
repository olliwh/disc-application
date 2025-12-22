using backend_disc.Dtos.Auth;
using backend_disc.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, [FromQuery] string db = "mssql")
        {
            var result = await _authService.Login(dto, db);
            if(result == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(result);
        }
    }
}
