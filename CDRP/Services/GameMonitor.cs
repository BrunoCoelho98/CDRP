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
    class GameMonitor
    {
        public List<GameInfo> gameList { get; private set; }

        public GameMonitor()
        {
            LoadGameList();
        }

        private void LoadGameList()
        {
            try
            {
                string json = File.ReadAllText("games.json");
                gameList = JsonConvert.DeserializeObject<List<GameInfo>>(json);
                Trace.WriteLine("Game list loaded");

            }
            catch (Exception e)
            {
                Trace.WriteLine("Error loading game list: " + e.Message);
            }
        }

        // Verify if the name of the process is in the list of games
        public bool IsGame(Process process)
        {
            foreach(GameInfo game in gameList)
            {
                if (game.ProcessName.Equals(process.ProcessName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
