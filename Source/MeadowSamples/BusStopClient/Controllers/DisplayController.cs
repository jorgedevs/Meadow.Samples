using BusStopClient.Models;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BusStopClient.Controllers
{
    public class DisplayController
    {
        private static readonly Lazy<DisplayController> instance =
            new Lazy<DisplayController>(() => new DisplayController());
        public static DisplayController Instance => instance.Value;

        MicroGraphics graphics;
        Font12x20 large;
        Font8x12 medium;

        Color backgroundColor;
        Color fontColor;
        string backgroundImage;
        string stopSignImage;

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
                device: MeadowApp.Device,
                spiBus: spiBus,
                chipSelectPin: MeadowApp.Device.Pins.D02,
                dcPin: MeadowApp.Device.Pins.D00,
                resetPin: MeadowApp.Device.Pins.D01
            );

            graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true
            };

            large = new Font12x20();
            medium = new Font8x12();

            SetTheme();
        }

        public void SetTheme()
        {
            var today = DateTime.Now;
            var sunset = DaylightTimes.GetDaylight(today.Month).Sunset;
            var sunrise = DaylightTimes.GetDaylight(today.Month).Sunrise;

            bool isDaylight = today.TimeOfDay >= sunrise.TimeOfDay 
                && today.TimeOfDay <= sunset.TimeOfDay;

            backgroundColor = isDaylight ? ColorConstants.DayBackground : ColorConstants.NightBackground;
            fontColor = isDaylight ? ColorConstants.DarkFont : ColorConstants.LightFont;
            backgroundImage = isDaylight ? ImageConstants.DayBackground : ImageConstants.NightBackground;
            stopSignImage = isDaylight ? ImageConstants.DayStopSign : ImageConstants.NightStopSign;
        }

        public void DrawSplashScreen() 
        {
            graphics.Clear(ColorConstants.SplashColor, false);
            DisplayJPG(105, 214, ImageConstants.SplashImage);
            graphics.Show();
        }

        public void DrawBackgroundAndStopInfo()
        {
            graphics.Clear(backgroundColor, false);

            DisplayJPG(0, 370, backgroundImage);

            DisplayJPG(19, 33, stopSignImage);
        }

        public void DrawStopInfo(Stop stop)
        {
            graphics.CurrentFont = medium;

            graphics.DrawText(95, 50, stop.Name, fontColor);
            graphics.DrawText(95, 85, $"STOP #{stop.StopNo}", fontColor, ScaleFactor.X2);

            graphics.Show();
        }

        public void DrawBusArrivals(List<Schedule> arrivals)
        {
            graphics.CurrentFont = large;

            graphics.DrawRectangle(15, 160, 290, 180, backgroundColor, true);

            if (arrivals.Count == 0)
                return;

            if (arrivals.Count > 0 && arrivals[0] != null)
            {
                graphics.DrawText(15, 160, Truncate($"{arrivals[0].RouteNo} {arrivals[0].Destination}", 16), fontColor);
                graphics.DrawText(305, 160, $"{arrivals[0].ExpectedCountdown} MIN", fontColor, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 1 && arrivals[1] != null)
            {
                graphics.DrawText(15, 200, Truncate($"{arrivals[1].RouteNo} {arrivals[1].Destination}", 16), fontColor);
                graphics.DrawText(305, 200, $"{arrivals[1].ExpectedCountdown} MIN", fontColor, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 2 && arrivals[2] != null)
            {
                graphics.DrawText(15, 240, Truncate($"{arrivals[2].RouteNo} {arrivals[2].Destination}", 16), fontColor);
                graphics.DrawText(305, 240, $"{arrivals[2].ExpectedCountdown} MIN", fontColor, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 3 && arrivals[3] != null)
            {
                graphics.DrawText(15, 280, Truncate($"{arrivals[3].RouteNo} {arrivals[3].Destination}", 16), fontColor);
                graphics.DrawText(305, 280, $"{arrivals[3].ExpectedCountdown} MIN", fontColor, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 4 && arrivals[4] != null)
            {
                graphics.DrawText(15, 320, Truncate($"{arrivals[4].RouteNo} {arrivals[4].Destination}", 16), fontColor);
                graphics.DrawText(305, 320, $"{arrivals[4].ExpectedCountdown} MIN", fontColor, alignment: TextAlignment.Right);
            }

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
            var resourceName = $"BusStopClient.{filename}";

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