using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Moisture;

namespace Sensors.Moisture.Capacitive_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Capacitive capacitive;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            capacitive = new Capacitive(
                Device.CreateAnalogInputPort(Device.Pins.A01), 2.84f, 1.37f);

            TestCapacitiveSensor();
        }

        async Task TestCapacitiveSensor()
        {
            Console.WriteLine("TestCapacitiveSensor...");

            // Use Read(); to get soil moisture value from 0 - 100
            while (true)
            {
                float moisture = await capacitive.Read();

                if (moisture > 1f)
                    moisture = 1f;
                else
                if (moisture < 0)
                    moisture = 0;

                Console.WriteLine($"Moisture {moisture * 100}%");
                Thread.Sleep(1000);
            }
        }
    }
}
