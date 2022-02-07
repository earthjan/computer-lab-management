using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RemoteScreenshot;
using Microsoft.Extensions.DependencyInjection;
using RemoteScreenshot.Models;

namespace RemoteScreenshot.MyProcess
{
    public class OutputDeviceHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public OutputDeviceHostedService(IServiceProvider services)
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
                    StartMonitoringOutputDevices(this.Services);
                    // actually, IDK why I put a delay here as the main delay timer (not the first one isntead). However, this delays the hosted service to run again.
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public static void StartMonitoringOutputDevices(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RemoteDesktopContext>();

                List<Desktop> remoteDesktops = db.GetAllRemoteDesktops();

                remoteDesktops.ForEach((desktop) =>
                {
                    string status = OutputDeviceMonitoring.GetOutputDeviceStatus(desktop.OutputDeviceMonitoringScriptDirectory, desktop.Name, desktop.Username, desktop.Password);

                    bool isUpdated = db.UpdateOutputDeviceStatus(desktop.DesktopId, status);

                    Console.WriteLine($"Done processing {desktop.Name}. Its status was {status}. The update was {isUpdated}.");
                });
            }                     
        }
    }
}
