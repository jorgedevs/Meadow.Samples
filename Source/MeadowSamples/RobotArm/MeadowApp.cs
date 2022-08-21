using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;

namespace RobotArm
{
    public class MeadowApp : App<F7FeatherV1>
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

        public override Task Initialize()
        {
            Console.Write("Initializing...");

            robotArmController = new RobotArmController
            (
                pwmBase: Device.CreatePwmPort(Device.Pins.D04, new Meadow.Units.Frequency(500)),
                pwmGrip: Device.CreatePwmPort(Device.Pins.D03, new Meadow.Units.Frequency(500)),
                pwmVertical: Device.CreatePwmPort(Device.Pins.D02, new Meadow.Units.Frequency(500)),
                pwmHorizontal: Device.CreatePwmPort(Device.Pins.D05, new Meadow.Units.Frequency(500))
            );

            baseRotateLeft = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D08, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            baseRotateLeft.Clicked += BaseRotateLeftClicked;
            baseRotateRight = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D09, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            baseRotateRight.Clicked += BaseRotateRightClicked;

            gripOpen = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D10, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            gripOpen.Clicked += GripOpenClicked;
            gripClose = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D11, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            gripClose.Clicked += GripCloseClicked;

            verticalMoveUp = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D06, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            verticalMoveUp.Clicked += VerticalMoveUpClicked;
            verticalMoveDown = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D07, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            verticalMoveDown.Clicked += VerticalMoveDownClicked;

            horizontalMoveForward = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            horizontalMoveForward.Clicked += HorizontalMoveForwardClicked;
            horizontalMoveBackward = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D13, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            horizontalMoveBackward.Clicked += HorizontalMoveBackwardClicked;

            Console.WriteLine("Done");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            robotArmController.Test();

            return Task.CompletedTask;
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