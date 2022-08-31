using System.Collections.ObjectModel;

namespace BusStopClient.Models
{
    public class Stop
    {
        public string City { get; set; }
        public string Name { get; set; }
        public string BayNo { get; set; }
        public string Routes { get; set; }
        public string StopNo { get; set; }
        public string OnStreet { get; set; }
        public string AtStreet { get; set; }
        public string Distance { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WheelchairAccess { get; set; }
    }

    public class Schedule
    {
        public int ExpectedCountdown { get; set; }
        public bool AddedTrip { get; set; }
        public bool AddedStop { get; set; }
        public bool CancelledTrip { get; set; }
        public bool CancelledStop { get; set; }
        public string RouteNo { get; set; }
        public string Pattern { get; set; }
        public string LastUpdate { get; set; }
        public string Destination { get; set; }
        public string ScheduleStatus { get; set; }
        public string ExpectedLeaveTime { get; set; }
    }

    public class NextBus
    {
        public string RouteNo { get; set; }
        public string RouteName { get; set; }
        public string Direction { get; set; }
        public ObservableCollection<Schedule> Schedules { get; set; }
    }
}