using BusStopClient.Controllers;
using BusStopClient.Services;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace BusStopClient
{
    public class MeadowApp : App<F7FeatherV1>
    {
        string BUS_STOP_NUMBER = "51195";

        RgbPwmLed onboardLed;
        PushButton button;

        DateTime activeTime;
        bool isFirstRun = true;
        bool isBusy, isMonitoring;

        public override async Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            Resolver.Log.Loglevel = Meadow.Logging.LogLevel.Trace;

            DisplayController.Instance.Initialize();
            DisplayController.Instance.DrawSplashScreen();

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            button = new PushButton(Device, Device.Pins.D04);
            button.Clicked += ButtonClicked;

            onboardLed.SetColor(Color.Green);
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            await UpdateBusArrivals();

            activeTime = DateTime.Now;
            activeTime = activeTime.Add(TimeSpan.FromMinutes(20));
        }

        async Task UpdateWeatherStatus() 
        {
            if (isBusy)
                return;
            isBusy = true;

            onboardLed.StartPulse(Color.Yellow);

            var weather = await WeatherService.Instance.GetWeatherForecast();
            DisplayController.Instance.UpdateWeatherStatus(weather);

            onboardLed.Stop();
            onboardLed.SetColor(Color.Green);

            isBusy = false;
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

            try
            {
                var busStop = await BusService.Instance.GetStopInfoAsync(BUS_STOP_NUMBER);

                while (true)
                {
                    //int TIMEZONE_OFFSET = -8;
                    var today = DateTime.Now;//.AddHours(TIMEZONE_OFFSET);

                    Console.WriteLine(today.ToString());

                    if (DisplayController.Instance.IsChangeThemeTime(today) || isFirstRun)
                    {
                        isFirstRun = false;
                        DisplayController.Instance.UpdateTheme();
                        DisplayController.Instance.DrawStopInfo(busStop);
                        await UpdateWeatherStatus();
                    }

                    DisplayController.Instance.UpdateClock(
                        today.ToString("T", CultureInfo.CreateSpecificCulture("en-us")));

                    if (today < activeTime)
                    {
                        isMonitoring = true;

                        if (today.Second == 0 
                            || today.Second == 1 
                            || today.Second == 30 
                            || today.Second == 31)
                        {
                            await UpdateBusArrivals();

                            GC.Collect();
                        }
                    }
                    else
                    {
                        if (isMonitoring 
                            || (today.Minute == 0 && today.Second == 0)
                            || (today.Minute == 15 && today.Second == 0)
                            || (today.Minute == 30 && today.Second == 0)
                            || (today.Minute == 45 && today.Second == 0))
                        {
                            isMonitoring = false;
                            await UpdateWeatherStatus();

                            GC.Collect();
                        }
                    }

                    DisplayController.Instance.Show();

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}