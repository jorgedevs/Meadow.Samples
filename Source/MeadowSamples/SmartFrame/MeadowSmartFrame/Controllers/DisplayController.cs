using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;

namespace MeadowSmartFrame.Controllers
{
    public class DisplayController
    {
        private static readonly Lazy<DisplayController> instance =
            new Lazy<DisplayController>(() => new DisplayController());
        public static DisplayController Instance => instance.Value;

        bool currentTheme;

        MicroGraphics graphics;

        static DisplayController() { }

        public void Initialize()
        {
            var config = new SpiClockConfiguration(
                new Frequency(12000, Frequency.UnitType.Kilohertz),
                SpiClockConfiguration.Mode.Mode0);
            var spiBus = MeadowApp.Device.CreateSpiBus(
                MeadowApp.Device.Pins.SCK, MeadowApp.Device.Pins.MOSI, MeadowApp.Device.Pins.MISO, config);

            var display = new Ili9488
            (
                spiBus: spiBus,
                chipSelectPin: MeadowApp.Device.Pins.D02,
                dcPin: MeadowApp.Device.Pins.D00,
                resetPin: MeadowApp.Device.Pins.D01, 480, 320
            );

            graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true,
                Rotation = RotationType._90Degrees,
                Stroke = 2
            };
        }

        public void DrawSplashScreen()
        {
            graphics.Clear(ColorConstants.SplashColor, false);
            DisplayJPG(164, 106, ImageConstants.SplashImage);
            graphics.Show();
        }

        public void Show()
        {
            graphics.Show();
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
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MeadowSmartFrame.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }
}