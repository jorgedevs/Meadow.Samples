using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Foundation.Leds;
using SimpleJpegDecoder;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MarketProjectLab
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7CoreComputeV2>
    {
        RgbPwmLed onboardLed;
        MicroGraphics graphics;
        IProjectLabHardware projectLab;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            onboardLed = projectLab.RgbLed;
            onboardLed.SetColor(Color.Red);

            graphics = new MicroGraphics(projectLab.Display)
            {
                IgnoreOutOfBoundsPixels = true
            };

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void DrawImage()
        {
            var buffer = LoadJpeg(LoadResource("MarketProjectLab.image2.jpg"));
            graphics.DrawBuffer((graphics.Width - buffer.Width) / 2, 0, buffer);
            graphics.Show();
        }

        IPixelBuffer LoadJpeg(byte[] jpgData)
        {
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            return new BufferRgb888(decoder.Width, decoder.Height, jpg);
        }

        byte[] LoadResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public override Task Run()
        {
            DrawImage();

            return base.Run();
        }
    }
}