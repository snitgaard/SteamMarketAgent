using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SteamMarketAgent
{
    class BuildAgent
    {
        public string Id { get; private set; }
        public string BuildPath { get; private set; }

        public int TimeIntervalSeconds { get; set; }

        private object _agentLock;

        private readonly Thread _t;
        private bool _done;

        public BuildAgent(string id, string solutionPath, int seconds, object agentLock)
        {
            Id = id;
            BuildPath = solutionPath;
            TimeIntervalSeconds = seconds;
            _agentLock = agentLock;
            _done = false;

            _t = new Thread(Run);
            _t.Start();
        }

        public void Run()
        {
            while (!_done)
            {
                try
                {
                    lock (_agentLock)
                    {
                        var exitCode = Do("build", BuildPath);
                        if (exitCode == 0)
                            Do("test", BuildPath);
                    }
                    Thread.Sleep(TimeIntervalSeconds * 1000);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Thread interrupted");
                    // just wake up
                }
            }
            Console.WriteLine("Agent stopped");
        }

        private int Do(string dotnetCommand, string buildPath)
        {
            Console.WriteLine($"{Id} {dotnetCommand}...");
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "dotnet",
                    WorkingDirectory = buildPath,
                    Arguments = dotnetCommand
                }
            };
            Console.WriteLine("Start process...");
            p.Start();
            p.WaitForExit();
            Console.WriteLine("Process terminated");

            Console.WriteLine("Writing to log file...");
            string logPath = buildPath + @"\logs\";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            using (var output = File.AppendText(logPath + $"{Id}.log"))
            {
                output.WriteLine($"Time: {p.StartTime}");
                output.WriteLine(p.StandardOutput.ReadToEnd());
                output.WriteLine();
            }
            var exitCode = p.ExitCode;
            p.Close();
            p.Dispose();
            Console.WriteLine("Done");
            return exitCode;
        }

        public void Cancel()
        {
            _done = true;
            if (_t.ThreadState != System.Threading.ThreadState.Running)
                _t.Interrupt();
        }
    }
}
