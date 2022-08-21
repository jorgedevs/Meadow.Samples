using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Units;

namespace TunePlayer
{
    public class MeadowApp : App<F7FeatherV2>
    {
        const int NUMBER_OF_NOTES = 16;
        Frequency[] melody;
        PiezoSpeaker piezo;

        public override Task Initialize() 
        {
            melody = new Frequency[NUMBER_OF_NOTES]
            {
                new Frequency(NoteFrequencies.NOTE_A3),
                new Frequency(NoteFrequencies.NOTE_B3),
                new Frequency(NoteFrequencies.NOTE_CS4),
                new Frequency(NoteFrequencies.NOTE_D4),
                new Frequency(NoteFrequencies.NOTE_E4),
                new Frequency(NoteFrequencies.NOTE_FS4),
                new Frequency(NoteFrequencies.NOTE_GS4),
                new Frequency(NoteFrequencies.NOTE_A4),
                new Frequency(NoteFrequencies.NOTE_A4),
                new Frequency(NoteFrequencies.NOTE_GS4),
                new Frequency(NoteFrequencies.NOTE_FS4),
                new Frequency(NoteFrequencies.NOTE_E4),
                new Frequency(NoteFrequencies.NOTE_D4),
                new Frequency(NoteFrequencies.NOTE_CS4),
                new Frequency(NoteFrequencies.NOTE_B3),
                new Frequency(NoteFrequencies.NOTE_A3),
            };

            piezo = new PiezoSpeaker(Device, Device.Pins.D10);

            return Task.CompletedTask;
        }

        public override async Task Run()
        {        
            while (true)
            {
                for (int i = 0; i < NUMBER_OF_NOTES; i++)
                {
                    //PlayTone with a duration in synchronous
                    piezo.PlayTone(melody[i], new TimeSpan(600));
                    Thread.Sleep(50);
                }

                await Task.Delay(1000);

                //PlayTone without a duration will return immediately and play the tone
                piezo.PlayTone(new Frequency(NoteFrequencies.NOTE_A4));
                await Task.Delay(2000);

                //call StopTone to end a tone started without a duration
                piezo.StopTone();

                await Task.Delay(2000);
            }
        }
    }

    public static class NoteFrequencies
    {
        public const float NOTE_A3 = 220;
        public const float NOTE_B3 = 247;
        public const float NOTE_CS4 = 277;
        public const float NOTE_D4 = 294;
        public const float NOTE_E4 = 330;
        public const float NOTE_FS4 = 370;
        public const float NOTE_GS4 = 415;
        public const float NOTE_A4 = 440;
    }
}
