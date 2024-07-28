using CDRP.Services;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;



// This class will be responsible for iterate through all processes, verify if the process is a game and return the game name
public class WindowMonitor
{
    GameMonitor gameMonitor = new GameMonitor();

    public string CheckRunningGame()
    {
        Process[] processList = Process.GetProcesses();

        foreach (Process process in processList)
        {
            Trace.WriteLine($"Checking process: {process.ProcessName}");
            if (gameMonitor.IsGame(process))
            {
                return process.ProcessName;
            }
        }
        return null;
    }
}
