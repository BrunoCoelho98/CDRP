using CDRP.Models;
using CDRP.Services;
using Discord;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

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
        public MainWindow()
        {
            InitializeComponent();
            gameMonitor = new GameMonitor();
            windowMonitor = new WindowMonitor();
        }

        private void StartMonitoring()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(5); // Adjust as needed
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            string activeGame = windowMonitor.CheckRunningGame();
            if (activeGame != null)
            {
                UpdateUI(activeGame);
                // UpdateDiscordStatus(activeGame);
            }
        }

        private void UpdateUI(string game)
        {
            if (game == null)
            {
                MessageBox.Show("No Active Game found");
                CurrentGameTextBox.Text = null;
                return;
            }
            UpdateGameName(game);
            // RunningTimeTextBox.Text = DateTime.Now.ToString(); // Placeholder for actual running time
            // IconComboBox.SelectedItem = game.Icon;
            // SelectedIconImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/{game.Icon}"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Button Clicked");
            UpdateUI(windowMonitor.CheckRunningGame());
        }

        private void UpdateGameName(string gameName)
        {
            CurrentGameTextBox.Text = gameName;
            MessageBox.Show($"Game: {gameName}");
        }
    }
}