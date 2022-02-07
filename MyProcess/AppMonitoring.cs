using System;
using System.Diagnostics;
using System.Text;

namespace RemoteScreenshot.MyProcess
{
    public class AppMonitoring
    {
        /// <summary>
        /// gets the currently running apps on a remote desktop using psexec with a powershell script.
        /// </summary>
        /// <param name="scriptPath">the path of the powershell script that includes the file and its extension.</param>
        /// <param name="machineName">the remote computer's desktop name.</param>
        /// <param name="username">the remote computer account username</param>
        /// <param name="password">the remote computer account password</param>
        /// <returns></returns>
        public static string GetCurrentlyRunningApps(string scriptPath, string machineName, string username, string password)
        {
            try
            {
                var output = new StringBuilder();

                var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "psexec.exe";
                p.StartInfo.Arguments = $"\\\\{machineName} -u {username} -p {password} -i cmd /c \"powershell -noninteractive -file \"{scriptPath}\"\"";

                // in asynchronous way, reads the Process's stream, and writes it on StringBuilder.
                p.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        output.Append("\n" + e.Data);
                    }
                });

                p.Start();

                // asynchronously reads the standard output of the spawned process.
                // raises OutputDataReceived events for each line of output.
                p.BeginOutputReadLine();
                p.WaitForExit();
             
                return output.ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return $"Operation failed: couldn't get the currently running apps on {machineName}.";
            }
        }
    }
}