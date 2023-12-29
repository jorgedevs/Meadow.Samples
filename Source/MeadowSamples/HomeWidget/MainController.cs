using HomeWidget.Controllers;
using HomeWidget.Hardware;
using Meadow;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace HomeWidget
{
    internal class MainController
    {
        bool firstWeatherForecast = true;

        private IHomeWidgetHardware hardware;
        private INetworkAdapter network;

        private DisplayController displayController;
        private RestClientController restClientController;

        public MainController(IHomeWidgetHardware hardware, IWiFiNetworkAdapter network)
        {
            this.hardware = hardware;
            this.network = network;
        }

        public void Initialize()
        {
            hardware.Initialize();

            displayController = new DisplayController(hardware.Display);
            restClientController = new RestClientController();

            //displayController.ShowSplashScreen();
            //Thread.Sleep(3000);
            //displayController.ShowDataScreen();
        }

        async Task UpdateOutdoorValues()
        {
            var outdoorConditions = await restClientController.GetWeatherForecast();

            if (outdoorConditions != null)
            {
                firstWeatherForecast = false;

                displayController.UpdateDisplay(
                    weatherIcon: outdoorConditions.Value.Item1,
                    temperature: outdoorConditions.Value.Item2,
                    humidity: outdoorConditions.Value.Item3);
            }
        }

        public async Task Run()
        {
            while (true)
            {
                if (network.IsConnected)
                {
                    int TimeZoneOffSet = -8; // PST
                    var today = DateTime.Now.AddHours(TimeZoneOffSet);

                    if (today.Minute == 0 ||
                        today.Minute == 30 ||
                        firstWeatherForecast)
                    {
                        Resolver.Log.Trace("Getting forecast values...");

                        await UpdateOutdoorValues();

                        Resolver.Log.Trace("Forecast acquired!");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                else
                {
                    Resolver.Log.Trace("Not connected, checking in 10s");

                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}