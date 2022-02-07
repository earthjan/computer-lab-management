using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using RemoteScreenshot.Models;
using static System.Console;

namespace RemoteScreenshot.MyProcess
{
    public class PingHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public PingHostedService(IServiceProvider services)
        {
            this.Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                try
                {
                    StartPingHost(this.Services);

                    // actually, IDK why I put a delay here as the main delay timer. However, this delays the hosted service before running again.
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public static void StartPingHost(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RemoteDesktopContext>();

                var pingSender = new Ping();
                var pingOptions = new PingOptions(128, true);

                List<Desktop> remoteDesktops = db.GetAllRemoteDesktops();

                remoteDesktops.ForEach((desktop) =>
                {
                    PingReply reply = pingSender.Send(desktop.IpAddress);

                    if (reply.Status == IPStatus.Success)
                    {
                        WriteLine("Ping successed for desktopID: {0}", desktop.DesktopId);
                        db.UpdateDesktopStatus(desktop.DesktopId, 1);
                    }
                    else
                    {
                        WriteLine("Ping failed for desktopID: {0}", desktop.DesktopId);
                        db.UpdateDesktopStatus(desktop.DesktopId, 0);
                    }
                });
            }
        }
    }
}
