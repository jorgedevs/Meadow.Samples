using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketEpaperDisplay
{
    public class MeadowApp : App<F7FeatherV2>
    {
        MicroGraphics graphics;
        RgbPwmLed onboardLed;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            var display = new Ssd1680(
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: Device.Pins.A05,
                dcPin: Device.Pins.A04,
                resetPin: Device.Pins.A02,
                busyPin: Device.Pins.A01,
                width: 122,
                height: 250);

            graphics = new MicroGraphics(display)
            {
                Rotation = RotationType._270Degrees,
                CurrentFont = new Font12x16()
            };

            graphics.Clear();
            graphics.DrawText(10, 10, "Hello World");
            graphics.Show();

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        public override Task Run()
        {
            Console.WriteLine("Run...");

            return base.Run();
        }
    }
}