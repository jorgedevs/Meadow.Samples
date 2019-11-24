using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.RTCs;

namespace ChristmasCountdown
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        //DS1307 rtc;
        DateTime rtc;
        CharacterDisplay display;

        public MeadowApp()
        {
            //rtc = new DS1307(Device.CreateI2cBus());            

            display = new CharacterDisplay
            (
                device: Device,
                pinRS: Device.Pins.D15,
                pinE: Device.Pins.D14,
                pinD4: Device.Pins.D13,
                pinD5: Device.Pins.D12,
                pinD6: Device.Pins.D11,
                pinD7: Device.Pins.D10
            );

            StartCountdown();
        }

        DateTime GetTime() 
        {
            return DateTime.Now;
        }

        void StartCountdown() 
        {
            DateTime ChristmasDate = new DateTime(GetTime().Year, 12, 25);
            display.WriteLine("Current Date:", 0);
            display.WriteLine(GetTime().Month + "/" + GetTime().Day + "/" + GetTime().Year, 1);
            display.WriteLine("Christmas Countdown:", 2);

            while (true)
            {
                var date = ChristmasDate.Subtract(GetTime());
                UpdateCountdown(date);
                Thread.Sleep(60000);
            }
        }

        void UpdateCountdown(TimeSpan date)
        {
            display.ClearLine(3);
            display.WriteLine(date.Days + "d" + date.Hours + "h" + date.Minutes + "m to go!", 3);
        }
    }
}