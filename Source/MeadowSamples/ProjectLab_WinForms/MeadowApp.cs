using Meadow;
using Meadow.Foundation.Displays;

namespace ProjectLab_WinForms
{
    public class MeadowApp : App<Windows>
    {
        private WinFormsDisplay _display = default!;
        private DisplayController _displayController;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            _display = new WinFormsDisplay(width: 320, height: 240);
            _displayController = new DisplayController(_display);

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Console.WriteLine("Run...");

            Application.Run(_display);

            return Task.CompletedTask;
        }

        public static async Task Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize();

            await MeadowOS.Start(args);
        }
    }
}