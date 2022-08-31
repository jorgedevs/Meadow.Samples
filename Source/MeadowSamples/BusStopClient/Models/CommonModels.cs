using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace BusStopClient.Models
{
    public class Stop
    {
        public string StopNo { get; set; }
        public string Name { get; set; }
        public string BayNo { get; set; }
        public string City { get; set; }
        public string OnStreet { get; set; }
        public string AtStreet { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WheelchairAccess { get; set; }
        public string Distance { get; set; }
        public string Routes { get; set; }
    }

    [JsonObject("Schedule")]
    public class Schedule
    {
        [JsonProperty("Pattern")]
        public string Pattern { get; set; }

        [JsonProperty("Destination")]
        public string Destination { get; set; }

        [JsonProperty("ExpectedLeaveTime")]
        public string ExpectedLeaveTime { get; set; }

        [JsonProperty("ExpectedCountdown")]
        public int ExpectedCountdown { get; set; }

        [JsonProperty("ScheduleStatus")]
        public string ScheduleStatus { get; set; }

        [JsonProperty("CancelledTrip")]
        public bool CancelledTrip { get; set; }

        [JsonProperty("CancelledStop")]
        public bool CancelledStop { get; set; }

        [JsonProperty("AddedTrip")]
        public bool AddedTrip { get; set; }

        [JsonProperty("AddedStop")]
        public bool AddedStop { get; set; }

        [JsonProperty("LastUpdate")]
        public string LastUpdate { get; set; }
    }

    [JsonObject("NextBus")]
    public class NextBus
    {
        [JsonProperty("RouteNo")]
        public string RouteNo { get; set; }

        [JsonProperty("RouteName")]
        public string RouteName { get; set; }

        [JsonProperty("Direction")]
        public string Direction { get; set; }

        [JsonProperty("Schedules")]
        public ObservableCollection<Schedule> Schedules { get; set; }
    }

    public class Root 
    {
        public ObservableCollection<NextBus> NextBuses { get; set; }
    }
}