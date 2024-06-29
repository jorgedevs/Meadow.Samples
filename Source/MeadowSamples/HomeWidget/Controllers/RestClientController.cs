using HomeWidget.DTOs;
using HomeWidget.Utils;
using Meadow;
using Meadow.Foundation.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeWidget.Controllers
{
    internal class RestClientController
    {
        string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";

        public async Task<(string, double, double)?> GetWeatherForecast()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = new TimeSpan(0, 5, 0);

                    HttpResponseMessage response = await client.GetAsync($"{climateDataUri}?q={Secrets.WEATHER_CITY}&appid={Secrets.WEATHER_API_KEY}");

                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var values = MicroJson.Deserialize<WeatherReadingDTO>(json);

                    double outdoorTemperature = values.main.temp - 273;
                    double outdoorHumidity = values.main.humidity;
                    var today = DateTime.Now.AddHours(-8);
                    var sunrise = DateTimeOffset.FromUnixTimeSeconds(values.sys.sunrise).DateTime.AddHours(-8);
                    var sunset = DateTimeOffset.FromUnixTimeSeconds(values.sys.sunset).DateTime.AddHours(-8);
                    bool isDayLight = today > sunrise && today < sunset;
                    string weatherIconFile = GetWeatherIcon(values.weather[0].id, isDayLight);

                    return (weatherIconFile, outdoorTemperature, outdoorHumidity);
                }
                catch (TaskCanceledException)
                {
                    Resolver.Log.Info("Request timed out.");
                    return null;
                }
                catch (Exception e)
                {
                    Resolver.Log.Info($"Request went sideways: {e.Message}");
                    return null;
                }
            }
        }

        string GetWeatherIcon(int weatherCode, bool isDayLight)
        {
            Resolver.Log.Info($"WeatherIcon: {weatherCode}");

            string resourceName;

            switch (weatherCode)
            {
                case WeatherCodeConstants.THUNDERSTORM_LIGHT_RAIN:
                    resourceName = isDayLight
                        ? $"HomeWidget.Resources.w_storm_light_sun.bmp"
                        : $"HomeWidget.Resources.w_storm_light_moon.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.THUNDERSTORM_RAIN &&
                    n <= WeatherCodeConstants.THUNDERSTORM):
                    resourceName = $"HomeWidget.Resources.w_storm.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.THUNDERSTORM_HEAVY &&
                    n <= WeatherCodeConstants.THUNDERSTORM_HEAVY_DRIZZLE):
                    resourceName = $"HomeWidget.Resources.w_storm_heavy.bmp";
                    break;

                case WeatherCodeConstants.DRIZZLE_LIGHT:
                    resourceName = isDayLight
                        ? $"HomeWidget.Resources.w_drizzle_light_sun.bmp"
                        : $"HomeWidget.Resources.w_drizzle_light_moon.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.DRIZZLE &&
                    n <= WeatherCodeConstants.DRIZZLE_RAIN):
                    resourceName = $"HomeWidget.Resources.w_drizzle.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.DRIZZLE_HEAVY_RAIN &&
                    n <= WeatherCodeConstants.DRIZZLE_SHOWER):
                    resourceName = $"HomeWidget.Resources.w_drizzle_heavy.bmp";
                    break;

                case WeatherCodeConstants.RAIN_LIGHT:
                    resourceName = isDayLight
                        ? $"HomeWidget.Resources.w_rain_light_sun.bmp"
                        : $"HomeWidget.Resources.w_rain_light_moon.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.RAIN_MODERATE &&
                    n <= WeatherCodeConstants.RAIN_FREEZING):
                    resourceName = $"HomeWidget.Resources.w_rain.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.RAIN_SHOWER_LIGHT &&
                    n <= WeatherCodeConstants.RAIN_SHOWER_RAGGED):
                    resourceName = $"HomeWidget.Resources.w_rain_heavy.bmp";
                    break;

                case WeatherCodeConstants.SNOW_LIGHT:
                    resourceName = isDayLight
                        ? $"HomeWidget.Resources.w_snow_light_sun.bmp"
                        : $"HomeWidget.Resources.w_snow_light_moon.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.SNOW &&
                    n <= WeatherCodeConstants.SNOW_SHOWER_SLEET):
                    resourceName = $"HomeWidget.Resources.w_snow.bmp";
                    break;
                case int n when (n >= WeatherCodeConstants.SNOW_RAIN_LIGHT &&
                    n <= WeatherCodeConstants.SNOW_SHOWER_HEAVY):
                    resourceName = $"HomeWidget.Resources.w_snow_rain.bmp";
                    break;

                case WeatherCodeConstants.MIST:
                case WeatherCodeConstants.FOG:
                    resourceName = $"HomeWidget.Resources.w_mist.bmp";
                    break;

                case WeatherCodeConstants.CLOUDS_CLEAR:
                    resourceName = isDayLight
                        ? $"HomeWidget.Resources.w_clear_sun.bmp"
                        : $"HomeWidget.Resources.w_clear_moon.bmp";
                    break;
                case WeatherCodeConstants.CLOUDS_FEW:
                case WeatherCodeConstants.CLOUDS_SCATTERED:
                    resourceName = isDayLight
                        ? $"HomeWidget.Resources.w_clouds_sun.bmp"
                        : $"HomeWidget.Resources.w_clouds_moon.bmp";
                    break;
                case WeatherCodeConstants.CLOUDS_BROKEN:
                case WeatherCodeConstants.CLOUDS_OVERCAST:
                    resourceName = $"HomeWidget.Resources.w_clouds.bmp";
                    break;
                default:
                    resourceName = $"HomeWidget.Resources.w_misc.bmp";
                    break;
            }

            return resourceName;
        }
    }
}