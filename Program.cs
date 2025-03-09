using System.Diagnostics;

class SimpleSystemMonitor
{
    /// <summary>
    /// Path of the log file saved in the user's documents folder with a unique timestamp in the filename.
    /// </summary>
    static string logFile = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        $"SimpleSystemMonitorLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
    );

    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    static void Main()
    {
        // Display to the user where the log file will be saved.
        DisplayLogFilePath();
        // Log the start of system monitoring.
        LogData("=== System Monitoring Started ===");
        // Start the continuous monitoring cycle.
        while (true)
        {
            RunMonitoringCycle();
        }
    }

    /// <summary>
    /// Runs a system monitoring cycle.
    /// Clears the console, displays the header, monitors CPU and processes, and waits before repeating.
    /// </summary>
    static void RunMonitoringCycle()
    {
        Console.Clear(); // Clear the console to update the display.
        DisplayHeader(); // Show the header information.
        MonitorCpuUsage(); // Monitor and log CPU usage.
        MonitorTopProcesses(); // Monitor and log top memory-consuming processes.
        DisplayExitMessage(); // Show exit instructions.
        Thread.Sleep(5000); // Wait for 5 seconds before the next cycle.
    }

    /// <summary>
    /// Displays the path of the log file to the user.
    /// </summary>
    static void DisplayLogFilePath()
    {
        // Inform the user where the log file will be saved.
        Console.WriteLine("Log file will be saved at: " + logFile);
    }

    /// <summary>
    /// Displays header information for the monitoring cycle.
    /// </summary>
    static void DisplayHeader()
    {
        // Show the main title of the application.
        Console.WriteLine("Simple System Monitoring Application");
        // Remind the user of the log file location.
        Console.WriteLine("Log file: " + logFile);
    }

    /// <summary>
    /// Monitors overall CPU usage, displays it, and logs the data.
    /// </summary>
    static void MonitorCpuUsage()
    {
        // Calculate overall CPU usage.
        double overallCpu = GetOverallCpuUsage();
        // Prepare a string to display CPU usage.
        string cpuUsageText = $"Total CPU Usage: {overallCpu:F2}%";
        Console.WriteLine(cpuUsageText); // Display CPU usage.
        LogData(cpuUsageText); // Log CPU usage.
    }

    /// <summary>
    /// Retrieves and displays the top 10 processes based on memory usage.
    /// </summary>
    static void MonitorTopProcesses()
    {
        Console.WriteLine("\nTop Processes by Memory Usage:");
        LogData("Top Processes by Memory Usage:");
        // Retrieve a list of processes with their names and memory usage, sorted by memory consumption.
        var processes = Process.GetProcesses()
            .Where(p => !string.IsNullOrEmpty(p.ProcessName))
            .Select(p => new
            {
                Name = p.ProcessName,
                Memory = SafeMemoryUsage(p)
            })
            .OrderByDescending(p => p.Memory)
            .Take(10);
        // Loop through each process and display and log its details.
        foreach (var p in processes)
        {
            string procInfo = $"{p.Name} - {p.Memory:F2} MB";
            Console.WriteLine(procInfo);
            LogData(procInfo);
        }
    }

    /// <summary>
    /// Displays exit instructions for the application.
    /// </summary>
    static void DisplayExitMessage()
    {
        // Inform the user to press Ctrl+C to exit.
        Console.WriteLine("\nPress Ctrl+C to exit...");
    }

    /// <summary>
    /// Calculates overall CPU usage by sampling processor time over a short interval.
    /// </summary>
    /// <returns>CPU usage as a percentage.</returns>
    static double GetOverallCpuUsage()
    {
        // Record initial time and total CPU time.
        var startTime = DateTime.UtcNow;
        double startCpu = Process.GetProcesses().Sum(p => SafeCpuTime(p));
        Thread.Sleep(100); // Wait a short period to measure CPU usage difference.
        // Record final time and total CPU time.
        var endTime = DateTime.UtcNow;
        double endCpu = Process.GetProcesses().Sum(p => SafeCpuTime(p));
        // Calculate CPU time used during the interval.
        double cpuUsedMs = endCpu - startCpu;
        double totalMsPassed = (endTime - startTime).TotalMilliseconds * Environment.ProcessorCount;
        return (cpuUsedMs / totalMsPassed) * 100;
    }

    /// <summary>
    /// Safely retrieves the total processor time (in milliseconds) for a given process.
    /// </summary>
    /// <param name="p">The process to query.</param>
    /// <returns>Processor time in milliseconds, or 0 if access is denied.</returns>
    static double SafeCpuTime(Process p)
    {
        try
        {
            return p.TotalProcessorTime.TotalMilliseconds;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Safely retrieves memory usage (in MB) for a given process.
    /// </summary>
    /// <param name="p">The process to query.</param>
    /// <returns>Memory usage in megabytes, or 0 if access is denied.</returns>
    static double SafeMemoryUsage(Process p)
    {
        try
        {
            return p.PrivateMemorySize64 / (1024.0 * 1024.0);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Logs a message to the log file with the current timestamp.
    /// </summary>
    /// <param name="message">The message to log.</param>
    static void LogData(string message)
    {
        try
        {
            // Append the log message to the file.
            using var writer = new StreamWriter(logFile, true);
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
        catch (Exception ex)
        {
            // If logging fails, display an error message.
            Console.WriteLine("Logging error: " + ex.Message);
        }
    }
}
