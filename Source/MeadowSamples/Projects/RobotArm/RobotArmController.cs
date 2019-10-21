using Meadow.Foundation.Servos;
using Meadow.Hardware;

namespace RobotArm
{
    public class RobotArmController
    {
        protected Servo servoBase;
        protected Servo shoulder1;
        protected Servo shoulder2;
        protected Servo gripper;

        public RobotArmController(IPwmPort pwmServoBase, IPwmPort pwmShoulder1, IPwmPort pwmShoulder2, IPwmPort pwmGripper)
        {
            servoBase = new Servo(pwmServoBase, NamedServoConfigs.Ideal180Servo);
            shoulder1 = new Servo(pwmShoulder1, NamedServoConfigs.Ideal180Servo);
            shoulder2 = new Servo(pwmShoulder2, NamedServoConfigs.Ideal180Servo);
            gripper = new Servo(pwmGripper, NamedServoConfigs.Ideal180Servo);
        }

        public void RotateBase(int angle) 
        {
            servoBase.RotateTo(angle);
        }

        public void RotateShoulder(int angle)
        {
            shoulder1.RotateTo(angle);
            shoulder2.RotateTo(angle);
        }

        public void RotateGripper(int angle)
        {
            gripper.RotateTo(angle);
        }
    }
}