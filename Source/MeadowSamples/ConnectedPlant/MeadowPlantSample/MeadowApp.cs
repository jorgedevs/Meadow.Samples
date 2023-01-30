using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowPlantSample
{
    public class MeadowApp : App<F7FeatherV1>
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.10f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.50f;

        Capacitive capacitive;
        LedBarGraph ledBarGraph;

        public override async Task Initialize()
        {
            var led = new RgbLed(
                Device, 
                Device.Pins.OnboardLedRed, 
                Device.Pins.OnboardLedGreen, 
                Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLedColors.Red);

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
            ledBarGraph.StartBlink(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
            await Task.Delay(5000);
            ledBarGraph.Stop();

            capacitive = new Capacitive
            (
                Device.CreateAnalogInputPort(Device.Pins.A00),
                minimumVoltageCalibration: new Voltage(MINIMUM_VOLTAGE_CALIBRATION),
                maximumVoltageCalibration: new Voltage(MAXIMUM_VOLTAGE_CALIBRATION)
            );
            capacitive.Updated += CapacitiveUpdated;

            led.SetColor(RgbLedColors.Green);
        }

        private void CapacitiveUpdated(object sender, IChangeResult<double> e)
        {
            var percentage = e.New;
            Console.WriteLine($"{percentage}");
            
            if (percentage > 1) { percentage = 1; }
            else if (percentage < 0) { percentage = 0; }

            ledBarGraph.Percentage = (float)percentage;
            ledBarGraph.StartBlink(ledBarGraph.GetTopLedForPercentage());
        }

        async Task Calibration()
        {
            IAnalogInputPort analogIn = Device.CreateAnalogInputPort(Device.Pins.A00, 5, TimeSpan.FromMilliseconds(40), new Voltage(3.3));
            Voltage voltage;

            while (true)
            {
                voltage = await analogIn.Read();
                Console.WriteLine("Voltage: " + voltage.Millivolts.ToString());
                Thread.Sleep(1000);
            }
        }

        public override Task Run()
        {
            capacitive.StartUpdating(TimeSpan.FromSeconds(1));

            //Calibration();

            return Task.CompletedTask;
        }
    }
}