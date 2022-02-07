using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemoteScreenshot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace RemoteScreenshot.MyProcess
{
    public class AppMonitoringHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public AppMonitoringHostedService(IServiceProvider services)
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
                    StartMonitoring(this.Services);

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

        public void StartMonitoring(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RemoteDesktopContext>();
                
                // List<Desktop> remoteDesktops = db.GetAllRemoteDesktops();

                // remoteDesktops.ForEach((desktop) =>
                // {
                //     string apps = AppMonitoring.GetCurrentlyRunningApps(desktop.AppMonitoringScriptDirectory, desktop.Name, desktop.Username, desktop.Password);

                //     bool isUpdated = db.UpdateCurrentlyRunningApps(desktop.DesktopId, apps);

                //     Console.WriteLine($"Done processing {desktop.Name} for monitoring its apps.");
                //     Console.WriteLine($"    {apps}");
                //     Console.WriteLine($"    The update was {isUpdated}.");
                // });
            }
        }
    }
}