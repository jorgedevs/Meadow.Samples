using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;

namespace HomeWidget.Hardware
{
    internal class HomeWidgetHardware : IHomeWidgetHardware
    {
        public IRgbPwmLed Led { get; private set; }

        public IGraphicsDisplay Display { get; private set; }

        public void Initialize()
        {
            Led = new RgbPwmLed(
                redPwmPin: MeadowApp.Device.Pins.OnboardLedRed,
                greenPwmPin: MeadowApp.Device.Pins.OnboardLedGreen,
                bluePwmPin: MeadowApp.Device.Pins.OnboardLedBlue,
                commonType: CommonType.CommonAnode);

            Display = new Epd4in2bV2(
                spiBus: MeadowApp.Device.CreateSpiBus(),
                chipSelectPin: MeadowApp.Device.Pins.D03,
                dcPin: MeadowApp.Device.Pins.D02,
                resetPin: MeadowApp.Device.Pins.D01,
                busyPin: MeadowApp.Device.Pins.D00);
        }
    }
}