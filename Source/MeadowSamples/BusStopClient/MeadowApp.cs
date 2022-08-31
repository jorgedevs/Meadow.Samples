using BusStopClient.Controllers;
using BusStopClient.Services;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace BusStopClient
{
    public class MeadowApp : App<F7FeatherV1>
    {
        string BUS_STOP_NUMBER = "51195";

        RgbPwmLed onboardLed;

        public override async Task Initialize()
        {
            onboardLed = new RgbPwmLed(device: Device,
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
            DisplayController.Instance.DrawBackgroundAndStopInfo();

            while (true) 
            {
                onboardLed.StartBlink(Color.Orange);

                var arrivals = await BusService.GetSchedulesAsync(BUS_STOP_NUMBER);
                DisplayController.Instance.DrawBusArrivals(arrivals);

                onboardLed.Stop();
                onboardLed.SetColor(Color.Green);
                onboardLed.IsOn = true;

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }
}