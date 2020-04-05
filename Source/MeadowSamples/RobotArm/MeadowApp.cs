using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;

namespace RobotArm
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    { 
        PushButton gripOpen;
        PushButton gripClose;
        PushButton baseRotateLeft;
        PushButton baseRotateRight;
        PushButton verticalMoveUp;
        PushButton verticalMoveDown;
        PushButton horizontalMoveForward;
        PushButton horizontalMoveBackward;

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

            //baseRotateLeft = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D08, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //baseRotateLeft.Clicked += BaseRotateLeftClicked;
            //baseRotateRight = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D09, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //baseRotateRight.Clicked += BaseRotateRightClicked;
            
            //gripOpen = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D10, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //gripOpen.Clicked += GripOpenClicked;
            //gripClose = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D11, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //gripClose.Clicked += GripCloseClicked;

            //verticalMoveUp = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D06, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //verticalMoveUp.Clicked += VerticalMoveUpClicked;
            //verticalMoveDown = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D07, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //verticalMoveDown.Clicked += VerticalMoveDownClicked;

            //horizontalMoveForward = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //horizontalMoveForward.Clicked += HorizontalMoveForwardClicked;
            //horizontalMoveBackward = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D13, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            //horizontalMoveBackward.Clicked += HorizontalMoveBackwardClicked;

            Console.WriteLine("Done");

            robotArmController.Test();
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

        #region Vertical
        void VerticalMoveUpClicked(object sender, EventArgs e)
        {
            robotArmController.MoveVerticalUp();
        }
        void VerticalMoveDownClicked(object sender, EventArgs e)
        {
            robotArmController.MoveVerticalDown();
        }
        #endregion

        #region Horizontal
        void HorizontalMoveForwardClicked(object sender, EventArgs e)
        {
            robotArmController.MoveHorizontalForward();
        }
        void HorizontalMoveBackwardClicked(object sender, EventArgs e)
        {
            robotArmController.MoveHorizontalBackward();
        }
        #endregion
    }
}