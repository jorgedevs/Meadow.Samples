using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace LedDice
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PwmLed[] leds;

        public MeadowApp()
        {
            leds = new PwmLed[7];
            leds[0] = new PwmLed(Device.CreatePwmPort(Device.Pins.D06), TypicalForwardVoltage.Red);
            leds[1] = new PwmLed(Device.CreatePwmPort(Device.Pins.D07), TypicalForwardVoltage.Red);
            leds[2] = new PwmLed(Device.CreatePwmPort(Device.Pins.D08), TypicalForwardVoltage.Red);
            leds[3] = new PwmLed(Device.CreatePwmPort(Device.Pins.D09), TypicalForwardVoltage.Red);
            leds[4] = new PwmLed(Device.CreatePwmPort(Device.Pins.D10), TypicalForwardVoltage.Red);
            leds[5] = new PwmLed(Device.CreatePwmPort(Device.Pins.D11), TypicalForwardVoltage.Red);
            leds[6] = new PwmLed(Device.CreatePwmPort(Device.Pins.D12), TypicalForwardVoltage.Red);
            
            BlinkLeds();
        }

        void ShowNumber(int number)
        {
            foreach (var led in leds)
                led.IsOn = false;
            
            switch(number)
            {
                case 1:
                    leds[3].IsOn = true;
                    break;
                case 2:
                    leds[1].IsOn = true;
                    leds[5].IsOn = true;
                    break;
                case 3:
                    leds[1].IsOn = true;
                    leds[3].IsOn = true;
                    leds[5].IsOn = true;
                    break;
                case 4:
                    leds[0].IsOn = true;
                    leds[1].IsOn = true;
                    leds[5].IsOn = true;
                    leds[6].IsOn = true;
                    break;
                case 5:
                    leds[0].IsOn = true;
                    leds[1].IsOn = true;
                    leds[5].IsOn = true;
                    leds[6].IsOn = true;
                    leds[3].IsOn = true;
                    break;
                case 6:
                    leds[0].IsOn = true;
                    leds[1].IsOn = true;
                    leds[2].IsOn = true;
                    leds[4].IsOn = true;
                    leds[5].IsOn = true;
                    leds[6].IsOn = true;
                    break;
            }
        }

        public void BlinkLeds()
        {
            while (true)
            {
                for(int i=1; i<7; i++)
                {
                    ShowNumber(i);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
