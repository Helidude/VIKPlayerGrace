using NLog;
using System;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace VIKPlayerGrace
{
    public class GraceCommands : CommandModule
    {
        public static readonly Logger Log = LogManager.GetLogger("GraceCommands");

        [Command("grace add", "Grant a player extended leave")]
        [Permission(MyPromoteLevel.SpaceMaster)]
        public void GraceAdd(string playerName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            if (DupeCheck.IsDupe(playerId))
            {
                Context.Respond($"{playerName} already added or does not exist");
                return;
            }

            PlayerControl.Add(playerName);
            Context.Respond($"{playerName} successfully added");
            Log.Info($"{playerName} successfully added");
        }

        [Command("grace remove", "Revoke players extended leave")]
        [Permission(MyPromoteLevel.SpaceMaster)]
        public void GraceRemove(string playerName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            if (DupeCheck.IsDupe(playerId))
            {             
                PlayerControl.Remove(playerId);
                Context.Respond($"Player {playerName} successfully removed");
                Log.Info($"Player {playerName} successfully removed");
                return;
            }

            Context.Respond($"Player {playerName} not found");
        }
    }
}
