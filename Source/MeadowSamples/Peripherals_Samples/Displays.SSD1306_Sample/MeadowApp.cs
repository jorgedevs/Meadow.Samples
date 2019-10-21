using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;

namespace Displays.SSD1306_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        protected SSD1306 display;
        protected GraphicsLibrary graphics;

        public MeadowApp()
        {
            var i2CBus = Device.CreateI2cBus();
            display = new SSD1306(i2CBus, 60, SSD1306.DisplayType.OLED128x32);
            graphics = new GraphicsLibrary(display);

            Console.WriteLine("Test display API");
            TestRawDisplayAPI();
            Thread.Sleep(1000);

            Console.WriteLine("Test Graphics Library");
            TestDisplayGraphicsAPI();
        }

        void TestRawDisplayAPI()
        {
            display.Clear(true);

            for (int i = 0; i < 30; i++)
            {
                display.DrawPixel(i, i, true);
                display.DrawPixel(30 + i, i, true);
                display.DrawPixel(60 + i, i, true);
            }

            display.Show();
        }

        void TestDisplayGraphicsAPI()
        {
            graphics.Clear();

            graphics.CurrentFont = new Font8x12();
            graphics.DrawText(0, 0, "Hello, Meadow F7");
            graphics.DrawRectangle(5, 14, 30, 10, true);

            graphics.Show();
        }
    }
}