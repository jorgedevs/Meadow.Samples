using Meadow.Foundation.Servos;
using Meadow.Hardware;
using System;
using System.Threading;

namespace RobotArm
{
    public class RobotArmController
    {
        public const int GRIP_OPEN = 70;
        public const int GRIP_CLOSE = 140;
        public const int BASE_LEFT = 0;
        public const int BASE_RIGHT = 180;


        int _baseAngle;
        Servo _base;

        int _gripAngle;
        Servo _grip;

        Servo _vertical;

        Servo _horizontal;

        public RobotArmController(IPwmPort pwmBase, IPwmPort pwmGrip, IPwmPort pwmVertical, IPwmPort pwmHorizontal)
        {
            var config = new ServoConfig(
                minimumAngle: 0,
                maximumAngle: 180,
                minimumPulseDuration: 700,
                maximumPulseDuration: 3000,
                frequency: 50);

            _base = new Servo(pwmBase, config);
            _grip = new Servo(pwmGrip, config);
            _vertical = new Servo(pwmVertical, config);
            _horizontal = new Servo(pwmHorizontal, config);

            Initialize();
        }

        public void Initialize() 
        {
            _base.RotateTo(90);
            _grip.RotateTo(70);
            //_vertical.RotateTo(90);
            //_horizontal.RotateTo(90);
        }

        public void MoveBaseLeft()
        {
            Console.WriteLine($"MoveBaseLeft...{_baseAngle + 10}");
            if (_baseAngle < _base.Config.MaximumAngle)
            {
                for (int i = 0; i < 10; i++)
                {
                    _baseAngle++;
                    _base.RotateTo(_baseAngle);
                    Thread.Sleep(500);
                }
            }
        }
        public void MoveBaseRight()
        {
            Console.WriteLine($"MoveBaseRight...{_baseAngle - 10}");
            if (_baseAngle > _base.Config.MinimumAngle)
            {
                for (int i = 0; i < 10; i++)
                {
                    _baseAngle--;
                    _base.RotateTo(_baseAngle);
                    Thread.Sleep(500);
                }
            }
        }

        public void MoveGrip(int angle)
        {
            Console.WriteLine($"MoveGrip...{angle}");
            if (angle != _gripAngle)
                _grip.RotateTo(angle);
            _gripAngle = angle;
        }

        public void MoveVertical(int angle)
        {
            Console.WriteLine("MoveVertical...");
            _vertical.RotateTo(angle);
        }

        public void MoveHorizontal(int angle)
        {
            Console.WriteLine("MoveHorizontal...");
            _horizontal.RotateTo(angle);
        }
    }
}