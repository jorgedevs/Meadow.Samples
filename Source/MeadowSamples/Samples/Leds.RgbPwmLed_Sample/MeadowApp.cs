using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;

namespace Leds.RgbPwmLed_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly RgbPwmLed rgbPwmLed;

        public MeadowApp()
        {
            rgbPwmLed = new RgbPwmLed(
                Device.CreatePwmPort(Device.Pins.OnboardLedRed),
                Device.CreatePwmPort(Device.Pins.OnboardLedGreen),
                Device.CreatePwmPort(Device.Pins.OnboardLedBlue));

            TestRgbPwmLed();
        }

        protected void TestRgbPwmLed()
        {
            while (true)
            {
                //for (int i = 0; i < (int)RgbLed.Colors.count; i++)
                //{
                //    rgbLed.SetColor((RgbLed.Colors)i);
                //    Console.WriteLine(((RgbLed.Colors)i).ToString());
                //    Thread.Sleep(1000);
                //}

                // SetColor
                rgbPwmLed.SetColor(Color.Red);
                Console.WriteLine("Red");
                Thread.Sleep(1000);

                rgbPwmLed.SetColor(Color.Green);
                Console.WriteLine("Green");
                Thread.Sleep(1000);

                rgbPwmLed.SetColor(Color.Blue);
                Console.WriteLine("Blue");
                Thread.Sleep(1000);

                // Blink
                rgbPwmLed.StartBlink(Color.Red);
                Console.WriteLine("Blinking Red");
                Thread.Sleep(3000);

                rgbPwmLed.StartBlink(Color.Green);
                Console.WriteLine("Blinking Green");
                Thread.Sleep(3000);

                rgbPwmLed.StartBlink(Color.Blue);
                Console.WriteLine("Blinking Blue");
                Thread.Sleep(3000);

                // Pulse
                rgbPwmLed.StartPulse(Color.Red);
                Console.WriteLine("Pulsing Red");
                Thread.Sleep(3000);

                rgbPwmLed.StartPulse(Color.Green);
                Console.WriteLine("Pulsing Green");
                Thread.Sleep(3000);

                rgbPwmLed.StartPulse(Color.Blue);
                Console.WriteLine("Pulsing Blue");
                Thread.Sleep(3000);
            }
        }
    }
}