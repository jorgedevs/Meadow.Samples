using Amqp;
using Amqp.Framing;
using Meadow;
using Meadow.Units;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeadowDigitalTwin.Azure
{
    /// <summary>
    /// You'll need to create an IoT Hub - https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-create-through-portal
    /// Create a device within your IoT Hub
    /// And then generate a SAS token - this can be done via the Azure CLI 
    /// 
    /// Example
    /// az iot hub generate-sas-token
    /// --hub-name HUB_NAME 
    /// --device-id DEVICE_ID 
    /// --resource-group RESOURCE_GROUP 
    /// --login [Open Shared access policies -> Select iothubowner -> copy Primary connection string]
    /// </summary>
    public class IotHubManager
    {
        private Connection connection;
        private SenderLink sender;

        public IotHubManager() { }

        public async Task Initialize()
        {
            string hostName = Secrets.HUB_NAME + ".azure-devices.net";
            string userName = Secrets.DEVICE_ID + "@sas." + Secrets.HUB_NAME;
            string senderAddress = "devices/" + Secrets.DEVICE_ID + "/messages/events";

            Resolver.Log.Info("Create connection factory...");
            var factory = new ConnectionFactory();

            Resolver.Log.Info("Create connection ...");
            connection = await factory.CreateAsync(new Address(hostName, 5671, userName, Secrets.SAS_TOKEN));

            Resolver.Log.Info("Create session ...");
            var session = new Session(connection);

            Resolver.Log.Info("Create SenderLink ...");
            sender = new SenderLink(session, "send-link", senderAddress);
        }

        public Task SendEnvironmentalReading(
            (Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) reading,
            Volume volume)
        {
            try
            {


                string messagePayload = $"" +
                        $"{{" +
                        $"\"Temperature\":{reading.Temperature.Value.Celsius.ToString("F1")}," +
                        $"\"Humidity\":{reading.Humidity.Value.Percent.ToString("F1")}," +
                        $"\"Pressure\":{reading.Pressure.Value.Millibar.ToString("F1")}," +
                        $"\"Volume\":{volume.Milliliters.ToString("F1")}" +
                        $"}}";

                var payloadBytes = Encoding.UTF8.GetBytes(messagePayload);
                var message = new Message()
                {
                    BodySection = new Data() { Binary = payloadBytes }
                };

                sender.SendAsync(message);

                Resolver.Log.Info($"Message sent - Temperature: {reading.Temperature.Value.Celsius:n1}ºC | " +
                    $"Humidity: {reading.Humidity.Value.Percent:n1}% | " +
                    $"Pressure: {reading.Pressure.Value.Millibar:n1}mBar | " +
                    $"Volume: {volume.Milliliters:n1}ml");
            }
            catch (Exception ex)
            {
                Resolver.Log.Info($"-- D2C Error - {ex.Message} --");
            }

            return Task.CompletedTask;
        }
    }
}
