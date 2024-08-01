using CDRP.Models;
using CDRP.Services;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;



// This class will be responsible for iterate through all processes, verify if the process is a game and return the game name
public class WindowMonitor
{
    public GameMonitor gameMonitor = new GameMonitor();

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    // Importa a função GetWindowText da API do Windows
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    // Importa a função IsWindowVisible da API do Windows
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    // Delegate para a função EnumWindows
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


    private List<WindowInfo> GetOpenWindows()
    {
        List<WindowInfo> openWindows = new List<WindowInfo>();

        EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                StringBuilder windowText = new StringBuilder(256);
                GetWindowText(hWnd, windowText, windowText.Capacity);

                if (!string.IsNullOrEmpty(windowText.ToString()))
                {
                    int processId;
                    GetWindowThreadProcessId(hWnd, out processId);

                    Process process = Process.GetProcessById(processId);
                    openWindows.Add(new WindowInfo
                    {
                        Title = windowText.ToString(),
                        ProcessName = process.ProcessName
                    });
                }
            }
            return true; // Continue enumerating windows
        }, IntPtr.Zero);

        return openWindows;
    }

    // Importa a função GetWindowThreadProcessId da API do Windows
    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


    public GameInfo CheckRunningGame()
        {
            List<WindowInfo> windowList = GetOpenWindows();

            foreach (WindowInfo window in windowList)
            {
                Trace.WriteLine($"{window.Title}");
                if (gameMonitor.IsGame(window) != null)
                {
                    return gameMonitor.IsGame(window);
                }
            }
            return null;
        }
}
