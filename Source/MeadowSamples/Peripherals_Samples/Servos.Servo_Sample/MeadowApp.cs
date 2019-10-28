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

            ServoConfig config = new ServoConfig(
                minimumAngle: 0, 
                maximumAngle: 180,
                minimumPulseDuration: 700,
                maximumPulseDuration: 3000,
                frequency: 50);

            servo = new Servo(Device.CreatePwmPort(Device.Pins.D05), config);

            TestServo();
        }

        void TestServo()
        {
            Console.WriteLine("TestServo...");

            int angle = 0;
            servo.RotateTo(angle);
            Thread.Sleep(3000);

            while (true)
            {
                servo.RotateTo(servo.Config.MaximumAngle);
                Thread.Sleep(3000);

                servo.RotateTo(servo.Config.MinimumAngle);
                Thread.Sleep(3000);
            }
        }
    }
}