using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RemoteScreenshot
{
    public class RemoteDesktopSession
    {
        /// <summary>
        /// gets the console session number of desktop that is written on a file consisting the tasklist.
        /// </summary>
        /// <param name="textFilePath">path of a file to where the tasklist of a desktop is written.</param>
        /// <param name="machineName">remote desktop name used to identify the name of the file.</param>
        /// <returns>session number</returns>
        public static string GetConsoleSessionNumber(string textFilePath, string machineName)
        {
            // reads a file consisting psexec tasklist, and
            // converts it to string.
            string output = File.ReadAllText($"{textFilePath}{machineName}.txt");

            int index = output.IndexOf("#") + 7;
            string currentChar = output[index].ToString();
            string sessionNumber = "";

            // matches single digit using ^\d$.
            var rx = new Regex(@"^\d$");

            // gets the console session number.
            while (rx.Matches(currentChar).Count > 0)
            {
                sessionNumber += output[index];
                index++;
                currentChar = output[index].ToString();
            }

            return sessionNumber;
        }

        /// <summary>
        /// Writes onto a file the output of a tasklist run on a remote desktop via psexec.
        /// </summary>
        /// <param name="textFilePath">path of the file to write onto. Make sure to have "\" as the last character.</param>
        /// <param name="machineName">remote desktop name</param>
        /// <param name="username">remote desktop username</param>
        /// <param name="password">remote desktop password</param>
        public static void WriteConsoleSessions(string textFilePath, string machineName, string username, string password)
        {
            var output = new StringBuilder();

            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "psexec.exe";
            p.StartInfo.Arguments = $"\\\\{machineName} -u {username} -p {password} -i tasklist /FI \"SESSIONNAME eq Console\" /FO LIST";

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

            // writes on a file the Process's stream until the process closes.
            StreamWriter sw = new StreamWriter($"{textFilePath}{machineName}.txt");
            try
            {
                //Write a line of text
                sw.WriteLine(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            p.WaitForExit();
            p.Close();

            // closes the file where Process's stream is written.
            sw.Close();
        }
    }
}