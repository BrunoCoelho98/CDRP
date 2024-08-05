using CDRP.Models;
using CDRP.Services;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;

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
        // Get the handle of the foreground window (the window that is currently active)
        IntPtr foregroundWindow = GetForegroundWindow();

        // If no window is currently active, return from the function
        if (foregroundWindow == IntPtr.Zero)
            return;

        // Get detailed information about the active window
        WindowInfo activeWindow = GetWindowInfo(foregroundWindow);

        // If the active window information is not null, proceed
        if (activeWindow != null)
        {
            // Check if the active window corresponds to a game from the game monitor
            GameInfo game = gameMonitor.IsGame(activeWindow);

            // If the active window is a game and its title is different from the previously stored game window title
            if (game != null && activeWindow.Title != currentGameWindowTitle)
            {
                // Update the current game window title to the new game's title
                currentGameWindowTitle = activeWindow.Title;

                isGameInFocus = true;

                playingStopwatch.Start();
                pausedStopwatch.Stop();
            }
            // If the active window is not a game but the previous window was a game
            else if (game == null && isGameInFocus)
            {
                isGameInFocus = false;

                playingStopwatch.Stop();
                pausedStopwatch.Start();
            }

            // The following lines are commented out, but they would log the formatted playing and paused times to the console
            // Trace.WriteLine($"Playing Time: {FormatTime(PlayingTime)}");
            // Trace.WriteLine($"Paused Time: {FormatTime(PausedTime)}");
        }
    }


    public string FormatTime(TimeSpan time)
    {
        return $"{time.Hours}h{time.Minutes}m{time.Seconds}s";
    }

    public bool isGameRunning()
    {
        return playingStopwatch.IsRunning;
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