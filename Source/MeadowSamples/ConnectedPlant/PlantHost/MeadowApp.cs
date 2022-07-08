using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowApp
{
    public class MeadowApp : App<F7FeatherV1>, IApp
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.81f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.50f;

        Capacitive capacitive;
        LedBarGraph ledBarGraph;

        async Task IApp.Initialize()
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
            ledBarGraph.StartBlink();
            await Task.Delay(5000);
            ledBarGraph.Stop();

            capacitive = new Capacitive
            (
                Device.CreateAnalogInputPort(Device.Pins.A00), 
                new Voltage(MINIMUM_VOLTAGE_CALIBRATION, Voltage.UnitType.Volts),
                new Voltage(MAXIMUM_VOLTAGE_CALIBRATION, Voltage.UnitType.Volts)
            );
            var consumer = Capacitive.CreateObserver(
                handler: result => 
                {
                    var percentage = ExtensionMethods.Map(result.New, 0.30, 1.10, 0, 1);
                    Console.WriteLine($"{percentage}");
                    UpdatePercentage(percentage);
                },
                filter: null
            );
            capacitive.Subscribe(consumer);
            await capacitive.Read();

            led.SetColor(RgbLed.Colors.Green);
        }

        async Task Calibration()
        {
            IAnalogInputPort analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);
            Voltage voltage;

            while (true)
            {
                voltage = await analogIn.Read();
                Console.WriteLine("Voltage: " + voltage.ToString());
                Thread.Sleep(1000);
            }
        }

        void UpdatePercentage(double percentage)
        {
            //Console.WriteLine(percentage);

            if (percentage > 1) { percentage = 1; }
            else if (percentage < 0) { percentage = 0; }

            ledBarGraph.Percentage = percentage;
            ledBarGraph.SetLedBlink(ledBarGraph.GetTopLedForPercentage());

            //if (percentage > 1)
            //{
            //    ledBarGraph.Percentage = 1;
            //    ledBarGraph.SetLedBlink(9);
            //}
            //else if (percentage < 0)
            //{
            //    ledBarGraph.Percentage = 0;
            //    ledBarGraph.SetLedBlink(0);
            //}
            //else
            //{
            //    ledBarGraph.Percentage = percentage;
            //    ledBarGraph.SetLedBlink((int)(percentage * 10));
            //}
        }

        public override async Task Run()
        {
            capacitive.StartUpdating(TimeSpan.FromSeconds(1));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}