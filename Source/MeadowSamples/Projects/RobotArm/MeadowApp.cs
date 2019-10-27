using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Servos;
using Meadow.Hardware;

namespace RobotArm
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PushButton gripOpen;
        PushButton gripClose;
        
        PushButton baseRotateLeft;
        PushButton baseRotateRight;

        RobotArmController robotArmController;

        public MeadowApp()
        {
            Console.Write("Initializing...");

            robotArmController = new RobotArmController
            (
                pwmBase: Device.CreatePwmPort(Device.Pins.D04),
                pwmGrip: Device.CreatePwmPort(Device.Pins.D03),
                pwmVertical: Device.CreatePwmPort(Device.Pins.D02),
                pwmHorizontal: Device.CreatePwmPort(Device.Pins.D05)
            );

            baseRotateLeft = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D08, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            baseRotateLeft.Clicked += BaseRotateLeftClicked;            
            baseRotateRight = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D09, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            baseRotateRight.Clicked += BaseRotateRightClicked;            
            
            gripOpen = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D10, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            gripOpen.Clicked += GripOpenClicked;
            gripClose = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D11, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            gripClose.Clicked += GripCloseClicked;

            Console.WriteLine("Done");
        }

        #region Base
        void BaseRotateLeftClicked(object sender, EventArgs e)
        {
            robotArmController.MoveBaseLeft();
        }
        void BaseRotateRightClicked(object sender, EventArgs e)
        {
            robotArmController.MoveBaseRight();
        }
        //void BaseRotateRightPressStarted(object sender, EventArgs e)
        //{
        //    Console.WriteLine("isBusy = false");
        //    isBusy = false;
        //}
        //void BaseRotateRightPressEnded(object sender, EventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        if (isBusy)
        //            return;
        //        isBusy = true;
        //        Console.WriteLine("Rotating right...");
        //        while (isBusy)
        //        {
        //            if (baseServo.Angle > baseServo.Config.MinimumAngle)
        //            {
        //                baseAngle--;
        //                baseServo.RotateTo(baseAngle);
        //                Thread.Sleep(50);
        //            }
        //        }
        //    });
        //}
        //void BaseRotateLeftPressStarted(object sender, EventArgs e)
        //{
        //    Console.WriteLine("isBusy = false");
        //    isBusy = false;
        //}
        //void BaseRotateLeftPressEnded(object sender, EventArgs e)
        //{
        //    Task.Run(() => 
        //    {
        //        if (isBusy)
        //            return;
        //        isBusy = true;
        //        Console.WriteLine("Rotating Left...");
        //        while (isBusy)
        //        {
        //            if (baseServo.Angle < baseServo.Config.MaximumAngle)
        //            {
        //                baseAngle++;
        //                baseServo.RotateTo(baseAngle);
        //                Thread.Sleep(50);
        //            }
        //        }
        //    });
        //}
        #endregion

        #region Grip
        void GripOpenClicked(object sender, EventArgs e)
        {            
            robotArmController.MoveGrip(RobotArmController.GRIP_OPEN);
        }
        void GripCloseClicked(object sender, EventArgs e)
        {
            robotArmController.MoveGrip(RobotArmController.GRIP_CLOSE);
        }
        #endregion
    }
}