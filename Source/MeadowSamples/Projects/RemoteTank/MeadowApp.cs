using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;

namespace RemoteTank
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led ledRed;
        Led ledGreen;
        Led ledBlue;

        TankController tankController;

        public MeadowApp()
        {
            ConfigurePorts();
            BlinkLeds();
        }

        public void ConfigurePorts()
        {
            Console.WriteLine("Creating Outputs...");
            ledRed = new Led(Device.CreateDigitalOutputPort(Device.Pins.D00));
            ledGreen = new Led(Device.CreateDigitalOutputPort(Device.Pins.D01));
            ledBlue = new Led(Device.CreateDigitalOutputPort(Device.Pins.D02));

            tankController = new TankController
            (
                motorLeftPort: Device.CreatePwmPort(Device.Pins.D03, 1000, 0.75f), 
                motorRightPort: Device.CreatePwmPort(Device.Pins.D04, 1000, 0.75f)
            );
        }

        public void BlinkLeds()
        {
            var state = false;

            while (true)
            {
                int wait = 200;

                state = !state;

                Console.WriteLine($"State: {state}");

                ledRed.IsOn = state;
                Thread.Sleep(wait);
                ledGreen.IsOn = state;
                Thread.Sleep(wait);
                ledBlue.IsOn = state;
                Thread.Sleep(wait);

                Console.WriteLine($"Forward");
                tankController.TurnRight();
                Thread.Sleep(3000);

                Console.WriteLine($"Stop");
                tankController.Stop();
                Thread.Sleep(3000);

                Console.WriteLine($"Backward");
                tankController.TurnLeft();
                Thread.Sleep(3000);
                tankController.Stop();
            }
        }
    }
}
