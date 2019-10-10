using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;
using System.Threading;

namespace Simon
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int ANIMATION_DELAY = 200;

        Led[] leds = new Led[4];

        PushButton buttonRed;
        PushButton buttonGreen;
        PushButton buttonBlue;
        PushButton buttonYellow;

        bool isAnimating = false;
        SimonGame game = new SimonGame();

        public MeadowApp()
        {
            leds[0] = new Led(Device.CreateDigitalOutputPort(Device.Pins.D10));
            leds[1] = new Led(Device.CreateDigitalOutputPort(Device.Pins.D09));
            leds[2] = new Led(Device.CreateDigitalOutputPort(Device.Pins.D08));
            leds[3] = new Led(Device.CreateDigitalOutputPort(Device.Pins.D07));

            buttonRed = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D01, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            buttonRed.Clicked += ButtonRedClicked;

            buttonGreen = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            buttonGreen.Clicked += ButtonGreenClicked;

            buttonBlue = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D03, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            buttonBlue.Clicked += ButtonBlueClicked;

            buttonYellow = new PushButton(Device.CreateDigitalInputPort(Device.Pins.D04, InterruptMode.EdgeBoth, ResistorMode.Disabled));
            buttonYellow.Clicked += ButtonYellowClicked;

            Console.WriteLine("Welcome to Simon");
            SetAllLEDs(true);
            game.OnGameStateChanged += OnGameStateChanged;
            game.Reset();
        }

        void ButtonRedClicked(object sender, EventArgs e)
        {
            OnButton(0);
        }

        void ButtonGreenClicked(object sender, EventArgs e)
        {
            OnButton(1);
        }

        void ButtonBlueClicked(object sender, EventArgs e)
        {
            OnButton(2);
        }

        void ButtonYellowClicked(object sender, EventArgs e)
        {
            OnButton(3);
        }

        void OnButton(int buttonIndex)
        {
            Console.WriteLine("Button tapped: " + buttonIndex);
            if (isAnimating == false)
            {                
                TurnOnLED(buttonIndex);
                game.EnterStep(buttonIndex);
            }
        }

        void OnGameStateChanged(object sender, SimonEventArgs e)
        {
            var th = new Thread(() =>
            {
                switch (e.GameState)
                {
                    case GameState.Start:
                        break;
                    case GameState.NextLevel:
                        ShowStartAnimation();
                        ShowNextLevelAnimation(game.Level);
                        ShowSequenceAnimation(game.Level);
                        break;
                    case GameState.GameOver:
                        ShowGameOverAnimation();
                        game.Reset();
                        break;
                    case GameState.Win:
                        ShowGameWonAnimation();
                        break;
                }
            });
            th.Start();
        }

        void TurnOnLED(int index, int duration = 400)
        {
            leds[index].IsOn = true;
            Thread.Sleep(duration);
            leds[index].IsOn = false;
        }

        void SetAllLEDs(bool isOn)
        {
            leds[0].IsOn = isOn;
            leds[1].IsOn = isOn;
            leds[2].IsOn = isOn;
            leds[3].IsOn = isOn;
        }

        void ShowStartAnimation()
        {
            if (isAnimating)
                return;
            isAnimating = true;
            SetAllLEDs(false);
            for (int i = 0; i < 4; i++)
            {
                leds[i].IsOn = true;
                Thread.Sleep(ANIMATION_DELAY);
            }
            for (int i = 0; i < 4; i++)
            {
                leds[3 - i].IsOn = false;
                Thread.Sleep(ANIMATION_DELAY);
            }
            isAnimating = false;
        }

        void ShowNextLevelAnimation(int level)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            SetAllLEDs(false);
            for (int i = 0; i < level; i++)
            {
                Thread.Sleep(ANIMATION_DELAY);
                SetAllLEDs(true);
                Thread.Sleep(ANIMATION_DELAY * 3);
                SetAllLEDs(false);
            }
            isAnimating = false;
        }

        void ShowSequenceAnimation(int level)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            var steps = game.GetStepsForLevel();
            SetAllLEDs(false);
            for (int i = 0; i < level; i++)
            {
                Thread.Sleep(200);
                TurnOnLED(steps[i], 400);
            }
            isAnimating = false;
        }

        void ShowGameOverAnimation()
        {
            if (isAnimating)
                return;
            isAnimating = true;
            
            for (int i = 0; i < 20; i++)
            {
                SetAllLEDs(false);
                Thread.Sleep(50);
                SetAllLEDs(true);
                Thread.Sleep(50);
            }
            isAnimating = false;
        }

        void ShowGameWonAnimation()
        {
            ShowStartAnimation();
            ShowStartAnimation();
            ShowStartAnimation();
            ShowStartAnimation();
        }
    }
}