using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteScreenshot.MyProcess
{
    public class RemoteDesktopScreenshot
    {
        public static int TakeRemotelyScreenshot(string psexecDirectory, string machineName, string username, string password, string userSession, string nircmdDirectory, string screenshotDirectory)
        {
            string fullPsexecArg = $"\\\\{machineName} -u {username} -p {password} -i {userSession} \"{nircmdDirectory}\" savescreenshot \"{screenshotDirectory}{machineName}.jpg\"";

            using (var psexecProcess = Process.Start(psexecDirectory, fullPsexecArg))
            {
                psexecProcess.WaitForExit();
                
                return psexecProcess.ExitCode;
            }
        }

        public static string Screenshot()
        {
            string psexec = "C:\\Windows\\System32\\PSTools\\PsExec.exe";
            string computerArg = "\\\\vm1";
            string usernameArg = "-u virtualmachine1";
            string passwordArg = "-p virtualmachine1";
            string @switch = "-i 1";
            string rCommand = "\"C:\\Windows\\System32\\nircmd64\\nircmd.exe\"";
            string rCommandArg = "savescreenshot \"\\\\vmware-host\\Shared Folders\\Users\\Earth Jan\\Desktop\\test.jpg\"";

            string fullPsexecArg = $"{computerArg} {usernameArg} {passwordArg} {@switch} {rCommand} {rCommandArg}";

            using (var psexecProcess = Process.Start(psexec, fullPsexecArg))
            {
                psexecProcess.WaitForExit();
                return psexecProcess.ExitCode + " " + $"{psexec} {fullPsexecArg}";
            }
        }
    }
}
