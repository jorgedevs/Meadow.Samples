using BusStopClient.Controllers;
using BusStopClient.Models;
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
    public class MeadowApp : App<F7FeatherV2>
    {
        string BUS_STOP_NUMBER = "51195";
        Stop busStop;

        RgbPwmLed onboardLed;

        public override async Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                device: Device,
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

            await DateTimeService.Instance.GetDateTime();

            DisplayController.Instance.Initialize();
            DisplayController.Instance.DrawSplashScreen();

            onboardLed.SetColor(Color.Green);
        }

        public override async Task Run()
        {
            busStop = await BusService.Instance.GetStopInfoAsync(BUS_STOP_NUMBER);

            DisplayController.Instance.SetTheme();

            DisplayController.Instance.DrawBackgroundAndStopInfo();

            DisplayController.Instance.DrawStopInfo(busStop);

            while (true)
            {
                onboardLed.StartPulse(Color.Orange, 1, 0);

                var today = DateTime.Now;
                var sunset = DaylightTimes.GetDaylight(today.Month).Sunset;
                var sunrise = DaylightTimes.GetDaylight(today.Month).Sunrise;

                if (today.TimeOfDay >= sunrise.TimeOfDay
                    && today.TimeOfDay <= sunset.TimeOfDay) 
                {
                    DisplayController.Instance.SetTheme();
                    DisplayController.Instance.DrawBackgroundAndStopInfo();
                    DisplayController.Instance.DrawStopInfo(busStop);
                }

                
                //Console.WriteLine($"{today.Year}/{today.Month}/{today.Day} {today.Hour}:{today.Minute}:{today.Second}");

                var arrivals = await BusService.Instance.GetSchedulesAsync(BUS_STOP_NUMBER);
                DisplayController.Instance.DrawBusArrivals(arrivals);

                onboardLed.Stop();
                onboardLed.SetColor(Color.Green);

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }
    }
}