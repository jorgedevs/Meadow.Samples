using System;
using System.Collections.Generic;

namespace BusStopClient.Models
{
    public class WeatherConstants
    {
        private static readonly Lazy<WeatherConstants> instance =
            new Lazy<WeatherConstants>(() => new WeatherConstants());
        public static WeatherConstants Instance => instance.Value;

        public Dictionary<int, string> Weather { get; protected set; }

        static WeatherConstants() { }

        public WeatherConstants() 
        {
            Console.WriteLine("Loading dictionary...");

            Weather = new Dictionary<int, string>();

            Weather.Add(200, "Thunderstorm Light Rain");
            Weather.Add(201, "Thunderstorm Rain");
            Weather.Add(202, "Thunderstorm Heavy Rain");
            Weather.Add(210, "Thunderstorm Light");
            Weather.Add(211, "Thunderstorm");
            Weather.Add(212, "Thunderstorm Heavy");
            Weather.Add(221, "Thunderstorm Ragged");
            Weather.Add(230, "Thunderstorm Light Drizzle");
            Weather.Add(231, "Thunderstorm Drizzle");
            Weather.Add(232, "Thunderstorm Heavy Drizzle");

            Weather.Add(300, "Drizzle Light");
            Weather.Add(301, "Drizzle");
            Weather.Add(302, "Drizzle Heavy");
            Weather.Add(310, "Drizzle Light Rain");
            Weather.Add(311, "Drizzle Rain");
            Weather.Add(312, "Drizzle Heavy Rain");
            Weather.Add(313, "Drizzle Shower Rain");
            Weather.Add(314, "Drizzle Shower Heavy");
            Weather.Add(321, "Drizzle Shower");

            Weather.Add(500, "Rain Lght");
            Weather.Add(501, "Rain Moderate");
            Weather.Add(502, "Rain Heavy");
            Weather.Add(503, "Rain Very Heavy");
            Weather.Add(504, "Rain Extreme");
            Weather.Add(511, "Rain Freezing");
            Weather.Add(520, "Rain Shower Light");
            Weather.Add(521, "Rain Shower");
            Weather.Add(522, "Rain Shower Heavy");
            Weather.Add(531, "Rain Shower Ragged");

            Weather.Add(600, "Snow Light");
            Weather.Add(601, "Snow");
            Weather.Add(602, "Snow Heavy");
            Weather.Add(611, "Sleet");
            Weather.Add(612, "Snow Shower Sleet Light");
            Weather.Add(613, "Snow Shower Sleet");
            Weather.Add(615, "Snow Rain Light");
            Weather.Add(621, "Snow Rain");
            Weather.Add(622, "Snow Shower Light");
            Weather.Add(631, "Snow Shower");
            Weather.Add(632, "Snow Shower Heavy");

            Weather.Add(701, "Mist");
            Weather.Add(711, "Smoke");
            Weather.Add(721, "Haze");
            Weather.Add(731, "Dust Sand");
            Weather.Add(741, "Fog");
            Weather.Add(751, "Sand");
            Weather.Add(761, "Dust");
            Weather.Add(762, "Ash");
            Weather.Add(771, "Aquall");
            Weather.Add(781, "Tornado");

            Weather.Add(800, "Clouds Clear");
            Weather.Add(801, "Clouds Few");
            Weather.Add(802, "Clouds Scattered");
            Weather.Add(803, "Clouds Broken");
            Weather.Add(804, "Clouds Overcast");

            Console.WriteLine("Done");
        }
    }
}