using BusStopClient.Controllers;
using BusStopClient.Services;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace BusStopClient
{
    public class MeadowApp : App<F7FeatherV2>
    {
        string BUS_STOP_NUMBER = "51195";

        RgbPwmLed onboardLed;
        PushButton button;

        DateTime activeTime;
        bool isFirstRun = true;
        bool isBusy;

        public override async Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            DisplayController.Instance.Initialize();
            DisplayController.Instance.DrawSplashScreen();

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            var connectionResult = await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            await DateTimeService.Instance.GetDateTime();

            button = new PushButton(Device, Device.Pins.D04);
            button.Clicked += ButtonClicked;

            onboardLed.SetColor(Color.Green);
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            await UpdateBusArrivals();

            activeTime = DateTime.Now;
            activeTime = activeTime.Add(TimeSpan.FromMinutes(5));
        }

        async Task UpdateBusArrivals() 
        {
            if (isBusy)
                return;
            isBusy = true;

            onboardLed.StartPulse(Color.Magenta);

            var arrivals = await BusService.Instance.GetSchedulesAsync(BUS_STOP_NUMBER);
            DisplayController.Instance.DrawBusArrivals(arrivals);

            onboardLed.Stop();
            onboardLed.SetColor(Color.Green);

            isBusy = false;
        }

        public override async Task Run()
        {
            var busStop = await BusService.Instance.GetStopInfoAsync(BUS_STOP_NUMBER);

            while (true)
            {
                var today = DateTime.Now;

                if (today.Second == 0 && DisplayController.Instance.IsChangeThemeTime(today) || isFirstRun)
                {
                    isFirstRun = false;
                    DisplayController.Instance.UpdateTheme();
                    DisplayController.Instance.DrawStopInfo(busStop);
                }

                DisplayController.Instance.UpdateClock(
                    today.ToString("T", CultureInfo.CreateSpecificCulture("en-us")));

                if (today < activeTime)
                {
                    if (today.Second == 0 
                        || today.Second == 1 
                        || today.Second == 30 
                        || today.Second == 31)
                    {
                        await UpdateBusArrivals();
                    }
                }
                else
                {
                    DisplayController.Instance.ClearBusArrivals();
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}