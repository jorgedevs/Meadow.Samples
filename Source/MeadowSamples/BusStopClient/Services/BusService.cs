using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BusStopClient.Models;
using System.Text.Json;

namespace BusStopClient.Services
{
    public sealed class BusService
    {
        private const string RestServiceBaseAddress = "https://api.translink.ca/RTTIAPI/V1/stops/";
        private const string AcceptHeaderApplicationJson = "application/json";
        private const string API_KEY = "[API KEY]";
        private HttpClient client;

        static BusService _instance = null;
        public static BusService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BusService();
                }
                return _instance;
            }
        }

        private BusService()
        {
            client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(AcceptHeaderApplicationJson));
        }

        public async Task<List<Schedule>> SimulateCall() 
        { 
            var arrivals = new List<Schedule>();

            await Task.Delay(1000);

            arrivals.Add(new Schedule() { Destination = "STANLEY PARK", ExpectedCountdown = 5 });
            arrivals.Add(new Schedule() { Destination = "STANLEY PARK", ExpectedCountdown = 15 });
            arrivals.Add(new Schedule() { Destination = "STANLEY PARK", ExpectedCountdown = 25 });
            arrivals.Add(new Schedule() { Destination = "STANLEY PARK", ExpectedCountdown = 35 });
            arrivals.Add(new Schedule() { Destination = "STANLEY PARK", ExpectedCountdown = 45 });

            return arrivals;
        }

        public async Task<List<Schedule>> GetSchedulesAsync(string busNumber)
        {
            var schedules = new List<Schedule>();
            string jsonResponse = string.Empty;

            try
            {
                HttpResponseMessage response = await client.GetAsync($"{RestServiceBaseAddress}{busNumber}/estimates?apiKey={API_KEY}");

                response.EnsureSuccessStatusCode();
                jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("::RESTCLIENT::GET_SCHEDULES_INFO = " + jsonResponse);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("CAUGHT EXCEPTION: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("CAUGHT EXCEPTION: " + e.Message);
            }

            if (string.IsNullOrEmpty(jsonResponse))
                return schedules;

            //try
            //{
            //    var nextBuses = JsonSerializer.Deserialize<List<NextBus>>(jsonResponse);

            //    foreach (var nextBus in nextBuses)
            //    {
            //        foreach (var schedule in nextBus.Schedules)
            //        {
            //            Schedule scheduleItem = new Schedule();
            //            //scheduleItem.RouteNo = nextBus.RouteNo;
            //            scheduleItem.Destination = schedule.Destination;
            //            scheduleItem.ExpectedCountdown = schedule.ExpectedCountdown;
            //            scheduleItem.ScheduleStatus = schedule.ScheduleStatus;
            //            schedules.Add(scheduleItem);
            //        }
            //    }
            //    schedules.Sort((b0, b1) => b0.ExpectedCountdown.CompareTo(b1.ExpectedCountdown));
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("CAUGHT EXCEPTION: " + ex);
            //}

            return schedules;
        }
    }
}