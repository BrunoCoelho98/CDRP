using CDRP.Models;
using DiscordRPC;
using System.Diagnostics;

namespace CDRP.Services
{
    internal class DiscordService
    {
        private DiscordRpcClient client;
        private string clientId = Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID");
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

            // Start connection with Discord
            client.Initialize();
        }

        public void UpdateDiscordStatus(GameInfo game, string playingTime, string pausedTime, string totalTime, bool isRunning)
        {
            if (game != null)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = game.Name,
                    // total time = playing time + paused time
                    State = $"Playing for {totalTime}",
                    Assets = new Assets()
                    {
                        LargeImageKey = game.Icon,
                        LargeImageText = game.Name,
                        SmallImageKey = "cdrp_icon",
                        SmallImageText = $"Playing time: {playingTime}\n Paused time: {pausedTime}"
                    }
                });
            }
            else
            {
                client.ClearPresence();
            }
        }

        // Terminate Discord connection
        public void Dispose()
        {
            client.Dispose();
        }



    }
}
