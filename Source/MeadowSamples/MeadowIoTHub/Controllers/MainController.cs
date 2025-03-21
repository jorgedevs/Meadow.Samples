using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowIoTHub.Controllers;

public class MainController
{
    int TIMEZONE_OFFSET = -8; // UTC-8

    IProjectLabHardware hardware;
    IWiFiNetworkAdapter network;
    IoTHubMqttController iotHubController;

    public MainController(IProjectLabHardware hardware, IWiFiNetworkAdapter network)
    {
        this.hardware = hardware;
        this.network = network;
    }

    public async Task Initialize()
    {
        await InitializeIoTHub();

        hardware.TemperatureSensor.Updated += TemperatureSensorUpdated;
    }

    private async Task InitializeIoTHub()
    {
        iotHubController = new IoTHubMqttController();

        while (!iotHubController.isAuthenticated)
        {
            if (network.IsConnected)
            {
                Resolver.Log.Info("Authenticating...");

                bool authenticated = await iotHubController.Initialize();

                if (authenticated)
                {
                    Resolver.Log.Info("Authenticated");
                    await Task.Delay(2000);
                    Resolver.Log.Info(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));
                }
                else
                {
                    Resolver.Log.Info("Not Authenticated");
                }
            }
            else
            {
                Resolver.Log.Info("Offline");
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    private async void TemperatureSensorUpdated(object sender, IChangeResult<Temperature> e)
    {
        hardware.RgbLed.StartBlink(Color.Orange);

        if (network.IsConnected && iotHubController.isAuthenticated)
        {
            Resolver.Log.Info("Sending data...");

            await iotHubController.SendTemperatureReading(e.New);

            Resolver.Log.Info("Data sent!");
            Thread.Sleep(3000);

            Resolver.Log.Info(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));
        }

        hardware.RgbLed.StartBlink(Color.Green);
    }

    public Task Run()
    {
        hardware.TemperatureSensor!.StartUpdating(TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }
}