﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Leds.PwmLed_Sample
{
    // Change F7MicroV2 to F7Micro for V1.x boards
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        List<PwmLed> pwmLeds;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            pwmLeds = new List<PwmLed>
            {
                new PwmLed(Device.CreatePwmPort(Device.Pins.D02), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D03), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D04), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D05), TypicalForwardVoltage.Blue),
                //new PwmLed(Device.CreatePwmPort(Device.Pins.D06), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D07), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D08), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D09), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D10), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D11), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D12), TypicalForwardVoltage.Blue),
                new PwmLed(Device.CreatePwmPort(Device.Pins.D13), TypicalForwardVoltage.Blue)
            };

            TestPwmLeds();
        }

        protected void TestPwmLeds()
        {
            Console.WriteLine("TestPwmLeds...");

            while (true)
            {
                Console.WriteLine("Turning on and off each led for 1 second");
                foreach (var pwmLed in pwmLeds)
                {
                    pwmLed.IsOn = true;
                    Thread.Sleep(100);
                    pwmLed.IsOn = false;
                }

                Console.WriteLine("Blinking the LED for a bit.");
                foreach (var pwmLed in pwmLeds)
                {
                    pwmLed.StartBlink();
                    Thread.Sleep(1000);
                    pwmLed.Stop();
                }

                Console.WriteLine("Pulsing the LED for a bit.");
                foreach (var pwmLed in pwmLeds)
                {
                    pwmLed.StartPulse();
                    Thread.Sleep(1000);
                    pwmLed.Stop();
                }

                Console.WriteLine("Set brightness the LED for a bit.");
                foreach (var pwmLed in pwmLeds)
                {
                    pwmLed.SetBrightness(0.25f);
                    Thread.Sleep(250);
                    pwmLed.SetBrightness(0.5f);
                    Thread.Sleep(250);
                    pwmLed.SetBrightness(0.75f);
                    Thread.Sleep(250);
                    pwmLed.SetBrightness(1.0f);
                    Thread.Sleep(250);
                    pwmLed.Stop();
                }
            }
        }
    }
}