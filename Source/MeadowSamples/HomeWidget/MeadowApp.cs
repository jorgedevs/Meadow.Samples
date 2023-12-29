using HomeWidget.Hardware;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace HomeWidget;

public class MeadowApp : App<F7FeatherV2>
{
    MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new HomeWidgetHardware();
        var network = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        mainController = new MainController(hardware, network);
        mainController.Initialize();

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await mainController.Run();
    }
}