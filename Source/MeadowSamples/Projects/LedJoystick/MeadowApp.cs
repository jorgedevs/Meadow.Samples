using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LedJoystick
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        //PwmLed up;
        PwmLed left;
        PwmLed right;
        //PwmLed down;

        IAnalogInputPort analogX;
        //IAnalogInputPort analogY;

        public MeadowApp()
        {
            Console.WriteLine($"MeadowApp...");

            //up = new PwmLed(Device.CreatePwmPort(Device.Pins.D07), TypicalForwardVoltage.Red);
            left = new PwmLed(Device.CreatePwmPort(Device.Pins.D02), TypicalForwardVoltage.Red);
            right = new PwmLed(Device.CreatePwmPort(Device.Pins.D03), TypicalForwardVoltage.Red);
            //down = new PwmLed(Device.CreatePwmPort(Device.Pins.D04), TypicalForwardVoltage.Red);

            analogX = Device.CreateAnalogInputPort(Device.Pins.A01);
            //analogY = Device.CreateAnalogInputPort(Device.Pins.A00);

            PulseRgbPwmLed();
        }

        async Task PulseRgbPwmLed()
        {
            Console.WriteLine($"PulseRgbPwmLed...");

            while (true)
            {
                //up.SetBrightness(0);
                //down.SetBrightness(0);
                //left.SetBrightness(0);
                //right.SetBrightness(0);

                float x = await analogX.Read();
                //float y = await analogY.Read();

                float brightnessLeftX = x / 1.58f;

                if (brightnessLeftX > 1f)
                    brightnessLeftX = 1f;                
                left.SetBrightness(1f - brightnessLeftX);

                //if (x < 1.57)
                //    left.SetBrightness(1);
                //else if (x > 1.59)
                //    right.SetBrightness(1);

                //if (y < 1.57)
                //    up.SetBrightness(1);
                //else if (y > 1.59)
                //    down.SetBrightness(1);

                Console.WriteLine($"X: {x}");
            }
        }
    }
}