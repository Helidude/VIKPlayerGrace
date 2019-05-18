using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Torch.Commands;
using Torch.Commands.Permissions;
using Torch.Managers;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using Torch.API.Managers;

namespace VIKPlayerGrace
{
    public class Commands : CommandModule
    {

        [Command("grace", "Grant a player extended leave")]
        public void GraceAdd(string playerName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            if (IsDupe(playerId))
            {
                Context.Respond($"{playerName} already added");
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
