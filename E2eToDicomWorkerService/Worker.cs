using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace E2eToDicomWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        // Fixed Json property name in config file so that it matches property name in the .Net class
        // Fixed DirectoryInfo.GetFiles search pattern (i.e. search string).
        // Fixed: Debug StartProcess method
        // Fixed: Save all images from each parse of a .E2E file to a unique folder named after the same GUID used for the name of the
        // anonymized e2es directory and dicom output directory.
        // Fixed: Wait for all E2EFileInterpreter processes to finish, and log a message to the application output window.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Debug

            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configFilePath = Path.Combine(exeDir, "config.json");

            string configFileContent = await File.ReadAllTextAsync(configFilePath);
            ConfigFileModel configFileModel = JsonSerializer.Deserialize<ConfigFileModel>(configFileContent);

            DirectoryInfo directoryInfo = new DirectoryInfo(configFileModel.E2eDirectory);

            FileInfo[] fileInfos = directoryInfo.GetFiles("*.E2E");

            IEnumerable<Process> startedProcessesQuery =
                from f in fileInfos
                let startedProcess = StartProcess(f.FullName, configFileModel)
                select startedProcess;

            List<Process> processes = startedProcessesQuery.ToList<Process>();

            int numStartedProcesses = processes.Count<Process>();

            _logger.LogInformation(processes.AllProcessesCompleted());

        }

        private static Process StartProcess(string fi, ConfigFileModel settings)
        {
            fi = "\"" + fi + '\"';

            Process e2eAnonymizer = new Process();

            string exeDirectoryPath = settings.ExecutableDirectoryPath;

            string exePath = exeDirectoryPath + @"\E2EFileInterpreter.exe";
            
            ProcessStartInfo startInfo = new ProcessStartInfo(/*"E2EFileInterpreter.exe"*/exePath, arguments: fi);
            //startInfo.WorkingDirectory = exeDirectoryPath;//GetE2EFileInterpreterPath();

            /*
             * For MacOS
            startInfo.UseShellExecute = true;

            string wd = startInfo.WorkingDirectory;

            startInfo.WorkingDirectory = GetE2EFileInterpreterPath();*/
            //startInfo.Work
            startInfo.UseShellExecute = false;

            e2eAnonymizer.StartInfo = startInfo;

            bool started = e2eAnonymizer.Start();

            return e2eAnonymizer;
        }

        public static string GetE2EFileInterpreterPath()
        {
            // Environment must be the environment of the login non-interactive shell, this environment is set only when you first login
            // to MacOS.
            string systemPath = Environment.GetEnvironmentVariable("PATH");

            //Process echo = new Process();

            //echo.StartInfo = new ProcessStartInfo("echo", "$PATH");

            //echo.StartInfo.UseShellExecute = true;

            //var test = echo.StartInfo.EnvironmentVariables;
            //var test2 = echo.StartInfo.Environment;
            //echo.StartInfo.RedirectStandardOutput/*RedirectStandardError*/ = true;

            //echo.Start();

            //StreamReader sr = echo.StandardOutput/*StandardError*/;

            //string output = sr.ReadToEnd();

            /*string[] paths = systemPath.Split(':');*/

            OSPlatform os = OSPlatform.Windows;

            string[] paths = RuntimeInformation.IsOSPlatform(os) ? systemPath.Split(';') : systemPath.Split(':');

            string test = "C:\\Users\\Christopher Aneke\\Desktop\\exeDirectories\\e2eFileInterpreterPublish";
            bool contains = test.Contains("E2EFileInterpreter", StringComparison.CurrentCultureIgnoreCase/*InvariantCultureIgnoreCase*//*OrdinalIgnoreCase*/) /*test.IndexOf("E2EFileInterpreter", StringComparison.OrdinalIgnoreCase) >= 0*/;

            string e2eFileInterpreterPath = paths.Single<string>(path => path.Contains("E2EFileInterpreter", StringComparison.InvariantCultureIgnoreCase));

            return e2eFileInterpreterPath;
        }
    }
}
