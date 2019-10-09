using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;

namespace ICs.IOExpanders.MCP23008_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IDigitalOutputPort redLed;
        IDigitalOutputPort greenLed;

        MCP23008 mcp23008;

        public MeadowApp()
        {
            Console.WriteLine("MeadowApp();");
            redLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);
            greenLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);

            redLed.State = true;

            mcp23008 = new MCP23008(Device.CreateI2cBus());

            TestMCP23008();
            Console.WriteLine("End - MeadowApp();");
        }

        public void TestMCP23008()
        {
            Console.WriteLine("TestMCP23008();");

            redLed.State = false;
            greenLed.State = true;

            // create an array of ports
            IDigitalOutputPort[] ports = new IDigitalOutputPort[8];
            for (byte i = 0; i <= 7; i++)
            {
                //ports[i] = mcp23008.CreateOutputPort(mcp23008, (byte)i, false);
            }

            Console.WriteLine("Ports Inititalized.");
            while (true)
            {
                // count from 0 to 7 (8 leds)
                for (int i = 0; i <= 7; i++)
                {
                    // turn on the LED that matches the count
                    for (byte j = 0; j <= 7; j++)
                    {
                        ports[j].State = (i == j);
                    }
                    Console.WriteLine("i: " + i.ToString());
                    Thread.Sleep(250);
                }
            }
        }
    }
}