using BusStopClient.Controllers;
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
        //MicroGraphics graphics;
        //Font12x20 large;
        //Font8x12 medium;

        //Color dayBackground = Color.FromHex("06BFCC");
        //Color darkFont = Color.FromHex("06416C");

        //string[] images = new string[1]
        //{
        //    "mystop.jpg"
        //};

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

            DisplayController.Instance.Initialize();
            DisplayController.Instance.DrawSplashScreen();

            onboardLed.SetColor(Color.Green);
        }

        public override async Task Run()
        {
            Console.WriteLine("Run");

            DisplayController.Instance.DrawBackgroundAndStopInfo();

            while (true) 
            {
                var arrivals = await BusService.GetSchedulesAsync("51195");
                DisplayController.Instance.DrawBusArrivals(arrivals);

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }
}