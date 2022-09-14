/* Copyright (C) Interneuron, Inc - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Chukwuemezie Aneke <ccanekedev@gmail.com>, May 2020
 */

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
