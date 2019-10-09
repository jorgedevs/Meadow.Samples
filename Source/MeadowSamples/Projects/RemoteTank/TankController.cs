using Meadow.Hardware;

namespace RemoteTank
{
    public class TankController
    {
        const float STOP = 0.75f;
        const float BACKWARD = 0.94f;
        const float FORWARD = 0.57f;

        protected IPwmPort motorPwmLeft;
        protected IPwmPort motorPwmRight;

        public TankController(IPwmPort motorLeftPort, IPwmPort motorRightPort)
        {
            motorPwmLeft = motorLeftPort;
            motorPwmRight = motorRightPort;
        }

        void UpdateMotors(float motorLeftValue, float motorRightValue)
        {
            motorPwmLeft.Stop();
            motorPwmRight.Stop();
            motorPwmLeft.DutyCycle = motorLeftValue;
            motorPwmRight.DutyCycle = motorRightValue;
            motorPwmLeft.Start();
            motorPwmRight.Start();
        }

        public void Stop() { UpdateMotors(STOP, STOP); }

        public void TurnLeft() { UpdateMotors(FORWARD, BACKWARD); }

        public void TurnRight() { UpdateMotors(BACKWARD, FORWARD); }

        public void MoveForward() { UpdateMotors(FORWARD, FORWARD); }

        public void MoveBackward() { UpdateMotors(BACKWARD, BACKWARD); }
    }
}
