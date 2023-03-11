using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using MeadowSmartFrame.Controllers;
using System;
using System.Threading.Tasks;

namespace MeadowSmartFrame
{
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;

        public override async Task Initialize()
        {
            Console.WriteLine("Initialize...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            DisplayController.Instance.Initialize();
            DisplayController.Instance.DrawSplashScreen();

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));


            onboardLed.SetColor(Color.Green);
        }

        public override Task Run()
        {
            Console.WriteLine("Run...");

            return base.Run();
        }
    }
}