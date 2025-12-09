
namespace backend_disc.Services
{
    public interface IWeatherService
    {
        Task<CurrentWeatherData?> GetWeatherAsync(double latitude, double longitude);
    }
}