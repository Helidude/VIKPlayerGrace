using NLog;
using System;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace VIKPlayerGrace
{
    public class GraceCommands : CommandModule
    {
        [Command("grace add", "Grant a player extended leave")]
        [Permission(MyPromoteLevel.SpaceMaster)]
        public void GraceAdd(string playerName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            if (DupeCheck.IsDupe(playerId))
            {
                Context.Respond($"{playerName} already added");
                return;
            }

            PlayerControl.Add(playerName);
            Context.Respond($"{playerName} successfully added");
        }

        [Command("grace remove", "Revoke players extended leave")]
        [Permission(MyPromoteLevel.SpaceMaster)]
        public void GraceRemove(string playerName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            PlayerControl.Remove(playerId);
        }
    }
}
