using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace RotationDetector
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led up;
        Led down; 
        Led left;
        Led right;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            up = new Led(Device.CreateDigitalOutputPort(Device.Pins.D15));
            down = new Led(Device.CreateDigitalOutputPort(Device.Pins.D12));
            left = new Led(Device.CreateDigitalOutputPort(Device.Pins.D14));
            right = new Led(Device.CreateDigitalOutputPort(Device.Pins.D13));
           
            BlinkLeds();
        }

        public void BlinkLeds()
        {
            var state = false;

            while (true)
            {
                int wait = 200;

                state = !state;

                Console.WriteLine($"State: {state}");

                up.IsOn = state;
                Thread.Sleep(wait);
                down.IsOn = state;
                Thread.Sleep(wait);
                left.IsOn = state;
                Thread.Sleep(wait);
                right.IsOn = state;
                Thread.Sleep(wait);
            }
        }
    }
}