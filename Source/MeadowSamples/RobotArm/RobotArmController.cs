using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;

namespace RobotArm
{
    public class RobotArmController
    {
        public const int GRIP_OPEN = 70;
        public const int GRIP_CLOSE = 140;

        int baseAngle;
        Servo _base;

        int _gripAngle;
        Servo _grip;

        int _verticalAngle;
        Servo _vertical;

        int _horizontalAngle;
        Servo _horizontal;

        public RobotArmController(IPwmPort pwmBase, IPwmPort pwmGrip, IPwmPort pwmVertical, IPwmPort pwmHorizontal)
        {
            _base = new Servo(pwmBase, NamedServoConfigs.SG90);
            _grip = new Servo(pwmGrip, NamedServoConfigs.SG90);
            _vertical = new Servo(pwmVertical, NamedServoConfigs.SG90);
            _horizontal = new Servo(pwmHorizontal, NamedServoConfigs.SG90);

            Initialize();
        }

        public void Initialize() 
        {
            _base.RotateTo(new Angle(0, Angle.UnitType.Degrees));
            _grip.RotateTo(new Angle(0, Angle.UnitType.Degrees));

            _verticalAngle = 0;
            _vertical.RotateTo(new Angle(_verticalAngle, Angle.UnitType.Degrees));

            //_horizontal.RotateTo(90);
        }

        public void MoveBaseLeft()
        {
            Console.WriteLine($"MoveBaseLeft...{baseAngle + 10}");
            if (baseAngle < _base.Config.MaximumAngle.Degrees)
            {
                for (int i = 0; i < 10; i++)
                {
                    baseAngle++;
                    _base.RotateTo(new Angle(baseAngle, Angle.UnitType.Degrees));
                    Thread.Sleep(500);
                }
            }
        }
        public void MoveBaseRight()
        {
            Console.WriteLine($"MoveBaseRight...{baseAngle - 10}");
            if (baseAngle > _base.Config.MinimumAngle.Degrees)
            {
                for (int i = 0; i < 10; i++)
                {
                    baseAngle--;
                    _base.RotateTo(new Angle(baseAngle, Angle.UnitType.Degrees));
                    Thread.Sleep(500);
                }
            }
        }

        public void MoveGrip(int angle)
        {
            Console.WriteLine($"MoveGrip...{angle}");
            if (angle != _gripAngle)
                _grip.RotateTo(new Angle(angle, Angle.UnitType.Degrees));
            _gripAngle = angle;
        }

        public void MoveVerticalUp()
        {
            Console.WriteLine($"MoveVerticalUp...{_verticalAngle + 10}");
            if (_verticalAngle < _vertical.Config.MaximumAngle.Degrees)
            {
                for (int i = 0; i < 10; i++)
                {
                    _verticalAngle++;
                    _vertical.RotateTo(new Angle(_verticalAngle, Angle.UnitType.Degrees));
                    Thread.Sleep(500);
                }
            }
        }
        public void MoveVerticalDown()
        {
            Console.WriteLine($"MoveVerticalDown...{_verticalAngle - 10}");
            if (_verticalAngle > _vertical.Config.MinimumAngle.Degrees)
            {
                for (int i = 0; i < 10; i++)
                {
                    _verticalAngle--;
                    _vertical.RotateTo(new Angle(_verticalAngle, Angle.UnitType.Degrees));
                    Thread.Sleep(500);
                }
            }
        }

        public void MoveHorizontalForward()
        {
            Console.WriteLine($"MoveHorizontalForward...{_horizontalAngle + 10}");
            if (_horizontalAngle < _horizontal.Config.MaximumAngle.Degrees)
            {
                for (int i = 0; i < 10; i++)
                {
                    _horizontalAngle++;
                    _horizontal.RotateTo(new Angle(_horizontalAngle, Angle.UnitType.Degrees));
                    Thread.Sleep(500);
                }
            }
        }
        public void MoveHorizontalBackward()
        {
            Console.WriteLine($"MoveHorizontalBackward...{_horizontalAngle - 10}");
            if (_horizontalAngle > _horizontal.Config.MinimumAngle.Degrees)
            {
                for (int i = 0; i < 10; i++)
                {
                    _horizontalAngle--;
                    _horizontal.RotateTo(new Angle(_horizontalAngle, Angle.UnitType.Degrees));
                    Thread.Sleep(500);
                }
            }
        }

        public void Test() 
        {
            Console.WriteLine("Test...");
            _grip.RotateTo(new Angle(0, Angle.UnitType.Degrees));

            bool isOpen = false;

            while (true) 
            {
                //_grip.RotateTo(100); //Close
                //Thread.Sleep(2000);

                //_grip.RotateTo(0); // Open
                //Thread.Sleep(2000);

                for (int i = 0; i <= 180; i++)
                {
                    _base.RotateTo(new Angle(i, Angle.UnitType.Degrees));
                    Thread.Sleep(20);
                }

                Thread.Sleep(2000);

                for (int i = 180; i >= 0; i--)
                {
                    _base.RotateTo(new Angle(i, Angle.UnitType.Degrees));
                    Thread.Sleep(20);
                }

                Thread.Sleep(2000);

                if (isOpen)
                    _grip.RotateTo(new Angle(100, Angle.UnitType.Degrees));
                else
                    _grip.RotateTo(new Angle(0, Angle.UnitType.Degrees));

                isOpen = !isOpen;
            }
        }
    }
}