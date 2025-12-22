using backend_disc.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;
        public WeatherController(IWeatherService service)
        {
            _service = service;
        }
        [HttpGet()]
        public async Task<IActionResult> GetWeatherByCoordinates([FromQuery] double latitude = 55.6761, [FromQuery] double longitude = 12.5683)
        {
            try
            {
                var currentWeather = await _service.GetWeatherAsync(latitude, longitude);
                if(currentWeather == null)
                {
                    return NotFound("Weather data not found for the given coordinates.");
                }
                return Ok(currentWeather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }
        }
    }
}
