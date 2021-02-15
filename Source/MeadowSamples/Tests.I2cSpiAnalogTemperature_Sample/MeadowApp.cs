using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
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
        St7789 displaySPI;
        GraphicsLibrary graphicsSPI;
        Ssd1306 displayI2C;
        GraphicsLibrary graphicsI2C;
        List<AnalogTemperature> temperatures;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            Console.WriteLine("Start...");

            Console.Write("Initializing I2C...");
            displayI2C = new Ssd1306(Device.CreateI2cBus(), 60, Ssd1306.DisplayType.OLED128x32);
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
            graphicsSPI = new GraphicsLibrary(displaySPI);
            graphicsSPI.Rotation = GraphicsLibrary.RotationType._90Degrees;
            graphicsSPI.Clear();
            graphicsSPI.Stroke = 3;
            graphicsSPI.DrawRectangle(0, 0, 240, 240);
            graphicsSPI.CurrentFont = new Font8x12();
            graphicsSPI.DrawText(24, 12, "SPI WORKING!", Color.White, GraphicsLibrary.ScaleFactor.X2);
            graphicsSPI.Show();
            Console.WriteLine("done");

            temperatures = new List<AnalogTemperature>
            {
                //new AnalogTemperature(Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35),
                //new AnalogTemperature(Device, Device.Pins.A01, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A02, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A03, AnalogTemperature.KnownSensorType.LM35),
                new AnalogTemperature(Device, Device.Pins.A04, AnalogTemperature.KnownSensorType.LM35), 
                new AnalogTemperature(Device, Device.Pins.A05, AnalogTemperature.KnownSensorType.LM35),
            };

            led.SetColor(RgbLed.Colors.Green);

            TestTemperatures();
        }

        async Task TestTemperatures()
        {
            while (true)
            {
                int tempIndex = 0;
                float? temp;

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
                        graphicsSPI.DrawRectangle(7, i * 32 + 44, 208, 24, Color.Black, true);
                        graphicsSPI.DrawText(7, i * 32 + 44, $"A0{i}: INACTIVE", Color.Red, GraphicsLibrary.ScaleFactor.X2);
                    }
                    else
                    {
                        graphicsSPI.DrawRectangle(7, i * 32 + 44, 208, 24, Color.Black, true);
                        graphicsSPI.DrawText(7, i * 32 + 44, $"{temperatures[tempIndex].AnalogInputPort.Pin}: {temp}", Color.Cyan, GraphicsLibrary.ScaleFactor.X2);
                        tempIndex++;
                    }

                    graphicsSPI.Show();
                    Thread.Sleep(100);
                }

                Thread.Sleep(3000);
            }
        }
    }
}