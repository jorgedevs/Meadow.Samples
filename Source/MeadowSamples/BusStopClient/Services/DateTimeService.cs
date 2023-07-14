using BusStopClient.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusStopClient.Services
{
    public sealed class DateTimeService
    {
        private static readonly Lazy<DateTimeService> instance =
            new Lazy<DateTimeService>(() => new DateTimeService());
        public static DateTimeService Instance => instance.Value;

        static string clockDataUri = $"http://worldtimeapi.org/api/timezone/America/{Secrets.DATE_TIME_CITY}/";

        static DateTimeService() { }

        public async Task GetDateTime()
        {
            using (HttpClient client = new HttpClient() 
            {
                Timeout = new TimeSpan(0, 5, 0)
            })
            {
                try
                {
                    var response = await client.GetAsync($"{clockDataUri}");
                    response.EnsureSuccessStatusCode();

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    string json = await response.Content.ReadAsStringAsync();
                    var values = JsonSerializer.Deserialize<DateTimeEntity>(json);

                    stopwatch.Stop();

                    var dateTime = values.datetime.Add(stopwatch.Elapsed);

                    MeadowApp.Device.PlatformOS.SetClock(new DateTime(
                        year: dateTime.Year,
                        month: dateTime.Month,
                        day: dateTime.Day,
                        hour: dateTime.Hour,
                        minute: dateTime.Minute,
                        second: dateTime.Second));
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }
    }
}