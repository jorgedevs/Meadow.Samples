using Meadow;
using Meadow.Foundation.Displays;
using WifiWeather.Services;
using WifiWeather.ViewModels;
using WifiWeather.Views;

public class MeadowApp : App<Windows>
{
    private WinFormsDisplay _display = default!;
    private DisplayView _displayController;

    public override Task Initialize()
    {
        Console.WriteLine("Creating Outputs");

        _display = new WinFormsDisplay(width: 320, height: 480);

        _displayController = new DisplayView();
        _displayController.Initialize(_display);

        return Task.CompletedTask;
    }

    async Task GetTemperature()
    {
        // Get outdoor conditions
        var outdoorConditions = await WeatherService.GetWeatherForecast();

        // Format indoor/outdoor conditions data
        var model = new WeatherViewModel(outdoorConditions);

        // Send formatted data to display to render
        _displayController.UpdateDisplay(model);
    }

    public override Task Run()
    {
        _ = Task.Run(async () =>
        {
            await GetTemperature();

            while (true)
            {
                if (DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                {
                    await GetTemperature();
                }

                _displayController.UpdateDateTime();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        });

        Application.Run(_display);

        return Task.CompletedTask;
    }

    public static async Task Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        await MeadowOS.Start(args);
    }
}