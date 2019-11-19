﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;

namespace PlantMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        LedBarGraph ledBarGraph;
        Capacitive capacitive;

        public MeadowApp()
        {
            Initialize();
            Run();
        }

        public void Initialize()
        {
            Console.WriteLine("Creating Outputs...");

            IDigitalOutputPort[] ports =
            {
                 Device.CreateDigitalOutputPort(Device.Pins.D05),
                 Device.CreateDigitalOutputPort(Device.Pins.D06),
                 Device.CreateDigitalOutputPort(Device.Pins.D07),
                 Device.CreateDigitalOutputPort(Device.Pins.D08),
                 Device.CreateDigitalOutputPort(Device.Pins.D09),
                 Device.CreateDigitalOutputPort(Device.Pins.D10),
                 Device.CreateDigitalOutputPort(Device.Pins.D11),
                 Device.CreateDigitalOutputPort(Device.Pins.D12),
                 Device.CreateDigitalOutputPort(Device.Pins.D13),
                 Device.CreateDigitalOutputPort(Device.Pins.D14)
            };
            ledBarGraph = new LedBarGraph(ports);

            capacitive = new Capacitive(Device.CreateAnalogInputPort(Device.Pins.A00), 2.84f, 1.37f);
        }

        public void Run()
        {
            while (true)
            {
                float moisture = capacitive.Read();

                if (moisture > 100)
                    moisture = 100.0f;
                else
                if (moisture < 0)
                    moisture = 0.0f;

                ledBarGraph.Percentage = moisture / 100f;

                Console.WriteLine($"Raw: {capacitive.Moisture} | Moisture {moisture}%");
                Thread.Sleep(1000);
            }
        }
    }
}