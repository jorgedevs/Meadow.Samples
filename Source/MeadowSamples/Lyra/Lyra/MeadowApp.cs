using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lyra
{
    public class MeadowApp : App<F7FeatherV2>
    {
        Led redLed;

        // Pacific (PDT = UTC-7). Change UTC hours by +1 when switching back to PST.
        static readonly (int hour, int minute, bool on)[] Schedule =
        {
            (6,  0, true),   // ON  at 23:00 PDT (06:00 UTC)
            (13, 0, false),  // OFF at 06:00 PDT (13:00 UTC)
        };

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            redLed = new Led(Device.Pins.D00);

            return base.Initialize();
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            await WaitForNtpSync();

            _ = LogTimeEveryMinute();

            while (true)
            {
                var now = DateTime.UtcNow;
                ApplySchedule(now);

                var delay = TimeUntilNextEvent(now);
                Resolver.Log.Info($"Next event in {delay.TotalMinutes:F1} min");
                await Task.Delay(delay);
            }
        }

        async Task WaitForNtpSync()
        {
            Resolver.Log.Info("Waiting for NTP sync...");
            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            while (!wifi.IsConnected)
            {
                Resolver.Log.Info($"Network connected: {wifi.IsConnected}");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            var ntpClient = Resolver.Device.PlatformOS.NtpClient;
            var ntpServer = Resolver.Device.PlatformOS.NtpServers?.FirstOrDefault();
            var synced = await ntpClient.Synchronize(ntpServer);
            Resolver.Log.Info(synced
                ? $"NTP synced: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC"
                : "NTP sync failed, proceeding with current clock");
        }

        async Task LogTimeEveryMinute()
        {
            while (true)
            {
                var utc = DateTime.UtcNow;
                var pst = utc.AddHours(-7); // PDT (UTC-7); change to -8 for PST
                Resolver.Log.Info($"UTC: {utc:yyyy-MM-dd HH:mm:ss} | Pacific: {pst:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        void ApplySchedule(DateTime now)
        {
            var latest = Schedule
                .Where(s => new TimeSpan(s.hour, s.minute, 0) <= now.TimeOfDay)
                .OrderByDescending(s => s.hour * 60 + s.minute)
                .FirstOrDefault();

            if (latest != default)
            {
                redLed.IsOn = latest.on;
                Resolver.Log.Info($"LED {(latest.on ? "ON" : "OFF")}");
            }
        }

        TimeSpan TimeUntilNextEvent(DateTime now)
        {
            var upcoming = Schedule
                .Select(s => new DateTime(now.Year, now.Month, now.Day, s.hour, s.minute, 0, DateTimeKind.Utc))
                .Where(t => t > now)
                .OrderBy(t => t)
                .FirstOrDefault();

            if (upcoming == default)
            {
                var first = Schedule.OrderBy(s => s.hour * 60 + s.minute).First();
                upcoming = new DateTime(now.Year, now.Month, now.Day, first.hour, first.minute, 0, DateTimeKind.Utc)
                               .AddDays(1);
            }

            return upcoming - now;
        }
    }
}