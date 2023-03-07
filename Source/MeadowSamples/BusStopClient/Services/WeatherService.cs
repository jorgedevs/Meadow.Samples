using BusStopClient.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusStopClient.Services
{
    public class WeatherService
    {
        private static readonly Lazy<WeatherService> instance =
            new Lazy<WeatherService>(() => new WeatherService());
        public static WeatherService Instance => instance.Value;

        static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";
        static string city = $"Vancouver";

        static WeatherService() { }

        public async Task<WeatherReading> GetWeatherForecast()
        {
            var weatherReading = new WeatherReading();

            using (HttpClient client = new HttpClient() 
            {
                Timeout = new TimeSpan(0, 5, 0)
            })
            {
                try
                {
                    var response = await client.GetAsync($"{climateDataUri}?q={city}&appid={Secrets.WEATHER_API_KEY}");
                    response.EnsureSuccessStatusCode();

                    if (!response.IsSuccessStatusCode)
                        return weatherReading;

                    string json = await response.Content.ReadAsStringAsync();
                    var values = JsonSerializer.Deserialize<WeatherReading>(json);

                    return values;
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                    return null;
                }
            }
        }
    }
}