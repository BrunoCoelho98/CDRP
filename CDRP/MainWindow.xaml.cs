using CDRP.Models;
using CDRP.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace CDRP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameMonitor gameMonitor;
        private WindowMonitor windowMonitor;
        private DispatcherTimer dispatcherTimer;
        private DiscordService discordService;
        private GameTimeTracker gameTimeTracker;
        public MainWindow()
        {
            InitializeComponent();
            gameMonitor = new GameMonitor();
            windowMonitor = new WindowMonitor();
            discordService = new DiscordService();
            gameTimeTracker = new GameTimeTracker(gameMonitor);
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(10); // Adjust as needed
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GameInfo activeGame = windowMonitor.CheckRunningGame();

            UpdateUI(activeGame);
            UpdateDiscordStatus(activeGame);

        }

        private void UpdateDiscordStatus(GameInfo game)
        {
            // Update the Rich Presence with your desired information
            discordService.UpdateDiscordStatus(game, gameTimeTracker.FormatTime(gameTimeTracker.PlayingTime), gameTimeTracker.FormatTime(gameTimeTracker.PausedTime), gameTimeTracker.FormatTime(gameTimeTracker.PlayingTime + gameTimeTracker.PausedTime), gameTimeTracker.isGameRunning());
        }

        private void UpdateUI(GameInfo game)
        {
            if (game == null)
            {
                // MessageBox.Show("No Active Game found");
                CurrentGameTextBox.Text = null;
                return;
            }
            UpdateGameName(game.Name);
            
            // IconComboBox.SelectedItem = game.Icon;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Button Clicked");

            GameInfo currentGame = windowMonitor.CheckRunningGame();
            if (currentGame != null)
            {
                Trace.WriteLine($"Current Game: {currentGame.Name}");
                Trace.WriteLine($"Current Game icon {currentGame.Icon}");
            }
            UpdateUI(currentGame);
            UpdateDiscordStatus(currentGame);
        }

        private void UpdateGameName(string gameName)
        {
            CurrentGameTextBox.Text = gameName;
            // MessageBox.Show($"Game: {gameName}");
        }

        protected override void OnClosed(EventArgs e)
        {
            discordService.Dispose();
            base.OnClosed(e);
        }
    }
}