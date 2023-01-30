using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ImageShow
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        MicroGraphics graphics;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            onboardLed = new RgbPwmLed(
                Device,
                Device.Pins.OnboardLedRed,
                Device.Pins.OnboardLedGreen,
                Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var config = new SpiClockConfiguration(
                new Frequency(12000, Frequency.UnitType.Kilohertz),
                SpiClockConfiguration.Mode.Mode0);
            var spiBus = Device.CreateSpiBus(
                Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            var display = new Ili9488
            (
                device: Device,
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D00,
                resetPin: Device.Pins.D01,
                width: 480, height: 320
            );

            graphics = new MicroGraphics(display)
            {
                Rotation = RotationType._270Degrees,
                IgnoreOutOfBoundsPixels = true
            };

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        public override Task Run()
        {
            Console.WriteLine("Run...");

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(10, 10, "Hello World", Color.White);

            graphics.Show();

            return base.Run();
        }
    }
}