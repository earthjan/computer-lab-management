using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteScreenshot
{
    public class Shutdown
    { 
        public static void RemotelyShutdown(string psshutdownDirectory, string machineName, string username, string password) 
        {
            string fullPsshutdowncArg = $@"\\{machineName} -u {username} -p {password} -s";
            
            using (var psshutdownProcess = Process.Start(psshutdownDirectory, fullPsshutdowncArg))
            {
                psshutdownProcess.WaitForExit();            
            }
        }
    }
}
