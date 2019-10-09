using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;

namespace Sensors.Moisture.Capacitive_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Capacitive capacitive;

        public MeadowApp()
        {
            capacitive = new Capacitive(Device.CreateAnalogInputPort(Device.Pins.A00));

            test();
        }

        async Task test() 
        {
            while (true)
            {
                Console.WriteLine($"======================== raw value: {capacitive.AnalogPort.Read().Result}");
                Thread.Sleep(1000);
            }
        }
    }
}
