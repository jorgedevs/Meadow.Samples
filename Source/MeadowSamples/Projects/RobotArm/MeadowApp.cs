using System.Threading;
using Meadow;
using Meadow.Devices;

namespace RobotArm
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        protected RobotArmController robotArmController;

        public MeadowApp()
        {
            robotArmController = new RobotArmController(
                pwmServoBase: Device.CreatePwmPort(Device.Pins.D05),
                pwmShoulder1: Device.CreatePwmPort(Device.Pins.D06),
                pwmShoulder2: Device.CreatePwmPort(Device.Pins.D07),
                pwmGripper: Device.CreatePwmPort(Device.Pins.D08)
            );

            TestRobotArm();
        }

        public void TestRobotArm()
        {
            int speed = 100;
            int angle = 0;

            while (true)
            {
                while (angle < 180)
                {
                    robotArmController.RotateBase(angle);
                    robotArmController.RotateShoulder(angle);
                    robotArmController.RotateGripper(angle);
                    Thread.Sleep(speed);
                    angle++;
                }

                while (angle > 0)
                {
                    robotArmController.RotateBase(angle);
                    robotArmController.RotateShoulder(angle);
                    robotArmController.RotateGripper(angle);
                    Thread.Sleep(speed);
                    angle--;
                }
            }
        }
    }
}