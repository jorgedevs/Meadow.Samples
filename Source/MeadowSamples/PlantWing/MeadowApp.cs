using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;

namespace PlantWing
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.81f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.50f;

        Capacitive capacitive;
        LedBarGraph ledBarGraph;

        public MeadowApp()
        {
            Initialize();

            //Calibration();
            StartReading();
        }

        void Initialize()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            IDigitalOutputPort[] ports =
            {
                Device.CreateDigitalOutputPort(Device.Pins.D11),
                Device.CreateDigitalOutputPort(Device.Pins.D10),
                Device.CreateDigitalOutputPort(Device.Pins.D09),
                Device.CreateDigitalOutputPort(Device.Pins.D08),
                Device.CreateDigitalOutputPort(Device.Pins.D07),
                Device.CreateDigitalOutputPort(Device.Pins.D06),
                Device.CreateDigitalOutputPort(Device.Pins.D05),
                Device.CreateDigitalOutputPort(Device.Pins.D04),
                Device.CreateDigitalOutputPort(Device.Pins.D03),
                Device.CreateDigitalOutputPort(Device.Pins.D02)
            };
            ledBarGraph = new LedBarGraph(ports);

            float percentage = 0;
            while (percentage <= 1)
            {
                percentage += 0.10f;
                ledBarGraph.Percentage = Math.Min(1.0f, percentage);
                Thread.Sleep(100);
            }
            ledBarGraph.Percentage = 0;

            capacitive = new Capacitive
            (
                Device.CreateAnalogInputPort(Device.Pins.A00),
                MINIMUM_VOLTAGE_CALIBRATION,
                MAXIMUM_VOLTAGE_CALIBRATION
            );

            led.SetColor(RgbLed.Colors.Green);
        }

        async Task Calibration() 
        {
            IAnalogInputPort analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);
            float voltage;

            while (true)
            {
                voltage = await analogIn.Read();
                Console.WriteLine("Voltage: " + voltage.ToString());
                Thread.Sleep(1000);
            }
        }

        async Task StartReading()
        {
            while (true)
            {
                float moisture = await capacitive.Read();

                if (moisture > 1)
                    moisture = 1f;
                else
                if (moisture < 0)
                    moisture = 0f;

                ledBarGraph.Percentage = moisture;
                Console.WriteLine($"Moisture {moisture * 100}%");
                Thread.Sleep(1000);
            }
        }
    }
}
