using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Rotary;
using System;

namespace EdgeASketch
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int x, y;
        Color color;
        St7789 st7789;
        GraphicsLibrary graphics;
        RotaryEncoder rotaryX;
        RotaryEncoder rotaryY;

        public MeadowApp()
        {
            x = 120;
            y = 120;

            color = Color.Black;

            var config = new SpiClockConfiguration(
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3);
            st7789 = new St7789(
                device: Device,
                spiBus: Device.CreateSpiBus(
                    clock: Device.Pins.SCK,
                    mosi: Device.Pins.MOSI,
                    miso: Device.Pins.MISO,
                    config: config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            graphics = new GraphicsLibrary(st7789);
            graphics.Clear(true);
            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);
            graphics.DrawPixel(x, y, color);
            graphics.Show();

            rotaryX = new RotaryEncoder(
                Device.CreateDigitalInputPort(Device.Pins.A00, InterruptMode.EdgeBoth, ResistorMode.PullUp, 0, 10),
                Device.CreateDigitalInputPort(Device.Pins.A01, InterruptMode.EdgeBoth, ResistorMode.PullUp, 0, 10));
            rotaryX.Rotated += RotaryXRotated;

            rotaryY = new RotaryEncoder(
                Device.CreateDigitalInputPort(Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.PullUp, 0, 10),
                Device.CreateDigitalInputPort(Device.Pins.D03, InterruptMode.EdgeBoth, ResistorMode.PullUp, 0, 10));
            rotaryY.Rotated += RotaryYRotated;

            Console.WriteLine("done");
        }

        void RotaryXRotated(object sender, RotaryTurnedEventArgs e)
        {
            if (e.Direction == RotationDirection.Clockwise)
                x++;
            else
                x--;

            graphics.DrawPixel(x, y + 1, Color.Red);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.DrawPixel(x, y - 1, Color.Red);
            graphics.Show();
        }

        void RotaryYRotated(object sender, RotaryTurnedEventArgs e)
        {
            if (e.Direction == RotationDirection.Clockwise)
                y++;
            else
                y--;

            graphics.DrawPixel(x + 1, y, Color.Red);
            graphics.DrawPixel(x, y, Color.Red);
            graphics.DrawPixel(x - 1, y, Color.Red);
            graphics.Show();
        }
    }
}