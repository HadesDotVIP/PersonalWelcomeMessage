using System.Collections.Generic;
using Newtonsoft.Json;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("PersonalWelcomeMessage", "Hades.VIP", "1.4.0")]
    [Description("Sends a private message to players when they join the server.")]

    class PersonalWelcomeMessage : CovalencePlugin
    {
        #region Configuration

        private Configuration config;

        public class Configuration
        {
            [JsonProperty("Welcome Message")]
            public string WelcomeMessage { get; set; }

            [JsonProperty("First Time Player Message")]
            public string FirstTimePlayerMessage { get; set; }

            [JsonProperty("Delay Before Sending Message (seconds)")]
            public float Delay { get; set; }
        }

        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(new Configuration
            {
                WelcomeMessage = "Hi {player_name}! Welcome back to {server_name}.",
                FirstTimePlayerMessage = "Hi {player_name}! Welcome to {server_name}. Our ruleset is heavily enforced and bans are issued daily. Ignorance is not an excuse, please read the rules and understand them BEFORE you begin playing: http://example.com | THANK YOU FOR PLAYING!",
                Delay = 5.0f
            }, true);
        }

        private void Init()
        {
            config = Config.ReadObject<Configuration>();
        }

        #endregion

        private void OnUserConnected(IPlayer player)
        {
            if (player.IsConnected)
            {
                string messageToSend = player.HasConnectedBefore ? config.WelcomeMessage : config.FirstTimePlayerMessage;
                messageToSend = messageToSend.Replace("{player_name}", player.Name).Replace("{server_name}", ConVar.Server.hostname);
                timer.Once(config.Delay, () => player.Message(messageToSend));
            }
        }
    }
}
