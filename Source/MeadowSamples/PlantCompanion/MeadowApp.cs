using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;
using System;
using System.Threading;

namespace PlantCompanion
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.81f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.50f;

        RgbPwmLed onboardLed;
        Capacitive capacitive;
        St7789 display;
        GraphicsLibrary graphics;

        public MeadowApp()
        {
            Initialize();

            Start();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            var config = new SpiClockConfiguration
            (
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3
            );
            display = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(display);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            capacitive = new Capacitive(
                device: Device, 
                analogPin: Device.Pins.A00, 
                minimumVoltageCalibration: MINIMUM_VOLTAGE_CALIBRATION,
                maximumVoltageCalibration: MAXIMUM_VOLTAGE_CALIBRATION);


            onboardLed.SetColor(Color.Green);
        }

        void Start() 
        {
            graphics.Clear();

            graphics.Stroke = 3;
            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(120, 120, "99%", Color.Black, GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }
    }
}
