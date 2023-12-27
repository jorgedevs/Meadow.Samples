using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System;
using System.Threading.Tasks;

namespace HomeWidget;

public class MeadowApp : App<F7FeatherV2>
{
    RgbPwmLed onboardLed;
    DisplayController displayController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue,
            CommonType.CommonAnode);

        var display = new Epd4in2bV2(
            spiBus: Device.CreateSpiBus(),
            chipSelectPin: Device.Pins.D03,
            dcPin: Device.Pins.D02,
            resetPin: Device.Pins.D01,
            busyPin: Device.Pins.D00);

        displayController = new DisplayController(display);

        return base.Initialize();
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        //_ = displayController.Run();

        return base.Run();
    }
}