using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoonModel
{
    public class MeadowApp : App<F7FeatherV1>
    {
        private List<string> colorHexCodes = new List<string> {
            "#FF5733", "#33FF57", "#3357FF", "#FF33A6", "#F3FF33"
            // Add more hex codes here for more transitions
        };

        RgbPwmLed onboardLed;

        RgbPwmLed[] rgbPwmLeds;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);

            rgbPwmLeds = new RgbPwmLed[4];
            rgbPwmLeds[0] = new RgbPwmLed(
                    Device.Pins.D02,
                    Device.Pins.D03,
                    Device.Pins.D04);
            rgbPwmLeds[1] = new RgbPwmLed(
                    Device.Pins.D05,
                    Device.Pins.D06,
                    Device.Pins.D07);
            rgbPwmLeds[2] = new RgbPwmLed(
                    Device.Pins.D08,
                    Device.Pins.D09,
                    Device.Pins.D10);
            rgbPwmLeds[3] = new RgbPwmLed(
                    Device.Pins.D11,
                    Device.Pins.D12,
                    Device.Pins.D13);

            return base.Initialize();
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            CycleColor();

            //BreathingEffect();

            //SmoothCycle();
        }

        async Task CycleColor()
        {
            var duration = TimeSpan.FromSeconds(1);

            while (true)
            {
                await SetColor(Color.FromHex("#FF1508"), duration);
                await SetColor(Color.FromHex("#FF3008"), duration);
                await SetColor(Color.FromHex("#FF4B08"), duration);
                await SetColor(Color.FromHex("#FF6608"), duration);
                await SetColor(Color.FromHex("#FF8508"), duration);
                await SetColor(Color.FromHex("#FFA908"), duration);
                await SetColor(Color.FromHex("#FFCB08"), duration);
                await SetColor(Color.FromHex("#FFEE08"), duration);
                await SetColor(Color.FromHex("#E0F50C"), duration);
                await SetColor(Color.FromHex("#AEE413"), duration);
                await SetColor(Color.FromHex("#78D21B"), duration);
                await SetColor(Color.FromHex("#42C021"), duration);
                await SetColor(Color.FromHex("#22BA40"), duration);
                await SetColor(Color.FromHex("#1BC177"), duration);
                await SetColor(Color.FromHex("#13C8AD"), duration);
                await SetColor(Color.FromHex("#0CD0E4"), duration);
                await SetColor(Color.FromHex("#0FC1F9"), duration);
                await SetColor(Color.FromHex("#1C9BEC"), duration);
                await SetColor(Color.FromHex("#2975DE"), duration);
                await SetColor(Color.FromHex("#3750D1"), duration);
                await SetColor(Color.FromHex("#5037D1"), duration);
                await SetColor(Color.FromHex("#7629DE"), duration);
                await SetColor(Color.FromHex("#9A1CEB"), duration);
                await SetColor(Color.FromHex("#C00FF8"), duration);
            }
        }
        async Task SetColor(Color color, TimeSpan duration)
        {
            rgbPwmLeds[0].SetColor(color);
            rgbPwmLeds[1].SetColor(color);
            rgbPwmLeds[2].SetColor(color);
            rgbPwmLeds[3].SetColor(color);
            await Task.Delay(duration);
        }

        void SetColor(Color color)
        {
            rgbPwmLeds[0].SetColor(color);
            rgbPwmLeds[1].SetColor(color);
            rgbPwmLeds[2].SetColor(color);
            rgbPwmLeds[3].SetColor(color);
        }

        void BreathingEffect()
        {
            bool increasing = true;
            float hue = 0;

            while (true) // Infinite loop for continuous effect
            {
                var color = ColorFromHSBA(hue, 1, 1, 1);

                SetColor(color);

                hue += 1;
                if (hue >= 360)
                {
                }
                Resolver.Log.Info($"Hue: {hue}");

                Thread.Sleep(50);
            }
        }
        Color ColorFromHSBA(double hue, double saturation, double brightness, double alpha)
        {
            double chroma = brightness * saturation;
            double huePrime = hue / 60.0;
            double x = chroma * (1 - Math.Abs(huePrime % 2 - 1));
            double r = 0, g = 0, b = 0;

            if (huePrime >= 0 && huePrime <= 1)
            {
                r = chroma; g = x;
            }
            else if (huePrime > 1 && huePrime <= 2)
            {
                r = x; g = chroma;
            }
            else if (huePrime > 2 && huePrime <= 3)
            {
                g = chroma; b = x;
            }
            else if (huePrime > 3 && huePrime <= 4)
            {
                g = x; b = chroma;
            }
            else if (huePrime > 4 && huePrime <= 5)
            {
                r = x; b = chroma;
            }
            else if (huePrime > 5 && huePrime <= 6)
            {
                r = chroma; b = x;
            }

            double m = brightness - chroma;
            r += m; g += m; b += m;

            // Clamp values between 0 and 1 and convert to 255 scale for RGB
            r = Math.Min(Math.Max(r, 0), 1) * 255;
            g = Math.Min(Math.Max(g, 0), 1) * 255;
            b = Math.Min(Math.Max(b, 0), 1) * 255;

            return Color.FromRgba((int)r, (int)g, (int)b, (int)(alpha * 255));
        }


        private int currentColorIndex = 0;
        private double transitionSpeed = 0.01;
        async Task SmoothCycle()
        {
            var currentColor = Color.FromHex("#FFFFFF");

            while (true)
            {
                await GetNextColorAsync(currentColor);
                await Task.Delay(3000);
            }
        }
        async Task<Color> GetNextColorAsync(Color currentColor)
        {
            var targetColor = Color.FromHex(colorHexCodes[currentColorIndex]);

            currentColorIndex = (currentColorIndex + 1) % colorHexCodes.Count;

            for (double t = 0; t < 1; t += transitionSpeed)
            {
                double r = (1 - t) * currentColor.R + t * targetColor.R;
                double g = (1 - t) * currentColor.G + t * targetColor.G;
                double b = (1 - t) * currentColor.B + t * targetColor.B;

                var transitionColor = new Color((float)r, (float)g, (float)b);

                await Task.Delay(30000);

                SetColor(transitionColor);
            }

            return targetColor;
        }
    }
}