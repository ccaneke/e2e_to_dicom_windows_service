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

namespace E2eToDicomWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configFilePath = Path.Combine(exeDir, "config.json");

            string configFileContent = await File.ReadAllTextAsync(configFilePath);
            ConfigFileModel configFileModel = JsonSerializer.Deserialize<ConfigFileModel>(configFileContent);

            DirectoryInfo directoryInfo = new DirectoryInfo(configFileModel.E2eDirectory);

            FileInfo[] fileInfos = directoryInfo.GetFiles(".E2E");

            IEnumerable<Process> startedProcessesQuery =
                from f in fileInfos
                let startedProcess = StartProcess(f.FullName)
                select startedProcess;

            List<Process> processes = startedProcessesQuery.ToList<Process>();

            int numStartedProcesses = processes.Count<Process>();

            _logger.LogInformation(processes.AllProcessesCompleted());

        }

        private static Process StartProcess(string fi)
        {

            Process e2eAnonymizer = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo("E2EFileInterpreter", arguments: fi);

            e2eAnonymizer.StartInfo = startInfo;

            e2eAnonymizer.Start();

            return e2eAnonymizer;
        }
    }
}
