using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace RemoteScreenshot.MyProcess
{
    public class AppMonitoring
    {
        public List<Process> getProcesses()
        {
            var processes = new List<Process>();

            Process[] proc = Process.GetProcesses();
            foreach (Process p in proc)
            {
                try
                {
                    var time = p.StartTime;
                    processes.Add(p);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return processes;
        }
    }
}