using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace MarketGnss
{
    // Change F7CoreComputeV2 to F7FeatherV2 (or F7FeatherV1) for Feather boards
    public class MeadowApp : App<F7CoreComputeV2>
    {
        protected IGnssTrackerHardware gnssTracker { get; set; }

        protected DisplayScreen DisplayScreen { get; set; }

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            gnssTracker = GnssTracker.Create();

            DisplayScreen = new DisplayScreen(gnssTracker.Display, RotationType._270Degrees);

            var image = Image.LoadFromResource("MarketGnss.meadow_logo.bmp");
            var displayImage = new Picture(27, 27, 197, 28, image)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            DisplayScreen.Controls.Add(displayImage);

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, Meadow Core-Compute!");

            return base.Run();
        }
    }
}