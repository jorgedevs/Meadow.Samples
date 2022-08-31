using BusStopClient.Models;
using BusStopClient.Services;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BusStopClient
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV1>
    {
        MicroGraphics graphics;
        Font12x20 large;
        Font8x12 medium;

        Color dayBackground = Color.FromHex("06BFCC");
        Color darkFont = Color.FromHex("06416C");

        string[] images = new string[1]
        {
            "mystop.jpg"
        };

        public override async Task Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            var connectionResult = await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

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

            large = new Font12x20();
            medium = new Font8x12();

            onboardLed.SetColor(Color.Green);
        }

        public override async Task Run()
        {
            Console.WriteLine("Run");

            DrawBackgroundAndStopInfo();

            while (true) 
            {
                var arrivals = await BusService.GetSchedulesAsync("51195");
                DrawBusArrivals(arrivals);

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            
        }

        void DrawBackgroundAndStopInfo() 
        {
            DisplayJPG(0, 0, images[0]);

            graphics.CurrentFont = medium;

            graphics.DrawText(95, 50, "WB KINGSWAY FS WINDSOR ST", darkFont);
            graphics.DrawText(95, 85, "STOP #51195", darkFont, ScaleFactor.X2);

            graphics.Show();
        }

        void DrawBusArrivals(List<Schedule> arrivals) 
        {
            graphics.CurrentFont = large;

            graphics.DrawRectangle(15, 160, 290, 180, dayBackground, true);

            if (arrivals.Count == 0)
                return;

            if (arrivals.Count > 0 && arrivals[0] != null)
            {
                graphics.DrawText(15, 160, $"019 {arrivals[0].Destination}", darkFont);
                graphics.DrawText(305, 160, $"{arrivals[0].ExpectedCountdown} MIN", darkFont, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 1 && arrivals[1] != null)
            {
                graphics.DrawText(15, 200, $"019 {arrivals[1].Destination}", darkFont);
                graphics.DrawText(305, 200, $"{arrivals[1].ExpectedCountdown} MIN", darkFont, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 2 && arrivals[2] != null)
            {
                graphics.DrawText(15, 240, $"019 {arrivals[2].Destination}", darkFont);
                graphics.DrawText(305, 240, $"{arrivals[2].ExpectedCountdown} MIN", darkFont, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 3 && arrivals[3] != null)
            {
                graphics.DrawText(15, 280, $"019 {arrivals[3].Destination}", darkFont);
                graphics.DrawText(305, 280, $"{arrivals[3].ExpectedCountdown} MIN", darkFont, alignment: TextAlignment.Right);
            }

            if (arrivals.Count > 4 && arrivals[4] != null)
            {
                graphics.DrawText(15, 320, $"019 {arrivals[4].Destination}", darkFont);
                graphics.DrawText(305, 320, $"{arrivals[4].ExpectedCountdown} MIN", darkFont, alignment: TextAlignment.Right);
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

            graphics.Show();
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
    }
}