using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Message;
using CDRP.Models;

namespace CDRP.Services
{
    internal class DiscordService
    {
        private DiscordRpcClient client;
        private string clientId = "1267241699982315530";

        public DiscordService()
        {
            
            client = new DiscordRpcClient(clientId);

            client.OnReady += (sender, e) =>
            {
                Trace.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Trace.WriteLine($"Received Update! {e.Presence}");
            };

            client.Initialize();
        }

        public void UpdateDiscordStatus(GameInfo game, string playingTime, string pausedTime)
        {
            if (game != null)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = game.Name,
                    State = "Running",
                    Assets = new Assets()
                    {
                        LargeImageKey = game.Icon,
                        LargeImageText = game.Name,
                        SmallImageKey = "cdrp_icon",
                        SmallImageText = $"Playing: {playingTime}\nPaused: {pausedTime}"
                    }
                });
            }
            else
            {
                client.ClearPresence();
            }
        }


        public void Dispose()
        {
            client.Dispose();
        }



    }
}
