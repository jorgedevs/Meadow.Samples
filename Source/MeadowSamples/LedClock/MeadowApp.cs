using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using System;
using System.Threading;

namespace LedClock
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Max7219 display;
        GraphicsLibrary graphics;

        public MeadowApp()
        {
            Console.Write("Initializing...");

            display = new Max7219(Device, Device.CreateSpiBus(), Device.Pins.D01, 4, Max7219.Max7219Type.Display);

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = GraphicsLibrary.RotationType._180Degrees;

            Console.WriteLine("done");

            TestClock();
        }

        void TestClock() 
        {
            Console.Write("TestClock...");

            DateTime clock = new DateTime(2020, 03, 23, 10, 25, 35);

            for (int i = 0; i < 200; i++)
            {
                graphics.Clear();
                graphics.DrawText(0, 1, $"{clock.Hour}");
                graphics.DrawText(0, 09, $"{clock.Minute}");
                graphics.DrawText(0, 17, $"{clock.Second}");
                graphics.DrawText(0, 25, $"JR");
                graphics.Show();

                Thread.Sleep(1000);
                clock = clock.AddSeconds(1);
            }

            Console.WriteLine("done");
        }
    }
}
