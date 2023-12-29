using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Leds;

namespace HomeWidget.Hardware
{
    internal interface IHomeWidgetHardware
    {
        IRgbPwmLed Led { get; }

        IGraphicsDisplay Display { get; }

        void Initialize();
    }
}