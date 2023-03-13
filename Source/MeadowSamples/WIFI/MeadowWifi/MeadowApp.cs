using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace MeadowWifi
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        readonly string SSID = "8c3bb16c0d954fb8b37999e1f040b279";
        readonly string PASSWORD = "b1cb00bd69424cba937a597fabb93052";
        readonly string CONNECT = "98dea8431f7443a3bf24f78650672361";

        IDefinition bleTreeDefinition;

        ICharacteristic Ssid;
        ICharacteristic Password;
        ICharacteristic Connect;

        string ssid;
        string password;

        RgbPwmLed onboardLed;

        public override Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            Ssid.ValueSet += (s, e) => { ssid = (string) e; };
            Password.ValueSet += (s, e) => { password = (string) e; };
            Connect.ValueSet += async (s, e) =>
            {
                var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
                await wifi.Connect(ssid, password, TimeSpan.FromSeconds(45));

                onboardLed.SetColor(Color.Orange);
            };

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        Definition GetDefinition()
        {
            var service = new Service(
                name: "MeadowWifiService",
                uuid: 253,

                Ssid = new CharacteristicString(
                    name: nameof(Ssid),
                    uuid: SSID,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write,
                    maxLength: 256),
                Password = new CharacteristicString(
                    name: nameof(Password),
                    uuid: PASSWORD,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write,
                    maxLength: 256),
                Connect = new CharacteristicBool(
                    name: nameof(Connect),
                    uuid: CONNECT,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write)
            );

            return new Definition("MeadowWifi", service);
        }
    }
}