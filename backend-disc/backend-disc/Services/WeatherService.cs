using System.Text.Json;
using System.Text.Json.Serialization;
namespace backend_disc.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrentWeatherData?> GetWeatherAsync(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,weathercode&forecast_days=1&timezone=auto";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var weatherData = JsonSerializer.Deserialize<WeatherData>(jsonResponse);
            int currentHour = DateTime.Now.Hour;
            if (weatherData == null || weatherData.Hourly == null
                || weatherData.Hourly.temperature_2m == null || weatherData.Hourly.weathercode == null
                || currentHour >= weatherData.Hourly.temperature_2m.Length
                )
            {
                return null;
            }
            var currentTemp = weatherData.Hourly.temperature_2m[currentHour];
            var currentWeatherCode = weatherData.Hourly.weathercode[currentHour];
            return new CurrentWeatherData
            {
                Temperature = currentTemp,
                WeatherCode = currentWeatherCode
            };
        }
    }

    public class WeatherData
    {
        [JsonPropertyName("hourly")]
        public HourlyData? Hourly { get; set; } 
    }

    public class HourlyData
    {
        [JsonPropertyName("time")]
        public string[]? time { get; set; }
        [JsonPropertyName("temperature_2m")]
        public double[]? temperature_2m { get; set; }
        [JsonPropertyName("weathercode")]
        public int[]? weathercode { get; set; }
    }

    public class CurrentWeatherData
    {
        public double Temperature { get; set; }
        public int WeatherCode { get; set; }
    }
}