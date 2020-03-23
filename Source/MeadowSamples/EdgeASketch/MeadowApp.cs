using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;

namespace EdgeASketch
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int X = 0, Y = 0;
        //RotaryEncoderWithButton RotaryX, RotaryY;

        PushButton xUp, xDown, yUp, yDown;

        public MeadowApp()
        {
            Console.Write("Initializing...");

            //RotaryX = new RotaryEncoderWithButton(
            //    device: Device, 
            //    aPhasePin: Device.Pins.D14, 
            //    bPhasePin: Device.Pins.D13, 
            //    buttonPin: Device.Pins.D12
            //);
            //RotaryX.Rotated += (s, e) =>
            //{
            //    if (e.Direction == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
            //        X++; 
            //    else
            //        X--;
            //    Console.WriteLine("X = {0}", X);
            //};

            //RotaryY = new RotaryEncoderWithButton(
            //    device: Device,
            //    aPhasePin: Device.Pins.D02,
            //    bPhasePin: Device.Pins.D03,
            //    buttonPin: Device.Pins.D04
            //);
            //RotaryY.Rotated += (s, e) =>
            //{
            //    if (e.Direction == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
            //        Y++;
            //    else
            //        Y--;
            //    Console.WriteLine("Y = {0}", Y);
            //};

            xUp = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            xUp.Clicked += (s, e) => { X++; Console.WriteLine("X = {0}", X); };

            xDown = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D14, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            xDown.Clicked += (s, e) => { X--; Console.WriteLine("X = {0}", X); };

            yUp = new PushButton(Device, Device.Pins.D02);
            yUp.Clicked += (s, e) => { Y++; Console.WriteLine("Y = {0}", Y); };

            yDown = new PushButton(Device, Device.Pins.D05);
            yDown.Clicked += (s, e) => { Y--; Console.WriteLine("Y = {0}", Y); };

            Console.WriteLine("done");
        }
    }
}