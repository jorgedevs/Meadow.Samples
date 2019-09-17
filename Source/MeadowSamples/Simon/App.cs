using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Simon
{
    public class App : App<F7Micro, App>
    {
        Led ledRed;
        Led ledGreen;
        Led ledBlue;
        Led ledYellow;

        public App()
        {
            ledRed    = new Led(Device.CreateDigitalOutputPort(Device.Pins.D10));
            ledGreen  = new Led(Device.CreateDigitalOutputPort(Device.Pins.D09));
            ledBlue   = new Led(Device.CreateDigitalOutputPort(Device.Pins.D08));
            ledYellow = new Led(Device.CreateDigitalOutputPort(Device.Pins.D07));

            TestLEDs();
        }

        protected void TestLEDs()
        {
            bool state = true;

            while(true)
            {
                ledRed.IsOn = state;
                ledGreen.IsOn = state;
                ledBlue.IsOn = state;
                ledYellow.IsOn = state;

                state = !state;

                Thread.Sleep(1000);
            }
        }
    }
}