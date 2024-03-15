using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace HomeWidget.Hardware
{
    internal interface IHomeWidgetHardware
    {
        IRgbPwmLed Led { get; }

        IPixelDisplay Display { get; }

        Htu21d EnvironmentalSensor { get; }

        void Initialize();
    }
}