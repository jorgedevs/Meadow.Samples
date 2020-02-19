using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using System;

namespace TemperatureMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        St7789 st7789;
        GraphicsLibrary graphics;
        int displayWidth, displayHeight;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);
            st7789 = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );
            displayWidth = Convert.ToInt32(st7789.Width);
            displayHeight = Convert.ToInt32(st7789.Height);

            graphics = new GraphicsLibrary(st7789);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            LoanScreen();
        }

        void LoanScreen() 
        {
            Console.WriteLine("LoanScreen...");

            graphics.Clear();

            var rand = new Random();
            int radius = 225;
            int originX = displayWidth / 2;
            int originY = displayHeight / 2 + 130;

            //var colors = new Color[4] { Color.FromHex("#006363"), Color.FromHex("#1D7373"), Color.FromHex("#009999"), Color.FromHex("#33CCCC") }; //blue
            var colors = new Color[4] { Color.FromHex("#A0000F"), Color.FromHex("#FB717E"), Color.FromHex("#FB3F51"), Color.FromHex("#F60018") }; //red

            graphics.Stroke = 3;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawCircle
                (
                    centerX: originX,
                    centerY: originY,
                    radius: radius,
                    color: colors[i - 1],
                    filled: true
                );

                graphics.Show();
                radius -= 20;
            }

            graphics.DrawLine(0, 220, 240, 220, Color.White);
            graphics.DrawLine(0, 230, 240, 230, Color.White);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(48, 140, "22.5°C", Color.White, GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }
    }
}