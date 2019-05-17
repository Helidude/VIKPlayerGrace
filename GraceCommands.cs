using NLog;
using System;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace VIKPlayerGrace
{
    class GraceCommands : CommandModule
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public GracePlugin Plugin => (GracePlugin)Context.Plugin;

        [Command("grace", "Grant a player extended leave")]
        [Permission(MyPromoteLevel.SpaceMaster)]
        public void GraceAdd(string playerName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            if (IsDupe(playerId))
            {
                Context.Respond($"{playerName} already added");
                return;
            }
            else
            {
                PlayersList.PlayerList.Add(new PlayerData
                {
                    PlayerId = playerId,
                    PlayerName = playerName,
                    GraceGrantedAt = DateTime.Now
                });

                GraceControl.ConfWriter(PlayersList.PlayerList);
                Context.Respond($"{playerName} successfully added");
            }
        }

        private bool IsDupe(long pid)
        {
            if (PlayersList.PlayerList == null)
                return false;

            foreach (var playerData in PlayersList.PlayerList)
            {
                if (playerData.PlayerId == pid)
                    return true;
            }

            return false;
        }
    }
}
