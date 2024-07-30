using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDRP.Models;
using Newtonsoft.Json;
using System.Diagnostics;

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
                Console.WriteLine("JSON file not found: " + filePath);
                return;
            }

            // Read and print the JSON content for debugging
            string json = File.ReadAllText(filePath);
            Console.WriteLine("JSON content read: " + json);

            // Deserialize the JSON content
            gameList = JsonConvert.DeserializeObject<List<GameInfo>>(json);

        }

        // Verify if the name of the process is in the list of games
        public GameInfo IsGame(Process process)
        {
            // normalize process name
            string processName = process.ProcessName.Trim().ToLower();
            foreach (GameInfo game in gameList)
            {
                string gameName = game.ProcessName.Trim().ToLower();
                if (gameName.Equals(processName, StringComparison.OrdinalIgnoreCase))
                {
                    return game;
                }
                Trace.WriteLine($"{processName} != {gameName}");  
            }
            return null;
        }

    }
}
