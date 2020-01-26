using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Servos;

namespace ServoHost
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Servo servo;
        RgbPwmLed led;

        public MeadowApp()
        {
            var servoConfig = new ServoConfig(
                minimumAngle: 0,
                maximumAngle: 180,
                minimumPulseDuration: 700,
                maximumPulseDuration: 3000,
                frequency: 50);
            servo = new Servo(Device.CreatePwmPort(Device.Pins.D04), servoConfig);

            led = new RgbPwmLed(Device,
                       Device.Pins.OnboardLedRed,
                       Device.Pins.OnboardLedGreen,
                       Device.Pins.OnboardLedBlue);
            led.SetColor(Color.Red);

            PulseRgbPwmLed();
        }

        void PulseRgbPwmLed()
        {
            led.SetColor(Color.Green);
            servo.RotateTo(0);

            while (true)
            {
                Console.WriteLine("Start Cycling...");
                led.SetColor(Color.Blue);
                Cycle(3);

                Console.WriteLine("Stop Cycling...");
                led.SetColor(Color.Blue);
                Thread.Sleep(1000);
                led.SetColor(Color.Green);
                Thread.Sleep(1000);

                Console.WriteLine("Rotate to 90 degrees...");
                led.SetColor(Color.Blue);
                servo.RotateTo(90);
                Thread.Sleep(1000);
                led.SetColor(Color.Green);
                Thread.Sleep(1000);

                Console.WriteLine("Rotate to 0 degrees...");
                led.SetColor(Color.Blue);
                servo.RotateTo(0);
                Thread.Sleep(1000);
                led.SetColor(Color.Green);
                Thread.Sleep(1000);
            }
        }

        void Cycle(int times) 
        { 
            for(int i=0; i<times; i++) 
            {
                servo.RotateTo(180);
                Thread.Sleep(1000);
                led.SetColor(Color.Green);
                servo.RotateTo(0);
                Thread.Sleep(1000);
            }
        }
    }
}