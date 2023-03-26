using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace MarketCharacterDisplay
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        CharacterDisplay display;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                    redPwmPin: Device.Pins.OnboardLedRed,
                    greenPwmPin: Device.Pins.OnboardLedGreen,
                    bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            display = new CharacterDisplay
            (
                pinRS: Device.Pins.D10,
                pinE: Device.Pins.D09,
                pinD4: Device.Pins.D08,
                pinD5: Device.Pins.D07,
                pinD6: Device.Pins.D06,
                pinD7: Device.Pins.D05,
                rows: 4, columns: 20
            );
            ShowSplashScreen();

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            wifi.NetworkConnected += NetworkConnected;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        private async void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            display.WriteLine($"  WILDERNESS LABS   ", 0);
            display.WriteLine($" DEVCAMP STARTS IN: ", 1);
            display.WriteLine($"  wldrn.es/devcamp  ", 3);

            while (true)
            {
                UpdateCountdown();
                await Task.Delay(1000);
            }
        }

        void ShowSplashScreen()
        {
            display.WriteLine("====================", 0);
            display.WriteLine(" DevCamp Countdown! ", 1);
            display.WriteLine("=   Joining WIFI   =", 2);
            display.WriteLine("====================", 3);
        }

        void UpdateCountdown()
        {
            int TimeZoneOffSet = -8; // PST
            var today = DateTime.Now.AddHours(TimeZoneOffSet);

            var christmasDate = new DateTime(today.Year, 05, 18);

            var countdown = christmasDate.Subtract(today);
            display.WriteLine($"  {countdown.Days.ToString("D3")}d {countdown.Hours.ToString("D2")}h {countdown.Minutes.ToString("D2")}m {countdown.Seconds.ToString("D2")}s", 2);
        }
    }
}