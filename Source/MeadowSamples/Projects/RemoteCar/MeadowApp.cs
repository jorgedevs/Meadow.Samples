using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Motors;

namespace RemoteCar
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        protected CarController carController;

        public MeadowApp()
        {
            Initialize();
            TestCar();
        }

        protected void Initialize() 
        {
            var motorLeft = new HBridgeMotor
            (
                a1Pin: Device.CreatePwmPort(Device.Pins.D07),
                a2Pin: Device.CreatePwmPort(Device.Pins.D08),
                enablePin: Device.CreateDigitalOutputPort(Device.Pins.D09)
            );
            var motorRight = new HBridgeMotor
            (
                a1Pin: Device.CreatePwmPort(Device.Pins.D02),
                a2Pin: Device.CreatePwmPort(Device.Pins.D03),
                enablePin: Device.CreateDigitalOutputPort(Device.Pins.D04)
            );

            carController = new CarController(motorLeft, motorRight);
        }

        protected void TestCar()
        {
            while (true)
            {
                carController.MoveForward();
                Thread.Sleep(1000);

                carController.Stop();
                Thread.Sleep(500);

                carController.MoveBackward();
                Thread.Sleep(1000);
            }
        }
    }
}