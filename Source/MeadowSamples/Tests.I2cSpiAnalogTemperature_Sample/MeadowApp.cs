using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Meadow.Foundation.Displays.DisplayBase;

namespace Tests.I2cSpiAnalogTemperature_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbLed led;
        St7789 displaySPI;
        GraphicsLibrary graphicsSPI;
        Ssd1306 displayI2C;
        GraphicsLibrary graphicsI2C;
        List<AnalogTemperature> temperatures;

        public MeadowApp()
        {
            led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            Console.WriteLine("Start...");

            Console.Write("Initializing I2C...");
            displayI2C = new Ssd1306(Device.CreateI2cBus(), 60, Ssd1306.DisplayType.OLED128x32);
            displayI2C.IgnoreOutOfBoundsPixels = true;
            graphicsI2C = new GraphicsLibrary(displayI2C);
            graphicsI2C.CurrentFont = new Font8x12();
            graphicsI2C.Clear();
            graphicsI2C.Stroke = 1;
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.DrawText(16, 12, "I2C WORKING!");
            graphicsI2C.Show();
            Console.WriteLine("done");

            Console.Write("Initializing SPI...");
            var config = new SpiClockConfiguration(24000, SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            displaySPI = new St7789
            (
                device: Device,
                spiBus: spiBus,
                chipSelectPin: null,
                dcPin: Device.Pins.D14,
                resetPin: Device.Pins.D13,
                width: 240, height: 240,
                displayColorMode: DisplayColorMode.Format16bppRgb565
            ); 
            displaySPI.IgnoreOutOfBoundsPixels = true;
            graphicsSPI = new GraphicsLibrary(displaySPI);
            graphicsSPI.Rotation = GraphicsLibrary.RotationType._90Degrees;
            graphicsSPI.Clear();
            graphicsSPI.Stroke = 3;
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.CurrentFont = new Font8x12();
            graphicsSPI.DrawText(24, 108, "SPI WORKING!", Color.White, GraphicsLibrary.ScaleFactor.X2);
            graphicsSPI.Show();
            Console.WriteLine("done");

            temperatures = new List<AnalogTemperature>
            {
                new AnalogTemperature(Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A01, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A02, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A03, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A04, AnalogTemperature.KnownSensorType.LM35), 
                new AnalogTemperature(Device, Device.Pins.A05, AnalogTemperature.KnownSensorType.LM35),
            };

            led.SetColor(RgbLed.Colors.Green);

            TestWifi().Wait();

            TestTemperatures().Wait();
        }

        async Task TestWifi() 
        {
            led.SetColor(RgbLed.Colors.Blue);

            graphicsSPI.Clear();            
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.DrawText(40, 108, "WIFI TESTS", Color.White, GraphicsLibrary.ScaleFactor.X2);
            graphicsSPI.Show();

            graphicsI2C.Clear();
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.Show();

            graphicsSPI.Clear();            
            graphicsSPI.DrawRectangle(0, 0, 240, 240);

            if (!Device.InitWiFiAdapter().Result)
                throw new Exception("Could not initialize the WiFi adapter.");

            Device.WiFiAdapter.WiFiConnected += WiFiAdapterConnectionCompleted;

            var networks = Device.WiFiAdapter.Scan();
            if (networks.Count > 0)
            {
                graphicsSPI.DrawText(16, 12, $"NETWORKS ({networks.Count})", Color.White, GraphicsLibrary.ScaleFactor.X2);

                Console.WriteLine("|-------------------------------------------------------------|---------|");
                Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                //foreach (WifiNetwork accessPoint in networks)
                for (int i=0; i<networks.Count; i++)
                {
                    Console.WriteLine($"| {networks[i].Ssid,-32} | {networks[i].SignalDbStrength,4} | {networks[i].Bssid,17} |   {networks[i].ChannelCenterFrequency,3}   |");
                    graphicsSPI.DrawText(16, i * 32 + 44, networks[i].Ssid, GraphicsLibrary.ScaleFactor.X2);
                }

                graphicsSPI.Show();
            }
            else
            {
                Console.WriteLine($"No access points detected.");
            }

            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");
            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            graphicsI2C.Clear();
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.DrawText(20, 12, "JOINED WIFI");
            graphicsI2C.Show();

            led.Stop();
            led.SetColor(RgbLed.Colors.Green);
            led.IsOn = true;

            await Task.Delay(5000);
        }

        void WiFiAdapterConnectionCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Connection request completed.");
        }

        async Task TestTemperatures()
        {
            graphicsSPI.Clear();
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.DrawText(24, 108, "ANALOG TESTS", Color.White, GraphicsLibrary.ScaleFactor.X2);
            graphicsSPI.Show();

            graphicsI2C.Clear();
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.Show();

            await Task.Delay(3000);

            graphicsSPI.Clear();
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.DrawText(56, 12, $"READINGS", Color.White, GraphicsLibrary.ScaleFactor.X2);

            while (true)
            {
                int tempIndex = 0;
                float? temp;
                float average = 0;

                for (int i = 0; i < 6; i++)
                {
                    temp = null;

                    if (tempIndex < temperatures.Count)
                    {
                        if ($"A0{i}" == temperatures[tempIndex].AnalogInputPort.Pin.ToString())
                        {
                            var conditions = await temperatures[tempIndex].Read();
                            temp = conditions.Temperature;
                        }
                    }

                    if (temp == null)
                    {
                        graphicsSPI.DrawRectangle(16, i * 32 + 44, 208, 24, Color.Black, true);
                        graphicsSPI.DrawText(16, i * 32 + 44, $"A0{i}: INACTIVE", Color.Red, GraphicsLibrary.ScaleFactor.X2);
                    }
                    else
                    {
                        graphicsSPI.DrawRectangle(16, i * 32 + 44, 208, 24, Color.Black, true);
                        graphicsSPI.DrawText(16, i * 32 + 44, $"{temperatures[tempIndex].AnalogInputPort.Pin}: {temp}", Color.FromHex("#00FF00"), GraphicsLibrary.ScaleFactor.X2);
                        tempIndex++;
                        average = average + (float)temp;
                    }

                    graphicsSPI.Show();
                    Thread.Sleep(100);
                }

                Thread.Sleep(500);
                graphicsI2C.Clear();
                graphicsI2C.DrawRectangle(0, 0, 128, 32);
                graphicsI2C.DrawText(12, 12, $"AVG: {average / temperatures.Count}");
                graphicsI2C.Show();
            }
        }
    }
}