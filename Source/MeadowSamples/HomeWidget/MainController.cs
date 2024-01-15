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

        hardware.EnvironmentalSensor.StartUpdating(TimeSpan.FromMinutes(30));
    }

    public void LoadRecipes()
    {
        recipes = new List<WeeklyMeal>()
        {
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Japan curry + rice" },
                MealB = new Recipe() { Name = "Baked pasta" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Fried rice" },
                MealB = new Recipe() { Name = "Mac + cheese" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Chicken + salad" },
                MealB = new Recipe() { Name = "Tuna casserole" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Korean beef bowl" },
                MealB = new Recipe() { Name = "Chicken + veggies" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Greek yogurt chicken" },
                MealB = new Recipe() { Name = "Pesto Turkey Spghtti" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "One pan chicken rice" },
                MealB = new Recipe() { Name = "Meatloaf + veggies" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Burrito bowl" },
                MealB = new Recipe() { Name = "Stuffed tender loins" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Taco salad" },
                MealB = new Recipe() { Name = "Pulled chikn veggies" }
            },
            new WeeklyMeal()
            {
                MealA = new Recipe() { Name = "Y+Y Chicken Salad" },
                MealB = new Recipe() { Name = "Chili + rice" }
            }
        };
    }
    async Task UpdateOutdoorValues()
    {
        var outdoorConditions = await restClientController.GetWeatherForecast();

        if (outdoorConditions != null)
        {
            firstWeatherForecast = false;

            var meals = GetWeeklyMeals();

            Resolver.Log.Info($"HTU21D - " +
                $"Temp: {hardware.EnvironmentalSensor.Temperature.Value.Celsius} | " +
                $"Humidity: {hardware.EnvironmentalSensor.Humidity.Value.Percent}");

            displayController.UpdateDisplay(
                outdoorIcon: outdoorConditions.Value.Item1,
                outdoorTemperature: outdoorConditions.Value.Item2,
                outdoorHumidity: outdoorConditions.Value.Item3,
                temperature: hardware.EnvironmentalSensor.Temperature.Value.Celsius,
                humidity: hardware.EnvironmentalSensor.Humidity.Value.Percent,
                meals.Item1.MealA.Name,
                meals.Item1.MealB.Name,
                meals.Item2.MealA.Name,
                meals.Item2.MealB.Name);
        }
    }

    static int GetCurrentWeek(DateTime startDate, DateTime currentDate)
    {
        TimeSpan timeDifference = currentDate - startDate;
        int totalWeeks = (int)(timeDifference.TotalDays / 7);

        int currentEvent = (totalWeeks % 9);

        return currentEvent;
    }

    (WeeklyMeal, WeeklyMeal) GetWeeklyMeals()
    {
        DateTime currentDate = DateTime.Now;
        DateTime startDate = new DateTime(currentDate.Year, 1, 1);

        int week = GetCurrentWeek(startDate, currentDate);
        int upcomingWeek = week + 2 > 9 ? 0 : week + 2;
        int weekAfter = upcomingWeek + 1 > 9 ? 0 : upcomingWeek + 1;

        Resolver.Log.Info($"Week {upcomingWeek} and {weekAfter}");

        return new(recipes[upcomingWeek], recipes[weekAfter]);
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