using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;

namespace RotationDetector
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led up;
        Led down; 
        Led left;
        Led right;
        GY521 gY521;

        public MeadowApp()
        {
            up = new Led(Device.CreateDigitalOutputPort(Device.Pins.D15));
            down = new Led(Device.CreateDigitalOutputPort(Device.Pins.D12));
            left = new Led(Device.CreateDigitalOutputPort(Device.Pins.D14));
            right = new Led(Device.CreateDigitalOutputPort(Device.Pins.D13));
            gY521 = new GY521(Device.CreateI2cBus());

            TestGY521();
        }

        void TestGY521()
        {
            gY521.Wake();

            while (true)
            {
                gY521.Refresh();
                Thread.Sleep(100);

                if (gY521.AccelerationY > 1000 && gY521.AccelerationY < 16000)
                    up.IsOn = true;
                else
                    up.IsOn = false;

                if (gY521.AccelerationY > 49000 && gY521.AccelerationY < 64535)
                    down.IsOn = true;
                else
                    down.IsOn = false;

                if (gY521.AccelerationX > 1000 && gY521.AccelerationX < 16000)
                    right.IsOn = true;
                else
                    right.IsOn = false;

                if (gY521.AccelerationX > 49000 && gY521.AccelerationX < 64535)
                    left.IsOn = true;
                else
                    left.IsOn = false;
            }
        }
    }
}