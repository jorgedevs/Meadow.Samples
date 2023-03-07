using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloLed
{
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            CycleColors(1000);

            return base.Run();
        }

        void CycleColors(int duration)
        {
            Console.WriteLine("Cycle colors...");

            while (true)
            {
                ShowColor(Color.Blue, duration);
                ShowColor(Color.Cyan, duration);
                ShowColor(Color.Green, duration);
                ShowColor(Color.GreenYellow, duration);
                ShowColor(Color.Yellow, duration);
                ShowColor(Color.Orange, duration);
                ShowColor(Color.OrangeRed, duration);
                ShowColor(Color.Red, duration);
                ShowColor(Color.MediumVioletRed, duration);
                ShowColor(Color.Purple, duration);
                ShowColor(Color.Magenta, duration);
                ShowColor(Color.Pink, duration);
            }
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            onboardLed.StartPulse(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }

        void ShowColor(Color color, int duration = 1000)
        {
            Console.WriteLine($"Color: {color}");
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }
}
