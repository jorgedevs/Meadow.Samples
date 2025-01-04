using HomeWidget.Hardware;
using Meadow;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace HomeWidget.Controllers;

public class MainController
{
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

        hardware.EnvironmentalSensor.StartUpdating(TimeSpan.FromMinutes(30));
    }

    async Task UpdateOutdoorValues()
    {
        var outdoorConditions = await restClientController.GetWeatherForecast();

        if (outdoorConditions != null)
        {
            Resolver.Log.Info($"HTU21D - " +
                $"Temp: {hardware.EnvironmentalSensor.Temperature.Value.Celsius} | " +
                $"Humidity: {hardware.EnvironmentalSensor.Humidity.Value.Percent}");

            displayController.UpdateDisplay(
                weatherIcon: outdoorConditions.weatherIcon,
                temperature: outdoorConditions.temperature,
                feelsLike: outdoorConditions.feelsLike,
                humidity: outdoorConditions.humidity,
                pressure: outdoorConditions.pressure,
                sunrise: outdoorConditions.sunrise,
                sunset: outdoorConditions.sunset,
                indoorTemperature: hardware.EnvironmentalSensor.Temperature.Value.Celsius,
                indoorHumidity: hardware.EnvironmentalSensor.Humidity.Value.Percent);
        }
    }

    public async Task Run()
    {
        while (true)
        {
            if (network.IsConnected)
            {
                await UpdateOutdoorValues();

                await Task.Delay(TimeSpan.FromHours(1));
            }
            else
            {
                Resolver.Log.Trace("Not connected, checking in 10s");

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}