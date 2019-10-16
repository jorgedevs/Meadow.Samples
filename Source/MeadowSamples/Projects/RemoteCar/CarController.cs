using Meadow.Foundation.Motors;

namespace RemoteCar
{
    public class CarController
    {
        protected HBridgeMotor motorLeft;
        protected HBridgeMotor motorRight;

        public CarController(HBridgeMotor motorLeft, HBridgeMotor motorRight)
        {
            this.motorLeft = motorLeft;
            this.motorRight = motorRight;
        }

        public void Stop()
        {
            motorLeft.Speed = 0f;
            motorRight.Speed = 0f;
        }

        public void TurnLeft()
        {
            motorLeft.Speed = 1f;
            motorRight.Speed = -1f;
        }

        public void TurnRight()
        {
            motorLeft.Speed = -1f;
            motorRight.Speed = 1f;
        }

        public void MoveForward()
        {
            motorLeft.Speed = -1f;
            motorRight.Speed = -1f;
        }

        public void MoveBackward()
        {
            motorLeft.Speed = 1f;
            motorRight.Speed = 1f;
        }
    }
}