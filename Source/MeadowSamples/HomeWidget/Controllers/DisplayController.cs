using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using System;

namespace HomeWidget.Controllers;

public class DisplayController
{
    Image _weatherIcon = Image.LoadFromResource("HomeWidget.Resources.w_misc.bmp");

    int padding = 10;

    protected DisplayScreen DisplayScreen { get; set; }

    protected Label YearMonth { get; set; }

    protected Label WeekdayDay { get; set; }

    protected Label Time { get; set; }

    protected Picture Weather { get; set; }

    protected Label Temperature { get; set; }

    protected Label Humidity { get; set; }

    protected Label OutdoorTemperature { get; set; }

    protected Label OutdoorHumidity { get; set; }

    protected Label UpcomingWeekMealA { get; set; }

    protected Label UpcomingWeekMealB { get; set; }

    protected Label WeekAfterMealA { get; set; }

    protected Label WeekAfterMealB { get; set; }

    Color backgroundColor = Color.White;
    Color foregroundColor = Color.Black;

    Font12x20 font12X20 = new Font12x20();
    Font6x8 font6x8 = new Font6x8();

    public DisplayController(IGraphicsDisplay display)
    {
        DisplayScreen = new DisplayScreen(display, RotationType._270Degrees)
        {
            BackgroundColor = backgroundColor
        };

        DisplayScreen.BeginUpdate();

        DisplayScreen.Controls.Add(new Box(0, 0, DisplayScreen.Width, DisplayScreen.Height)
        {
            ForeColor = Color.Black,
            IsFilled = false
        });

        Weather = new Picture(padding, padding, 100, 100, _weatherIcon);
        DisplayScreen.Controls.Add(Weather);

        DisplayScreen.Controls.Add(new Box(padding, padding, 100, 100)
        {
            ForeColor = Color.Black,
            IsFilled = false
        });

        OutdoorTemperature = new Label(padding + 2, padding + 2, font6x8.Width * 4, font6x8.Height)
        {
            Text = $"-C",
            TextColor = foregroundColor,
            Font = font6x8
        };
        DisplayScreen.Controls.Add(OutdoorTemperature);

        OutdoorHumidity = new Label(84, padding + 2, font6x8.Width * 4, font6x8.Height)
        {
            Text = $"-%",
            TextColor = foregroundColor,
            Font = font6x8,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(OutdoorHumidity);

        YearMonth = new Label(120, padding, 170, font12X20.Height)
        {
            Text = $"---- ----",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(YearMonth);

        WeekdayDay = new Label(120, 40, 170, font12X20.Height)
        {
            Text = $"---- ----",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(WeekdayDay);

        Time = new Label(120, 70, 170, font12X20.Height * 2)
        {
            Text = $"--:--",
            TextColor = foregroundColor,
            Font = font12X20,
            ScaleFactor = ScaleFactor.X2,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Time);

        DisplayScreen.Controls.Add(new Label(padding, 125, 135, font12X20.Height)
        {
            Text = $"TEMPERATURE",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });

        DisplayScreen.Controls.Add(new Label(155, 125, 135, font12X20.Height)
        {
            Text = $"HUMIDITY",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });

        Temperature = new Label(padding, 155, 135, font12X20.Height * 2)
        {
            Text = $"--.-°C",
            TextColor = foregroundColor,
            Font = font12X20,
            ScaleFactor = ScaleFactor.X2,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Temperature);

        Humidity = new Label(155, 155, 135, font12X20.Height * 2)
        {
            Text = $"--%",
            TextColor = foregroundColor,
            Font = font12X20,
            ScaleFactor = ScaleFactor.X2,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Humidity);

        DisplayScreen.Controls.Add(new Box(padding, 204, 280, 1)
        {
            ForeColor = Color.Black
        });

        DisplayScreen.Controls.Add(new Label(padding, 220, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"UPCOMING WEEK:",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });

        UpcomingWeekMealA = new Label(padding, 250, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"- Meal A",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(UpcomingWeekMealA);

        UpcomingWeekMealB = new Label(padding, 280, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"- Meal B",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(UpcomingWeekMealB);

        DisplayScreen.Controls.Add(new Label(padding, 310, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"WEEK AFTER:",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });

        WeekAfterMealA = new Label(padding, 340, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"- Meal A",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(WeekAfterMealA);

        WeekAfterMealB = new Label(padding, 370, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"- Meal B",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(WeekAfterMealB);

        DisplayScreen.EndUpdate();
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
        string outdoorIcon,
        double outdoorTemperature,
        double outdoorHumidity,
        double temperature,
        double humidity,
        string upcomingWeekMealA,
        string upcomingWeekMealB,
        string weekAfterMealA,
        string weekAfterMealB)
    {
        DisplayScreen.BeginUpdate();

        _weatherIcon = Image.LoadFromResource(outdoorIcon);
        Weather.Image = _weatherIcon;

        OutdoorTemperature.Text = $"{outdoorTemperature:N0}C";
        OutdoorHumidity.Text = $"{outdoorHumidity:N0}%";

        int TIMEZONE_OFFSET = -8;
        var today = DateTime.Now.AddHours(TIMEZONE_OFFSET);

        YearMonth.Text = today.ToString("yyyy MMMM");
        WeekdayDay.Text = $"{today.DayOfWeek},{today.Day}{GetOrdinalSuffix(today.Day)}";
        Time.Text = today.ToString("hh:mm");

        Temperature.Text = $"{temperature:N1}°C";
        Humidity.Text = $"{humidity:N0}%";

        UpcomingWeekMealA.Text = $" - {upcomingWeekMealA}";
        UpcomingWeekMealB.Text = $" - {upcomingWeekMealB}";
        WeekAfterMealA.Text = $" - {weekAfterMealA}";
        WeekAfterMealB.Text = $" - {weekAfterMealB}";

        DisplayScreen.EndUpdate();
    }
}