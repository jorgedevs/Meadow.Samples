using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Foundation.Servos;
using Meadow.Hardware;

namespace RotaryServo
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Servo servo;
        St7789 display;
        GraphicsLibrary graphics;
        RotaryEncoderWithButton rotary;
        RgbPwmLed onboardLed;

        public MeadowApp()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            var config = new SpiClockConfiguration(
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3);
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


            onboardLed.SetColor(Color.Green);
            Start();
        }

        void Start() 
        {
            graphics.Clear();

            graphics.DrawCircle(120, 120, 120, Color.White, true);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(90, 70, "SERVO", Color.Black);

            for(int i=0; i<180; i++) 
            {
                graphics.DrawRectangle(66, 98, 108, 60, Color.White, true);
                graphics.DrawText(66, 98, i.ToString("###"), Color.Black, GraphicsLibrary.ScaleFactor.X3);
                graphics.Show();
                Thread.Sleep(500);
            }

            for (int i=179; i>=0; i--)
            {
                graphics.DrawRectangle(66, 98, 108, 60, Color.White, true);
                graphics.DrawText(66, 98, i.ToString("###"), Color.Black, GraphicsLibrary.ScaleFactor.X3);
                graphics.Show();
                Thread.Sleep(500);
            }

            graphics.Show();
        }
    }
}
