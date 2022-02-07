using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemoteScreenshot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteScreenshot.MyProcess
{
    public class ScreenshotHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public ScreenshotHostedService(IServiceProvider services)
        {
            this.Services = services;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                try
                {
                    StartScreenshotProcess(this.Services);
                    // actually, IDK why I put a delay here as the main delay timer (not the first one isntead). However, this delays the hosted service to run again.
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public static void StartScreenshotProcess(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RemoteDesktopContext>();

                List<Desktop> remoteDesktops = db.GetAllRemoteDesktops();
                string psexecDirectory = db.GetPsexecDirectory();
                string adminConfigsScreenshotDirectory = db.GetAdminConfigsScreenshotDirectory();

                remoteDesktops.ForEach((desktop) =>
                {
                    RemoteDesktopSession.WriteConsoleSessions(
                        desktop.TasklistOutputDirectory,
                        desktop.Name,
                        desktop.Username,
                        desktop.Password);

                    string sessionNumber = RemoteDesktopSession.GetConsoleSessionNumber(
                        desktop.TasklistOutputDirectory,
                        desktop.Name);

                    RemoteDesktopScreenshot.TakeRemotelyScreenshot(
                        psexecDirectory,
                        desktop.Name,
                        desktop.Username,
                        desktop.Password,
                        sessionNumber,
                        desktop.NircmdDirectory,
                        desktop.ScreenshotDirectory);

                    db.InsertScreenshot(
                        desktop.DesktopId,
                        adminConfigsScreenshotDirectory,
                        desktop.Name);
                });
            }
        }

        public static void Nircmd()
        {
            using (var psexecProcess = Process.Start(@"C:\Windows\System32\nircmd64\nircmd.exe", "savescreenshot \"C:\\Users\\Earth Jan\\Desktop\\test.jpg\""))
            {
                psexecProcess.WaitForExit();
            }
        }
    }
}
