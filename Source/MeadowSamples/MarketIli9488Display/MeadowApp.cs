using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MarketIli9488Display
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
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            var config = new SpiClockConfiguration(
                new Frequency(12000, Frequency.UnitType.Kilohertz),
                SpiClockConfiguration.Mode.Mode0);
            var spiBus = Device.CreateSpiBus(
                Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            var display = new Ili9488
            (
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D00,
                resetPin: Device.Pins.D01,
                width: 480,
                height: 320
            );

            graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true,
                CurrentFont = new Font8x8(),
                Rotation = RotationType._270Degrees
            };

            graphics.Clear();
            DisplayJPG(0, 0, "img_devcamp.jpg");
            graphics.Show();

            onboardLed.SetColor(Color.Green);
            return base.Initialize();
        }

        void DisplayJPG(int x, int y, string filename)
        {
            var jpgData = LoadResource(filename);
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            int x_offset = 0;
            int y_offset = 0;
            byte r, g, b;

            for (int i = 0; i < jpg.Length; i += 3)
            {
                r = jpg[i];
                g = jpg[i + 1];
                b = jpg[i + 2];

                graphics.DrawPixel(x + x_offset, y + y_offset, Color.FromRgb(r, g, b));

                x_offset++;
                if (x_offset % decoder.Width == 0)
                {
                    y_offset++;
                    x_offset = 0;
                }
            }

            graphics.Show();
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MarketIli9488Display.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }


        public override async Task Run()
        {
            Console.WriteLine("Run...");

            while (true)
            {
                DisplayJPG(0, 0, "img_devcamp.jpg");
                graphics.Show();

                await Task.Delay(TimeSpan.FromSeconds(5));

                DisplayJPG(0, 0, "img_devcamp_qr.jpg");
                graphics.Show();

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}