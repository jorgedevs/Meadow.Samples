using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace I2cSpiAnalogTemperature_Sample
{
    public class MeadowApp : App<F7FeatherV1>
    {
        IWiFiNetworkAdapter wifi;

        RgbLed led;
        MicroGraphics graphicsSPI;
        MicroGraphics graphicsI2C;
        List<AnalogTemperature> temperatures;

        public MeadowApp()
        {
            led = new RgbLed(
                Device.Pins.OnboardLedRed, 
                Device.Pins.OnboardLedGreen, 
                Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLedColors.Red);

            Console.WriteLine("Start...");

            Console.Write("Initializing I2C...");
            var displayI2C = new Ssd1306(Device.CreateI2cBus(), 60, Ssd1306.DisplayType.OLED128x32);
            graphicsI2C = new MicroGraphics(displayI2C);
            graphicsI2C.CurrentFont = new Font8x12();
            graphicsI2C.Clear();
            graphicsI2C.Stroke = 1;
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.DrawText(16, 12, "I2C WORKING!");
            graphicsI2C.Show();
            Console.WriteLine("done");

            Console.Write("Initializing SPI...");
            var config = new SpiClockConfiguration(
                speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
                mode: SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(
                clock: Device.Pins.SCK,
                copi: Device.Pins.MOSI,
                cipo: Device.Pins.MISO,
                config: config);
            var displaySPI = new St7789(
                spiBus: spiBus,
                chipSelectPin: null,
                dcPin: Device.Pins.D14,
                resetPin: Device.Pins.D13,
                width: 240, height: 240); 
            graphicsSPI = new MicroGraphics(displaySPI);
            graphicsSPI.Rotation = RotationType._90Degrees;
            graphicsSPI.Clear();
            graphicsSPI.Stroke = 3;
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.CurrentFont = new Font8x12();
            graphicsSPI.DrawText(24, 108, "SPI WORKING!", Color.White, ScaleFactor.X2);
            graphicsSPI.Show();
            Console.WriteLine("done");

            Thread.Sleep(1000);

            temperatures = new List<AnalogTemperature>
            {
                new AnalogTemperature(Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device.Pins.A01, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device.Pins.A02, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device.Pins.A03, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device.Pins.A04, AnalogTemperature.KnownSensorType.LM35), 
                new AnalogTemperature(Device.Pins.A05, AnalogTemperature.KnownSensorType.LM35),
            };

            led.SetColor(RgbLedColors.Green);

            TestWifi().Wait();

            TestTemperatures().Wait();
        }

        async Task TestWifi()
        {
            bool testPassed = true;

            led.SetColor(RgbLedColors.Blue);

            graphicsSPI.Clear();
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.DrawText(40, 12, "WIFI TESTS", Color.Cyan, ScaleFactor.X2);
            graphicsSPI.Show();

            graphicsI2C.Clear();
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.Show();

            graphicsSPI.DrawText(16, 44, "Scanning...", Color.White, ScaleFactor.X2);
            graphicsSPI.Show();

            wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            var networks = await wifi.Scan(TimeSpan.FromSeconds(60));
            if (networks.Count > 0)
            {
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
                Console.WriteLine("|-------------------------------------------------------------|---------|");               
                for (int i=0; i<networks.Count; i++)
                {
                    Console.WriteLine($"| {networks[i].Ssid,-32} | {networks[i].SignalDbStrength,4} | {networks[i].Bssid,17} |   {networks[i].ChannelCenterFrequency,3}   |");                    
                }

                graphicsSPI.DrawText(32, 76, $"Found {networks.Count}!", Color.FromHex("#00FF00"), ScaleFactor.X2);
                graphicsSPI.Show();
            }
            else
            {
                Console.WriteLine($"No access points detected.");
                graphicsSPI.DrawText(32, 76, $"None found!", Color.Red, ScaleFactor.X2);
                graphicsSPI.Show();
            }

            if (testPassed)
            {
                graphicsSPI.DrawText(16, 108, "Joining...", Color.White, ScaleFactor.X2);
                graphicsSPI.Show();
            
                Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");
                await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

                if (wifi.IsConnected)
                {
                    testPassed = true;
                    graphicsSPI.DrawText(32, 140, "Connected!", Color.FromHex("#00FF00"), ScaleFactor.X2);
                    graphicsSPI.Show();
                }
                else
                {
                    testPassed = false;
                    graphicsSPI.DrawText(32, 140, "Failed!", Color.Red, ScaleFactor.X2);
                    graphicsSPI.Show();
                }
            }

            if (testPassed)
            {
                graphicsSPI.DrawText(16, 172, "Requesting...", Color.White, ScaleFactor.X2);
                graphicsSPI.Show();

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 5, 0);

                    try
                    {
                        string uri = "https://postman-echo.com/get?foo1=bar1&foo2=bar2";
                        HttpResponseMessage response = await client.GetAsync(uri);
                    
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                        graphicsSPI.DrawText(32, 204, "Returned OK!", Color.FromHex("#00FF00"), ScaleFactor.X2);
                        graphicsSPI.Show();
                    }
                    catch (TaskCanceledException)
                    {
                        testPassed = false;
                        Console.WriteLine("Request time out.");
                        graphicsSPI.DrawText(32, 204, "Failed!", Color.Red, ScaleFactor.X2);
                        graphicsSPI.Show();
                    }
                    catch (Exception e)
                    {
                        testPassed = false;
                        Console.WriteLine($"Request went sideways: {e.Message}");
                        graphicsSPI.DrawText(32, 204, "Failed!", Color.Red, ScaleFactor.X2);
                        graphicsSPI.Show();
                    }
                }
            }

            graphicsI2C.DrawText(36, 12, testPassed? "PASSED!" : "FAILED!");
            graphicsI2C.Show();

            led.Stop();
            led.SetColor(testPassed ? RgbLedColors.Green : RgbLedColors.Red);
            led.IsOn = true;

            await Task.Delay(3000);
        }

        async Task TestTemperatures()
        {
            led.SetColor(RgbLedColors.Green);

            graphicsSPI.Clear();
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.DrawText(24, 108, "ANALOG TESTS", Color.White, ScaleFactor.X2);
            graphicsSPI.Show();

            graphicsI2C.Clear();
            graphicsI2C.DrawRectangle(0, 0, 128, 32);
            graphicsI2C.Show();

            await Task.Delay(3000);

            graphicsSPI.Clear();
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.DrawText(56, 12, $"READINGS", Color.Cyan, ScaleFactor.X2);

            while (true)
            {
                int tempIndex = 0;
                double? temp;
                float average = 0;

                for (int i = 0; i < 6; i++)
                {
                    temp = null;

                    if (tempIndex < temperatures.Count)
                    {
                        //if ($"A0{i}" == temperatures[tempIndex]..AnalogInputPort.Pin.ToString())
                        //{
                            var conditions = await temperatures[i].Read();
                            temp = conditions.Celsius;
                        //}
                    }

                    if (temp == null)
                    {
                        graphicsSPI.DrawRectangle(16, i * 32 + 44, 208, 24, Color.Black, true);
                        graphicsSPI.DrawText(16, i * 32 + 44, $"A0{i}: INACTIVE", Color.Red, ScaleFactor.X2);
                    }
                    else
                    {
                        graphicsSPI.DrawRectangle(16, i * 32 + 44, 208, 24, Color.Black, true);
                        graphicsSPI.DrawText(16, i * 32 + 44, $"A{i}: {temp?.ToString("##.######")}", Color.FromHex("#00FF00"), ScaleFactor.X2);
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