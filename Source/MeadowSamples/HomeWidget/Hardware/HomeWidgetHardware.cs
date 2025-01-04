using Meadow.Foundation.Displays;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace HomeWidget.Hardware;

public class HomeWidgetHardware : IHomeWidgetHardware
{
    public IRgbPwmLed Led { get; private set; }

    public IPixelDisplay Display { get; private set; }

    public Htu21d EnvironmentalSensor { get; private set; }

    public void Initialize()
    {
        Led = new RgbPwmLed(
            redPwmPin: MeadowApp.Device.Pins.OnboardLedRed,
            greenPwmPin: MeadowApp.Device.Pins.OnboardLedGreen,
            bluePwmPin: MeadowApp.Device.Pins.OnboardLedBlue,
            commonType: CommonType.CommonAnode);

        Display = new Epd4in2bV2(
            spiBus: MeadowApp.Device.CreateSpiBus(),
            chipSelectPin: MeadowApp.Device.Pins.D12,
            dcPin: MeadowApp.Device.Pins.D13,
            resetPin: MeadowApp.Device.Pins.D14,
            busyPin: MeadowApp.Device.Pins.D15);

        EnvironmentalSensor = new Htu21d(MeadowApp.Device.CreateI2cBus());
    }
}