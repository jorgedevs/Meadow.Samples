using Meadow;
using System.Threading;

namespace Simon
{
    class Program
    {
        static IApp app;

        static void Main(string[] args)
        {
            app = new App();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}