using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;

namespace Displays.PCD8544_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Pcd8544 display;
        GraphicsLibrary graphics;

        public MeadowApp()
        {
            var spiBus = Device.CreateSpiBus();

            display = new Pcd8544
            (
                device: Device,
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D01,
                dcPin: Device.Pins.D00,
                resetPin: Device.Pins.D02
            );
            graphics = new GraphicsLibrary(display);

            Console.WriteLine("Test display API");
            TestRawDisplayAPI();
            Thread.Sleep(1000);

            Console.WriteLine("Test Graphics Library");
            TestDisplayGraphicsAPI();
        }

        void TestRawDisplayAPI() 
        {
            for (int i = 0; i < 35; i++)
            {
                display.DrawPixel(i, i, true);
            }

            display.Show();
        }

        void TestDisplayGraphicsAPI() 
        {
            graphics.Clear(true);
            graphics.CurrentFont = new Font8x12();
            graphics.DrawText(0, 0, "Meadow F7");
            graphics.DrawRectangle(5, 14, 30, 10, true);
            graphics.Show();
        }
    }
}