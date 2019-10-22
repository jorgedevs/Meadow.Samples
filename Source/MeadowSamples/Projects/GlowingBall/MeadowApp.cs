using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Servos;

namespace GlowingBall
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Servo servo;
        PwmLed pwmLedRed;
        PwmLed pwmLedBlue;


        public MeadowApp()
        {
            Initialize();
            BlinkLeds();
        }

        public void Initialize()
        {
            Console.WriteLine("Creating Outputs...");
            pwmLedRed = new PwmLed(Device.CreatePwmPort(Device.Pins.D06), 3.3f);
            pwmLedBlue = new PwmLed(Device.CreatePwmPort(Device.Pins.D05), 3.3f);            
            servo = new Servo(Device.CreatePwmPort(Device.Pins.D04), NamedServoConfigs.Ideal180Servo);
        }

        public void BlinkLeds()
        {
            pwmLedRed.StartBlink();
            Thread.Sleep(200);
            pwmLedBlue.StartBlink();

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
                Thread.Sleep(500);
            }
        }
    }
}