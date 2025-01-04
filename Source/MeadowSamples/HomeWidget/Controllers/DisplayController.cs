using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;

namespace HomeWidget.Controllers;

public class DisplayController
{
    Image _weatherIcon = Image.LoadFromResource("HomeWidget.Resources.w_misc.bmp");

    private readonly int padding = 10;
    private readonly Color backgroundColor = Color.White;
    private readonly Color foregroundColor = Color.Black;
    private readonly Font16x24 font16X24 = new Font16x24();
    private readonly Font12x20 font12X20 = new Font12x20();
    private readonly Font12x16 font12X16 = new Font12x16();

    private DisplayScreen DisplayScreen;

    private Label Year;
    private Label Month;
    private Label Weekday;
    private Label Day;

    private Picture Weather;

    private Label Temperature;
    private Label FeelsLike;
    private Label Humidity;
    private Label Pressure;
    private Label Sunrise;
    private Label Sunset;

    private Label IndoorTemperature;
    private Label IndoorHumidity;

    public DisplayController(IPixelDisplay display)
    {
        DisplayScreen = new DisplayScreen(display, RotationType._90Degrees)
        {
            BackgroundColor = backgroundColor
        };

        DisplayScreen.BeginUpdate();

        Weather = new Picture(padding + 4, padding + 4, 100, 100, _weatherIcon);
        DisplayScreen.Controls.Add(Weather);

        DisplayScreen.Controls.Add(new Box(padding, padding, 108, 108)
        {
            ForeColor = Color.Black,
            IsFilled = false
        });

        Year = new Label(120, padding, 170, font12X20.Height)
        {
            Text = $"----",
            TextColor = foregroundColor,
            Font = font12X20,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Year);

        Month = new Label(120, 34, 170, font12X20.Height)
        {
            Text = $"----",
            TextColor = foregroundColor,
            Font = font12X20,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Month);

        Weekday = new Label(120, 58, 170, font16X24.Height)
        {
            Text = $"----",
            TextColor = foregroundColor,
            Font = font16X24,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Weekday);

        Day = new Label(120, 86, 170, font12X20.Height * 2)
        {
            Text = $"--",
            TextColor = foregroundColor,
            Font = font12X20,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Day);

        int row0 = 138;
        LoadWeatherReading(padding, row0, DisplayScreen.Width - padding * 2, font12X16.Height, "Temperature", HorizontalAlignment.Left);
        Temperature = CreateWeatherValueLabel(padding, row0 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "-°C", HorizontalAlignment.Left);
        DisplayScreen.Controls.Add(Temperature);

        LoadWeatherReading(padding, row0, DisplayScreen.Width - padding * 2, font12X16.Height, "Humidity", HorizontalAlignment.Right);
        Humidity = CreateWeatherValueLabel(padding, row0 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "-%", HorizontalAlignment.Right);
        DisplayScreen.Controls.Add(Humidity);

        int row1 = 203;
        LoadWeatherReading(padding, row1, DisplayScreen.Width - padding * 2, font12X16.Height, "Feels Like", HorizontalAlignment.Left);
        FeelsLike = CreateWeatherValueLabel(padding, row1 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "-°C", HorizontalAlignment.Left);
        DisplayScreen.Controls.Add(FeelsLike);

        LoadWeatherReading(padding, row1, DisplayScreen.Width - padding * 2, font12X16.Height, "Pressure", HorizontalAlignment.Right);
        Pressure = CreateWeatherValueLabel(padding, row1 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "-atm", HorizontalAlignment.Right);
        DisplayScreen.Controls.Add(Pressure);

        int row2 = 268;
        LoadWeatherReading(padding, row2, DisplayScreen.Width - padding * 2, font12X16.Height, "Sunrise", HorizontalAlignment.Left);
        Sunrise = CreateWeatherValueLabel(padding, row2 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "--:--", HorizontalAlignment.Left);
        DisplayScreen.Controls.Add(Sunrise);

        LoadWeatherReading(padding, row2, DisplayScreen.Width - padding * 2, font12X16.Height, "Sunset", HorizontalAlignment.Right);
        Sunset = CreateWeatherValueLabel(padding, row2 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "--:--", HorizontalAlignment.Right);
        DisplayScreen.Controls.Add(Sunset);

        DisplayScreen.Controls.Add(new Box(padding, 328, 280, 1) { ForeColor = Color.Black });

        int row3 = 345;
        LoadWeatherReading(padding, row3, DisplayScreen.Width - padding * 2, font12X16.Height, "Temperature", HorizontalAlignment.Left);
        IndoorTemperature = CreateWeatherValueLabel(padding, row3 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "-°C", HorizontalAlignment.Left);
        DisplayScreen.Controls.Add(IndoorTemperature);

        LoadWeatherReading(padding, row3, DisplayScreen.Width - padding * 2, font12X16.Height, "Humidity", HorizontalAlignment.Right);
        IndoorHumidity = CreateWeatherValueLabel(padding, row3 + 21, DisplayScreen.Width - padding * 2, font16X24.Height, "-%", HorizontalAlignment.Right);
        DisplayScreen.Controls.Add(IndoorHumidity);
    }

    private void LoadWeatherReading(int left, int top, int width, int height, string title, HorizontalAlignment horizontalAlignment)
    {
        DisplayScreen.Controls.Add(new Label(left, top, width, font12X16.Height)
        {
            Text = title,
            TextColor = foregroundColor,
            Font = font12X16,
            HorizontalAlignment = horizontalAlignment
        });
    }

    private Label CreateWeatherValueLabel(int left, int top, int width, int height, string value, HorizontalAlignment horizontalAlignment)
    {
        return new Label(left, top, width, font16X24.Height)
        {
            Text = value,
            TextColor = foregroundColor,
            Font = font16X24,
            HorizontalAlignment = horizontalAlignment
        };
    }

    private static string GetOrdinalSuffix(int num)
    {
        string number = num.ToString();
        if (number.EndsWith("1")) return "st";
        if (number.EndsWith("2")) return "nd";
        if (number.EndsWith("3")) return "rd";
        if (number.EndsWith("11")) return "th";
        if (number.EndsWith("12")) return "th";
        if (number.EndsWith("13")) return "th";
        return "th";
    }

    public void UpdateDisplay(
        string weatherIcon,
        double temperature,
        double feelsLike,
        double humidity,
        double pressure,
        string sunrise,
        string sunset,
        double indoorTemperature,
        double indoorHumidity)
    {
        DisplayScreen.BeginUpdate();

        _weatherIcon = Image.LoadFromResource(weatherIcon);
        Weather.Image = _weatherIcon;

        int TIMEZONE_OFFSET = -7;
        var today = DateTime.Now.AddHours(TIMEZONE_OFFSET);

        Year.Text = today.Year.ToString();
        Month.Text = today.ToString("MMMM");
        Weekday.Text = today.DayOfWeek.ToString();
        Day.Text = $"{today.Day}{GetOrdinalSuffix(today.Day)}";

        Temperature.Text = $"{temperature:N0}°C";
        Humidity.Text = $"{humidity:N0}%";
        FeelsLike.Text = $"{feelsLike:N0}°C";
        Pressure.Text = $"{pressure:N0}mb";
        Sunrise.Text = sunrise;
        Sunset.Text = sunset;

        IndoorTemperature.Text = $"{indoorTemperature:N0}°C";
        IndoorHumidity.Text = $"{indoorHumidity:N0}%";

        DisplayScreen.EndUpdate();
    }
}