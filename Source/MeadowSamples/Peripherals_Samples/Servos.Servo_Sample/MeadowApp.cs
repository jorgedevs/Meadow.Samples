using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Servos;

namespace Servos.Servo_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Servo servo;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D04), NamedServoConfigs.Ideal180Servo);

            TestServo();
        }

        void TestServo()
        {
            Console.WriteLine("TestServo...");

            while (true)
            {
                if (servo.Angle <= servo.Config.MinimumAngle)
                {
                    Console.WriteLine($"Rotating to {servo.Config.MaximumAngle}");
                    servo.RotateTo(servo.Config.MaximumAngle);
                }
                else
                {
                    Console.WriteLine($"Rotating to {servo.Config.MinimumAngle}");
                    servo.RotateTo(servo.Config.MinimumAngle);
                }
                Thread.Sleep(5000);
            }
        }
    }
}
