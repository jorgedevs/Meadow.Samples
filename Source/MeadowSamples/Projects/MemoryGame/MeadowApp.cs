﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace MemoryGame
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {        
        protected GraphicsLibrary graphics;

        protected int currentColumn;
        protected IDigitalInputPort[] rowPorts = new IDigitalInputPort[4];
        protected IDigitalOutputPort[] columnPorts = new IDigitalOutputPort[4];

        protected char[] options;
        protected bool[] optionsSolved;
        protected char[] optionsPossible;
        protected int option1, option2;

        public MeadowApp()
        {
            options = new char[16];
            optionsSolved = new bool[16];
            optionsPossible = new char[8] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            option1 = option2 = -1;

            InitializePeripherals();
            LoadMemoryBoard();
            StartGameAnimation();
            CyclingColumnVDD();
        }

        protected bool IsLevelComplete()
        {
            bool isComplete = true;

            for (int i = 0; i < 16; i++)
            {
                if (!optionsSolved[i])
                {
                    isComplete = false;
                    break;
                }
            }

            return isComplete;
        }

        protected void InitializePeripherals()
        {            
            var display = new SSD1306(Device.CreateI2cBus(), 60, SSD1306.DisplayType.OLED128x32);
            graphics = new GraphicsLibrary(display);

            rowPorts[0] = Device.CreateDigitalInputPort(Device.Pins.D14, InterruptMode.EdgeFalling, ResistorMode.PullDown, 250);
            rowPorts[1] = Device.CreateDigitalInputPort(Device.Pins.D13, InterruptMode.EdgeFalling, ResistorMode.PullDown, 250);
            rowPorts[2] = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeFalling, ResistorMode.PullDown, 250);
            rowPorts[3] = Device.CreateDigitalInputPort(Device.Pins.D11, InterruptMode.EdgeFalling, ResistorMode.PullDown, 250);

            columnPorts[0] = Device.CreateDigitalOutputPort(Device.Pins.D10);
            columnPorts[1] = Device.CreateDigitalOutputPort(Device.Pins.D09);
            columnPorts[2] = Device.CreateDigitalOutputPort(Device.Pins.D06);
            columnPorts[3] = Device.CreateDigitalOutputPort(Device.Pins.D05);

            currentColumn = 0;
        }

        protected void LoadMemoryBoard()
        {
            for (int i = 0; i < 16; i++)
                options[i] = ' ';

            for (int i = 0; i < 8; i++)
            {
                PlaceCharacter(i);
                PlaceCharacter(i);
            }

            // Uncomment to print all board values
            // for (int i = 0; i < 16; i++)
            //    Debug.Print((i+1).ToString() + " " + options[i].ToString() + " ");
        }

        protected void PlaceCharacter(int i)
        {
            var r = new Random();
            bool isPlaced = false;

            while (!isPlaced)
            {
                int index = r.Next(16);
                if (options[index] == ' ')
                {
                    options[index] = optionsPossible[i];
                    isPlaced = true;
                }
            }
        }

        protected void StartGameAnimation()
        {
            DisplayText("MEMORY GAME", 20);
            Thread.Sleep(1000);
            DisplayText("Ready?", 40);
            Thread.Sleep(1000);
            DisplayText("Start!", 40);
            Thread.Sleep(1000);
            DisplayText("Select Button");
        }

        protected void CyclingColumnVDD()
        {
            Thread thread = new Thread(() =>
            {
                int lastButton = -1;

                while (true)
                {
                    Thread.Sleep(50);

                    int currentButton = -1;
                    switch (currentColumn)
                    {
                        case 0:
                            columnPorts[0].State = true;
                            columnPorts[1].State = false;
                            columnPorts[2].State = false;
                            columnPorts[3].State = false;

                            if (rowPorts[0].State) currentButton = 13;
                            if (rowPorts[1].State) currentButton = 9;
                            if (rowPorts[2].State) currentButton = 5;
                            if (rowPorts[3].State) currentButton = 1;
                            break;

                        case 1:
                            columnPorts[0].State = false;
                            columnPorts[1].State = true;
                            columnPorts[2].State = false;
                            columnPorts[3].State = false;

                            if (rowPorts[0].State) currentButton = 14;
                            if (rowPorts[1].State) currentButton = 10;
                            if (rowPorts[2].State) currentButton = 6;
                            if (rowPorts[3].State) currentButton = 2;
                            break;

                        case 2:
                            columnPorts[0].State = false;
                            columnPorts[1].State = false;
                            columnPorts[2].State = true;
                            columnPorts[3].State = false;

                            if (rowPorts[0].State) currentButton = 15;
                            if (rowPorts[1].State) currentButton = 11;
                            if (rowPorts[2].State) currentButton = 7;
                            if (rowPorts[3].State) currentButton = 3;
                            break;

                        case 3:
                            columnPorts[0].State = false;
                            columnPorts[1].State = false;
                            columnPorts[2].State = false;
                            columnPorts[3].State = true;

                            if (rowPorts[0].State) currentButton = 16;
                            if (rowPorts[1].State) currentButton = 12;
                            if (rowPorts[2].State) currentButton = 8;
                            if (rowPorts[3].State) currentButton = 4;
                            break;
                    }

                    currentColumn = (currentColumn == 3) ? 0 : currentColumn + 1;

                    if (currentButton != lastButton)
                    {
                        if (currentButton != -1)
                        {
                            if (optionsSolved[currentButton - 1])
                            {
                                DisplayText("Button " + options[currentButton - 1] + " Found", 8);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                if (option1 == -1)
                                    option1 = currentButton - 1;
                                else
                                    option2 = currentButton - 1;

                                DisplayText("Button = " + options[currentButton - 1], 24);
                                Thread.Sleep(1000);

                                if (option2 != -1 && option1 != option2)
                                {
                                    if (options[option1] == options[option2])
                                    {
                                        DisplayText(options[option1] + " == " + options[option2], 40);
                                        optionsSolved[option1] = optionsSolved[option2] = true;
                                    }
                                    else
                                        DisplayText(options[option1] + " != " + options[option2], 40);

                                    Thread.Sleep(1000);
                                    option1 = option2 = -1;
                                }
                            }
                        }
                        else
                        {
                            if (IsLevelComplete())
                            {
                                DisplayText("You Win!", 32);
                                Thread.Sleep(1000);
                                LoadMemoryBoard();
                                StartGameAnimation();
                            }
                            else
                            {
                                DisplayText("Select Button");
                            }
                        }
                    }

                    lastButton = currentButton;
                }
            });
            thread.Start();
        }

        protected void DisplayText(string text, int x = 12)
        {
            Console.WriteLine("Hellooooo");

            graphics.Clear();
            graphics.CurrentFont = new Font8x12();
            graphics.DrawRectangle(0, 0, 128, 32);
            graphics.DrawText(x, 12, text);
            graphics.Show();
        }
    }
}