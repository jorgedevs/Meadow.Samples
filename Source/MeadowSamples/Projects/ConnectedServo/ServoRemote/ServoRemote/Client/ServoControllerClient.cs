using Maple;
using System.Threading.Tasks;

namespace ServoRemote.Client
{
    public class ServoControllerClient : MapleClient
    {
        public async Task<bool> RotateToAsync(ServerItem server, int degrees)
        {
            //return (await SendCommandAsync("RotateTo?targetAngle=" + degrees, server.IpAddress));
            await Task.Delay(1000);
            return true;
        }

        public async Task<bool> StartSweepAsync(ServerItem server)
        {
            //return (await SendCommandAsync("StartSweep", server.IpAddress));
            await Task.Delay(1000);
            return true;
        }

        public async Task<bool> StopSweepAsync(ServerItem server)
        {
            //return (await SendCommandAsync("StopSweep", server.IpAddress));
            await Task.Delay(1000);
            return true;
        }
    }
}