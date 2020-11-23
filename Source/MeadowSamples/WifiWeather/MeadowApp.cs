using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Gateway.WiFi;
using System;
using System.Threading;
using WifiWeather.ServiceAccessLayer;

namespace WifiWeather
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {        
        public MeadowApp()
        {
            Initialize();

            ClientServiceFacade.FetchReadings().Wait();

            Console.WriteLine("Done!");
        }

        void Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            Device.InitWiFiAdapter().Wait();

            onboardLed.SetColor(Color.Blue);

            var result = Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            onboardLed.SetColor(Color.Green);
        }
    }
}