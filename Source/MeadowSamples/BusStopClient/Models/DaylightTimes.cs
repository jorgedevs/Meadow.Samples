using System;
using System.Collections.Generic;
using System.Linq;

namespace BusStopClient.Models
{
    public static class DaylightTimes
    {
        public static List<Daylight> Dates { get; private set; }

        static DaylightTimes()
        {
            Dates = new List<Daylight>();
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 01, 01, 08, 00, 0), Sunset = new DateTime(2022, 01, 1, 16, 20, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 02, 01, 07, 43, 0), Sunset = new DateTime(2022, 02, 1, 17, 10, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 03, 01, 07, 34, 0), Sunset = new DateTime(2022, 03, 1, 19, 11, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 04, 01, 06, 49, 0), Sunset = new DateTime(2022, 04, 1, 19, 44, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 05, 01, 05, 50, 0), Sunset = new DateTime(2022, 05, 1, 20, 29, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 06, 01, 05, 11, 0), Sunset = new DateTime(2022, 06, 1, 21, 09, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 07, 01, 05, 11, 0), Sunset = new DateTime(2022, 07, 1, 21, 21, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 08, 01, 05, 44, 0), Sunset = new DateTime(2022, 08, 1, 20, 52, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 09, 01, 06, 28, 0), Sunset = new DateTime(2022, 09, 1, 19, 54, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 10, 01, 07, 12, 0), Sunset = new DateTime(2022, 10, 1, 18, 50, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 11, 01, 07, 05, 0), Sunset = new DateTime(2022, 11, 1, 16, 45, 0) });
            Dates.Add(new Daylight { Sunrise = new DateTime(2022, 12, 01, 07, 46, 0), Sunset = new DateTime(2022, 12, 1, 16, 16, 0) });
        }

        public static Daylight GetDaylight(int month)
        {
            return Dates.FirstOrDefault(x => x.Sunrise.Month == month);
        }
    }
}