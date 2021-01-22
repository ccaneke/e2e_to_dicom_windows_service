using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace E2eToDicomWorkerService
{
    public static class Extensions
    {
        public static string AllProcessesCompleted(this IEnumerable<Process> processes)
        {
            IEnumerator<Process> enumerator = processes.GetEnumerator();

            while (enumerator.MoveNext())
            {
                enumerator.Current.WaitForExit();
            }

            string message = "All instances of the E2EFileInterpreter program have completed";

            return message;
        }
    }
}
