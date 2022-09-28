using BusStopClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusStopClient.Services
{
    public sealed class BusService
    {
        private static readonly Lazy<BusService> instance =
            new Lazy<BusService>(() => new BusService());
        public static BusService Instance => instance.Value;

        private const string RestServiceBaseAddress = "https://api.translink.ca/RTTIAPI/V1/stops/";
        private const string AcceptHeaderApplicationJson = "application/json";
        private const string API_KEY = "[API KEY]";

        static BusService() { }

        public async Task<Stop> GetStopInfoAsync(string busNumber)
        {
            var stop = new Stop();

            using (HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(RestServiceBaseAddress),
                Timeout = new TimeSpan(0, 5, 0)
            })
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(AcceptHeaderApplicationJson));

                    var response = await client.GetAsync($"{busNumber}?apiKey={API_KEY}", HttpCompletionOption.ResponseContentRead);
                    response.EnsureSuccessStatusCode();

                    if (!response.IsSuccessStatusCode)
                        return stop;

                    string json = await response.Content.ReadAsStringAsync();
                    var stopInfo = JsonSerializer.Deserialize<Stop>(json);

                    stop.Name = stopInfo.Name;
                    stop.StopNo = stopInfo.StopNo;
                    stop.Routes = stopInfo.Routes;
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");
                    return stop;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                    return stop;
                }
            }

            return stop;
        }

        public async Task<List<Schedule>> GetSchedulesAsync(string busNumber)
        {
            var schedules = new List<Schedule>();

            using (HttpClient client = new HttpClient() 
            { 
                BaseAddress = new Uri(RestServiceBaseAddress),
                Timeout = new TimeSpan(0, 5, 0)
            })
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(AcceptHeaderApplicationJson));

                    var response = await client.GetAsync($"{busNumber}/estimates?apiKey={API_KEY}", HttpCompletionOption.ResponseContentRead);
                    response.EnsureSuccessStatusCode();

                    if (!response.IsSuccessStatusCode)
                        return schedules;

                    string json = await response.Content.ReadAsStringAsync();
                    var nextBuses = JsonSerializer.Deserialize<List<NextBus>>(json);

                    foreach (var nextBus in nextBuses)
                    {
                        foreach (var schedule in nextBus.Schedules)
                        {
                            var scheduleItem = new Schedule();
                            scheduleItem.RouteNo = nextBus.RouteNo;
                            scheduleItem.Destination = schedule.Destination;
                            scheduleItem.ExpectedCountdown = schedule.ExpectedCountdown;
                            scheduleItem.ScheduleStatus = schedule.ScheduleStatus;
                            schedules.Add(scheduleItem);
                        }
                    }
                    schedules.Sort((b0, b1) => b0.ExpectedCountdown.CompareTo(b1.ExpectedCountdown));

                    return schedules;
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");
                    return schedules;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                    return schedules;
                }
            }
        }
    }
}