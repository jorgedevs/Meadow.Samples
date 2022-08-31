﻿using BusStopClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public static async Task<List<Schedule>> GetSchedulesAsync(string busNumber)
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

                    Console.WriteLine("Calling API...");

                    var response = await client.GetAsync($"{busNumber}/estimates?apiKey={API_KEY}", HttpCompletionOption.ResponseContentRead);
                    response.EnsureSuccessStatusCode();

                    if (!response.IsSuccessStatusCode)
                        return schedules;

                    string json = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"Json Response = {json}");

                    var nextBuses = JsonConvert.DeserializeObject<List<NextBus>>(json);

                    Console.WriteLine($"Next Busses {nextBuses.Count}");

                    foreach (var nextBus in nextBuses)
                    {
                        foreach (var schedule in nextBus.Schedules)
                        {
                            var scheduleItem = new Schedule();
                            //scheduleItem.RouteNo = nextBus.RouteNo;
                            scheduleItem.Destination = schedule.Destination;
                            scheduleItem.ExpectedCountdown = schedule.ExpectedCountdown;
                            scheduleItem.ScheduleStatus = schedule.ScheduleStatus;
                            schedules.Add(scheduleItem);
                        }
                    }
                    schedules.Sort((b0, b1) => b0.ExpectedCountdown.CompareTo(b1.ExpectedCountdown));
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

            GC.Collect();

            return schedules;
        }
    }
}