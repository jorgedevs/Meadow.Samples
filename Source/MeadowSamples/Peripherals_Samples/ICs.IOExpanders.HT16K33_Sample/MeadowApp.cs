using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;

namespace ICs.IOExpanders.HT16K33_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Ht16K33 ht16k33;

        public MeadowApp()
        {
            Console.WriteLine("Create I2C bus");
            var i2cBus = Device.CreateI2cBus();

            Console.WriteLine("Create HT16K33");
            ht16k33 = new Ht16K33(i2cBus);

            int index = 0;
            bool on = true;

            Console.WriteLine("Cycle HT16K33 outputs");

            // write your code here
            while (true)
            {
                ht16k33.ToggleLed((byte)index, on);
                ht16k33.UpdateDisplay();
                index++;

                if (index >= 128)
                {
                    index = 0;
                    on = !on;
                }

                Thread.Sleep(100);
            }
        }
    }
}
