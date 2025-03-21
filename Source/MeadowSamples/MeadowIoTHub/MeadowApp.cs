using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using MeadowIoTHub.Controllers;
using System.Threading.Tasks;

namespace MeadowIoTHub;

public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var network = Hardware.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        mainController = new MainController(Hardware, network);

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await mainController.Run();
    }
}