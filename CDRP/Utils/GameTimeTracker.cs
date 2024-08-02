using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;
using CDRP.Models;
using CDRP.Services;

public class GameTimeTracker
{
    public GameMonitor gameMonitor;
    private DispatcherTimer dispatcherTimer;
    private Stopwatch playingStopwatch;
    private Stopwatch pausedStopwatch;
    private DiscordService discordService;
    private string currentGameWindowTitle;
    private bool isGameInFocus;

    public TimeSpan PlayingTime => playingStopwatch.Elapsed;
    public TimeSpan PausedTime => pausedStopwatch.Elapsed;

    public GameTimeTracker(GameMonitor monitor)
    {
        gameMonitor = monitor;
        playingStopwatch = new Stopwatch();
        pausedStopwatch = new Stopwatch();
        dispatcherTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(10) // Check every 10 seconds
        };
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Start();
    }

    private void DispatcherTimer_Tick(object sender, EventArgs e)
    {
        IntPtr foregroundWindow = GetForegroundWindow();
        if (foregroundWindow == IntPtr.Zero)
            return;

        WindowInfo activeWindow = GetWindowInfo(foregroundWindow);

        if (activeWindow != null)
        {
            GameInfo game = gameMonitor.IsGame(activeWindow);

            if (game != null && activeWindow.Title != currentGameWindowTitle)
            {
                currentGameWindowTitle = activeWindow.Title;
                isGameInFocus = true;
                playingStopwatch.Start();
                pausedStopwatch.Stop();
            }
            else if (game == null && isGameInFocus)
            {
                isGameInFocus = false;
                playingStopwatch.Stop();
                pausedStopwatch.Start();
            }

            Trace.WriteLine($"Playing Time: {FormatTime(PlayingTime)}");
            Trace.WriteLine($"Paused Time: {FormatTime(PausedTime)}");
            discordService.UpdateDiscordStatus(game, FormatTime(PlayingTime), FormatTime(PausedTime));
        }
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{time.Hours}h{time.Minutes}m{time.Seconds}s";
    }

    private WindowInfo GetWindowInfo(IntPtr hwnd)
    {
        StringBuilder title = new StringBuilder(256);
        if (GetWindowText(hwnd, title, title.Capacity) > 0)
        {
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process process = Process.GetProcessById((int)pid);
            return new WindowInfo { Title = title.ToString(), ProcessName = process.ProcessName };
        }
        return null;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}