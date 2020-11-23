using System;
using System.Net.Http;
using System.Threading.Tasks;
using WifiWeather.Models;

namespace WifiWeather.ServiceAccessLayer
{
    public static class ClientServiceFacade
    {
        // TODO: change this IP for your localhost 
        static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather?q=Vancouver&appid=65ffb400cbae35050dc2d1d1ebd3ef8d";

        static ClientServiceFacade() { }

        /// <summary>
        /// Fetches the climate readings from the Web API Endpoint
        /// </summary>
        /// <returns></returns>
        public static async Task FetchReadings()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync(climateDataUri);

                try
                {
                    Console.WriteLine("sending request...");

                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(json);

                    var stuff = System.Text.Json.JsonSerializer.Deserialize(json, typeof(WeatherReading));

                    Console.WriteLine("deserialized to object");

                    var reading = stuff as WeatherReading;

                    Console.WriteLine($"Success");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request time out.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }
    }
}