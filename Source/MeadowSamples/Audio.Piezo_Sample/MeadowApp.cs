using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;

namespace Audio.Piezo_Sample
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        readonly PiezoSpeaker piezoSpeaker;

        public MeadowApp()
        {
            piezoSpeaker = new PiezoSpeaker(Device.CreatePwmPort(Device.Pins.D05));

            TestPiezoSpeaker();
        }

        protected void TestPiezoSpeaker()
        {
            while (true)
            {
                Console.WriteLine("Playing A4 note!");
                piezoSpeaker.PlayTone(440, 1000);
                piezoSpeaker.StopTone();
                Thread.Sleep(500);
            }
        }
    }
}