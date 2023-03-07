using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace RemoteTank
{
    public class MeadowApp : App<F7FeatherV1>
    {
        Led ledRed;
        Led ledGreen;
        Led ledBlue;

        TankController tankController;

        public override Task Initialize()
        {
            Console.WriteLine("Creating Outputs...");
            ledRed = new Led(Device.Pins.D00);
            ledGreen = new Led(Device.Pins.D01);
            ledBlue = new Led(Device.Pins.D02);

            tankController = new TankController
            (
                motorLeftPort: Device.CreatePwmPort(Device.Pins.D03, new Frequency(1000), 0.75f),
                motorRightPort: Device.CreatePwmPort(Device.Pins.D04, new Frequency(1000), 0.75f)
            );

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var state = false;

            while (true)
            {
                int wait = 200;

                state = !state;

                Console.WriteLine($"State: {state}");

                ledRed.IsOn = state;
                await Task.Delay(wait);
                ledGreen.IsOn = state;
                await Task.Delay(wait);
                ledBlue.IsOn = state;
                await Task.Delay(wait);

                Console.WriteLine($"Forward");
                tankController.TurnRight();
                await Task.Delay(3000);

                Console.WriteLine($"Stop");
                tankController.Stop();
                await Task.Delay(3000);

                Console.WriteLine($"Backward");
                tankController.TurnLeft();
                await Task.Delay(3000);
                tankController.Stop();
            }
        }
    }
}