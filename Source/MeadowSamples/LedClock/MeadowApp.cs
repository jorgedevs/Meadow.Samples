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
            
            Device.SetClock(new DateTime(2020, 03, 28, 00, 17, 00));

            Console.WriteLine("done");

            RunClock();
        }

        void RunClock() 
        {
            while(true)
            {
                DateTime clock = DateTime.Now;

                graphics.Clear();
                graphics.DrawText(0, 1, $"{clock:hh}");
                graphics.DrawText(0, 9, $"{clock:mm}");
                graphics.DrawText(0, 17, $"{clock:ss}");
                graphics.DrawText(0, 25, $"{clock:tt}");
                graphics.Show();

                Thread.Sleep(1000);
            }
        }
    }
}
