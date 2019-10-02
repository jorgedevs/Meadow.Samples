using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace Displays.Tft.ILI9163_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly Color WatchBackgroundColor = Color.White;

        protected ISpiBus spiBus;
        protected ILI9163 iLI9163;
        protected GraphicsLibrary display;
        protected int hour, minute, second, tick;

        public MeadowApp()
        {
            spiBus = Device.CreateSpiBus();// Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, 2000);

            iLI9163 = new ILI9163(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 128, height: 160);
            iLI9163.ClearScreen(31);
            iLI9163.Refresh();

            display = new GraphicsLibrary(iLI9163);

            DrawClock();
        }

        protected void DrawClock()
        {
            hour = 8;
            minute = 54;
            DrawWatchFace();
            while (true)
            {
                tick++;
                Thread.Sleep(1000);
                UpdateClock(second: tick % 60);
            }
        }
        protected void DrawWatchFace()
        {
            display.Clear();
            int xCenter = 64;
            int yCenter = 80;
            int x, y;
            display.CurrentFont = new Font8x12();
            display.DrawText(4, 4, "Wilderness Labs");
            display.DrawText(16, 144, "Meadow Clock");//Meadow Clock
            display.DrawCircle(xCenter, yCenter, 56, WatchBackgroundColor, true);
            for (int i = 0; i < 60; i++)
            {
                x = (int)(xCenter + 50 * System.Math.Sin(i * System.Math.PI / 30));
                y = (int)(yCenter - 50 * System.Math.Cos(i * System.Math.PI / 30));
                if (i % 5 == 0)
                    display.DrawCircle(x, y, 2, Color.Black, true);
                else
                    display.DrawPixel(x, y, Color.Black);
            }
        }
        protected void UpdateClock(int second = 0)
        {
            int xCenter = 64, yCenter = 80;
            int x, y, xT, yT;

            if (second == 0)
            {
                minute++;
                if (minute == 60)
                {
                    minute = 0;
                    hour++;
                    if (hour == 12)
                    {
                        hour = 0;
                    }
                }
            }
            //remove previous hour
            int previousHour = (hour - 1) < -1 ? 11 : (hour - 1);
            x = (int)(xCenter + 23 * System.Math.Sin(previousHour * System.Math.PI / 6));
            y = (int)(yCenter - 23 * System.Math.Cos(previousHour * System.Math.PI / 6));
            xT = (int)(xCenter + 3 * System.Math.Sin((previousHour - 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousHour - 3) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            xT = (int)(xCenter + 3 * System.Math.Sin((previousHour + 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousHour + 3) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            //current hour
            x = (int)(xCenter + 23 * System.Math.Sin(hour * System.Math.PI / 6));
            y = (int)(yCenter - 23 * System.Math.Cos(hour * System.Math.PI / 6));
            xT = (int)(xCenter + 3 * System.Math.Sin((hour - 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((hour - 3) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, Color.Black);
            xT = (int)(xCenter + 3 * System.Math.Sin((hour + 3) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((hour + 3) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, Color.Black);
            //remove previous minute
            int previousMinute = minute - 1 < -1 ? 59 : (minute - 1);
            x = (int)(xCenter + 35 * System.Math.Sin(previousMinute * System.Math.PI / 30));
            y = (int)(yCenter - 35 * System.Math.Cos(previousMinute * System.Math.PI / 30));
            xT = (int)(xCenter + 3 * System.Math.Sin((previousMinute - 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousMinute - 15) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            xT = (int)(xCenter + 3 * System.Math.Sin((previousMinute + 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((previousMinute + 15) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            //current minute
            x = (int)(xCenter + 35 * System.Math.Sin(minute * System.Math.PI / 30));
            y = (int)(yCenter - 35 * System.Math.Cos(minute * System.Math.PI / 30));
            xT = (int)(xCenter + 3 * System.Math.Sin((minute - 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((minute - 15) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, Color.Black);
            xT = (int)(xCenter + 3 * System.Math.Sin((minute + 15) * System.Math.PI / 6));
            yT = (int)(yCenter - 3 * System.Math.Cos((minute + 15) * System.Math.PI / 6));
            display.DrawLine(xT, yT, x, y, Color.Black);
            //remove previous second
            int previousSecond = second - 1 < -1 ? 59 : (second - 1);
            x = (int)(xCenter + 40 * System.Math.Sin(previousSecond * System.Math.PI / 30));
            y = (int)(yCenter - 40 * System.Math.Cos(previousSecond * System.Math.PI / 30));
            display.DrawLine(xCenter, yCenter, x, y, WatchBackgroundColor);
            //current second
            x = (int)(xCenter + 40 * System.Math.Sin(second * System.Math.PI / 30));
            y = (int)(yCenter - 40 * System.Math.Cos(second * System.Math.PI / 30));
            display.DrawLine(xCenter, yCenter, x, y, Color.Red);
            display.Show();
        }
    }
}