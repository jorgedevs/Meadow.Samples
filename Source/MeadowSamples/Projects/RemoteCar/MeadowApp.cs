using System.Threading;
using Meadow;
using Meadow.Devices;

namespace RemoteCar
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        HBridgeMotor motorLeft;
        HBridgeMotor motorRight;

        public MeadowApp()
        {
            ConfigurePorts();
            TestMotors();
        }

        public void ConfigurePorts()
        {
            motorRight = new HBridgeMotor
            (
                a1Pin: Device.CreatePwmPort(Device.Pins.D02),
                a2Pin: Device.CreatePwmPort(Device.Pins.D03),
                enablePin: Device.CreateDigitalOutputPort(Device.Pins.D04)
            );

            motorLeft = new HBridgeMotor
            (
                a1Pin: Device.CreatePwmPort(Device.Pins.D07),
                a2Pin: Device.CreatePwmPort(Device.Pins.D08),
                enablePin: Device.CreateDigitalOutputPort(Device.Pins.D09)
            );
        }

        public void TestMotors()
        {
            while (true)
            {
                motorLeft.Speed = 1f;
                motorRight.Speed = 1f;
                Thread.Sleep(1000);

                motorLeft.Speed = 0f;
                motorRight.Speed = 0f;
                Thread.Sleep(500);

                motorLeft.Speed = -1f;
                motorRight.Speed = -1f;
                Thread.Sleep(1000);
            }
        }
    }
}