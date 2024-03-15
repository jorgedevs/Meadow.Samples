using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace ProjectLab_WinForms
{
    public class DisplayController
    {
        DisplayScreen _screen;

        public DisplayController(IPixelDisplay display)
        {
            _screen = new DisplayScreen(display);
            _screen.Controls.Add(
            new Box(0, 0, _screen.Width / 2, _screen.Height / 2)
            {
                ForeColor = Meadow.Color.Red
            },
            new Meadow.Foundation.Graphics.MicroLayout.Label(0, 0, _screen.Width / 2, _screen.Height / 2)
            {
                Text = "Hello World!",
                TextColor = Meadow.Color.Black,
                BackColor = Meadow.Color.Transparent,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = Meadow.Foundation.Graphics.HorizontalAlignment.Center
            });
        }
    }
}