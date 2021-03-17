using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlantWing.Meadow
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

        async Task Initialize()
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

            capacitive = new Capacitive
            (
                Device.CreateAnalogInputPort(Device.Pins.A00),
                MINIMUM_VOLTAGE_CALIBRATION,
                MAXIMUM_VOLTAGE_CALIBRATION
            );
            
            Device.InitWiFiAdapter().Wait();

            ledBarGraph.StartBlink(500, 500);

            var result = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            ledBarGraph.Percentage = 1f;
            Thread.Sleep(1000);
            ledBarGraph.Percentage = 0f;

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
                var reading = await capacitive.Read();
                float moisture = reading.New;

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
