using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace TwitterClient
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV1>
    {
        MicroGraphics graphics;
        string[] images = new string[4] 
        { 
            "tw-logo.jpg", 
            "tw-like.jpg", 
            "tw-reply.jpg", 
            "tw-retweet.jpg" 
        };

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
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
                resetPin: Device.Pins.D01
            );

            graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true,
                CurrentFont = new Font8x8()
            };

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        public override Task Run()
        {
            Console.WriteLine("Run");

            graphics.DrawRectangle(0, 0, 320, 480, Color.FromHex("#15202B"), true);
            graphics.DrawRectangle(16, 16, 288, 448, Color.FromHex("#38444D"));
            graphics.DrawRectangle(32, 270, 256, 153, Color.FromHex("#38444D"));

            DisplayJPG(32, 32, images[0]);

            graphics.CurrentFont = new Font8x12();
            graphics.DrawText(110, 50, "Wilderness Labs Inc.", Color.White);
            graphics.DrawText(110, 74, "@wildernesslabs", Color.White);

            graphics.DrawText(32, 119, "Creators of Meadow", Color.White);
            graphics.DrawText(32, 143, "Secure IoT for every developer", Color.White);

            graphics.DrawText(32, 194, "230 Following", Color.White);
            graphics.DrawText(180, 194, "1466 Followers", Color.White);

            graphics.DrawText(32, 240, "Latest tweet:", Color.White);

            DisplayJPG(58, 434, images[1]);
            DisplayJPG(138, 434, images[2]);
            DisplayJPG(227, 434, images[3]);

            graphics.Show();

            return base.Run();
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
            var resourceName = $"TwitterClient.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}