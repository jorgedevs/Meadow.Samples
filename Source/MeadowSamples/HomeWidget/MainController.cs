using HomeWidget.Controllers;
using HomeWidget.Hardware;
using HomeWidget.Models;
using Meadow;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWidget;

internal class MainController
{
    bool firstWeatherForecast = true;

    private List<WeeklyMeal> recipes;

    private IHomeWidgetHardware hardware;
    private INetworkAdapter network;

    private DisplayController displayController;
    private RestClientController restClientController;

    public MainController(IHomeWidgetHardware hardware, IWiFiNetworkAdapter network)
    {
        this.hardware = hardware;
        this.network = network;
    }

    public void Initialize()
    {
        hardware.Initialize();

        LoadRecipes();

        displayController = new DisplayController(hardware.Display);
        restClientController = new RestClientController();

        //displayController.ShowSplashScreen();
        //Thread.Sleep(3000);
        //displayController.ShowDataScreen();
    }

    public void LoadRecipes()
    {
        recipes = new List<WeeklyMeal>();

        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Japan curry + rice" },
            MealB = new Recipe() { Name = "Baked pasta" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Fried rice" },
            MealB = new Recipe() { Name = "Mac + cheese" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Chicken + salad" },
            MealB = new Recipe() { Name = "Tuna casserole" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Korean beef bowl" },
            MealB = new Recipe() { Name = "Chicken + veggies" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Grk Yogurt Chkn" },
            MealB = new Recipe() { Name = "Pesto Turkey Sp" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = " " },
            MealB = new Recipe() { Name = "Meatloaf + Vggs" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Burrito bowl" },
            MealB = new Recipe() { Name = "Stuffed Tender L" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Taco salad" },
            MealB = new Recipe() { Name = "Plld Chkn Vggs" }
        });
        recipes.Add(new WeeklyMeal()
        {
            MealA = new Recipe() { Name = "Y+Y Chicken Salad" },
            MealB = new Recipe() { Name = "Chili + rice" }
        });
    }

    async Task UpdateOutdoorValues()
    {
        var outdoorConditions = await restClientController.GetWeatherForecast();

        if (outdoorConditions != null)
        {
            firstWeatherForecast = false;

            displayController.UpdateDisplay(
                weatherIcon: outdoorConditions.Value.Item1,
                temperature: outdoorConditions.Value.Item2,
                humidity: outdoorConditions.Value.Item3);
        }
    }

    public async Task Run()
    {
        while (true)
        {
            if (network.IsConnected)
            {
                int TimeZoneOffSet = -8; // PST
                var today = DateTime.Now.AddHours(TimeZoneOffSet);

                if (today.Minute == 0 ||
                    today.Minute == 30 ||
                    firstWeatherForecast)
                {
                    Resolver.Log.Trace("Getting forecast values...");

                    await UpdateOutdoorValues();

                    Resolver.Log.Trace("Forecast acquired!");
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
            else
            {
                Resolver.Log.Trace("Not connected, checking in 10s");

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}