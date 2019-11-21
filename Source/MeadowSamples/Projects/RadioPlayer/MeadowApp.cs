using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;

namespace RadioPlayer
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        float currentFrequency;

        PushButton btnNext;
        PushButton btnPrevious;
        SSD1306 display;
        GraphicsLibrary graphics;

        public MeadowApp()
        {
            InitializePeripherals();
            Start();
        }

        public void InitializePeripherals()
        {
            Console.WriteLine("Creating Outputs...");

            var i2CBus = Device.CreateI2cBus();
            display = new SSD1306(i2CBus, 60, SSD1306.DisplayType.OLED128x32);
            graphics = new GraphicsLibrary(display);
            graphics.Rotation = GraphicsLibrary.RotationType._180Degrees;

            btnNext = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D03, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            btnNext.Clicked += BtnNextClicked;

            btnPrevious = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D04, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            btnPrevious.Clicked += BtnPreviousClicked;

            currentFrequency = 94.5f;
        }

        void BtnNextClicked(object sender, EventArgs e)
        {
            DisplayText("      >>>>      ", 0);
            Thread.Sleep(1000);
            currentFrequency++;
            DisplayText($"<- FM {currentFrequency.ToString()} ->");
        }

        void BtnPreviousClicked(object sender, EventArgs e)
        {
            DisplayText("      <<<<      ", 0);
            Thread.Sleep(1000);
            currentFrequency--;
            DisplayText($"<- FM {currentFrequency.ToString()} ->");
        }

        void Start()
        {
            Console.WriteLine("Start...");

            DisplayText("Radio Player");
            Thread.Sleep(3000);
            DisplayText($"<- FM {currentFrequency.ToString()} ->");
        }

        void DisplayText(string text, int x = 12)
        {
            graphics.Clear();
            graphics.CurrentFont = new Font8x12();
            graphics.DrawRectangle(0, 0, 128, 32);
            graphics.DrawText(x, 12, text);
            graphics.Show();
        }
    }
}
