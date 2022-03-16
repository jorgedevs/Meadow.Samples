﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;

namespace Bluetooth.Led_Sample
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        Color selectedColor;

        RgbPwmLed onboardLed;

        Definition bleTreeDefinition;
        CharacteristicBool isOnCharacteristic;
        CharacteristicInt32 colorCharacteristic;

        readonly string IS_ON = "24517ccc888e4ffc9da521884353b08d";
        readonly string COLOR = "5a0bb01669ab4a49a2f2de5b292458f3";

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            PulseColor(Color.Red);

            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            isOnCharacteristic.ValueSet += IsOnCharacteristicValueSet;
            colorCharacteristic.ValueSet += ColorCharacteristicValueSet;

            PulseColor(Color.Green);
        }

        void IsOnCharacteristicValueSet(ICharacteristic c, object data)
        {
            if ((bool)data)
            {
                PulseColor(selectedColor);
                onboardLed.IsOn = true;
                isOnCharacteristic.SetValue(false);
            }
            else
            {
                onboardLed.Stop();
                onboardLed.IsOn = false;
                isOnCharacteristic.SetValue(true);
            }
        }

        void ColorCharacteristicValueSet(ICharacteristic c, object data)
        {
            int color = (int)data;

            byte r = (byte)((color >> 16) & 0xff);
            byte g = (byte)((color >> 8) & 0xff);
            byte b = (byte)((color >> 0) & 0xff);

            PulseColor(new Color(r / 255.0, g / 255.0, b / 255.0));

            colorCharacteristic.SetValue(color);
        }

        void PulseColor(Color color)
        {
            selectedColor = color;
            onboardLed.Stop();
            onboardLed.StartPulse(color);
        }

        Definition GetDefinition()
        {
            isOnCharacteristic = new CharacteristicBool(
                name: "On_Off",
                uuid: IS_ON,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);

            colorCharacteristic = new CharacteristicInt32(
                name: "CurrentColor",
                uuid: COLOR,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);

            var service = new Service(
                name: "ServiceA",
                uuid: 253,
                isOnCharacteristic,
                colorCharacteristic
            );

            return new Definition("MeadowRGB", service);
        }
    }
}