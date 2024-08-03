using CDRP.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace CDRP.Services
{
    public class GameMonitor
    {
        public List<GameInfo> gameList { get; set; }

        public GameMonitor()
        {
            LoadGameList();
        }

        private void LoadGameList()
        {
            string filePath = "games.json"; // Use the relative path to the JSON file

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Trace.WriteLine("JSON file not found: " + filePath);
                return;
            }

            // Read and print the JSON content for debugging
            string json = File.ReadAllText(filePath);
            Trace.WriteLine("JSON content read: " + json);

            // Deserialize the JSON content
            gameList = JsonConvert.DeserializeObject<List<GameInfo>>(json);

        }

        // Verify if the name of the window is in the list of games
        public GameInfo IsGame(WindowInfo window)
        {
            // normalize process name
            foreach (GameInfo game in gameList)
            {
                string gameName = game.WindowTitle.Trim().ToLower();
                if (gameName.Equals(window.Title.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    string gameProcess = game.ProcessName.Trim().ToLower();
                    string windowProcess = window.ProcessName.Trim().ToLower();
                    // Compare the process name to make sure that it is the actual game running and not a random window with the same name
                    if (gameProcess.Equals(windowProcess, StringComparison.OrdinalIgnoreCase))
                    {
                        return game;
                    }
                    Trace.WriteLine($"{windowProcess} != {gameProcess}");
                }
            }
            return null;
        }

    }
}
