using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Leds;

namespace HomeWidget.Hardware
{
    internal interface IHomeWidgetHardware
    {
        IRgbPwmLed Led { get; }

        IGraphicsDisplay Display { get; }

        Htu21d EnvironmentalSensor { get; }

        void Initialize();
    }
}