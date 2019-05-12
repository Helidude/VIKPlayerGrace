using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Engine.Voxels;

namespace VIKPlayerGrace
{
    public static class SessionPatches
    {
        /// <summary>
        /// Loads all players on leave from config file and applies it to the running session 
        /// </summary>
        public static void Refresh()
        {         
            PlayersList.PlayerList.Clear();
            foreach (var player in GraceControl.Plugin.Config.PlayersOnLeave) 
            {
                // Add Players from file to List<PlayerData>
                PlayersList.PlayerList.Add(new PlayerData
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName,
                    GraceGrantedAt = player.GraceGrantedAt,
                    GracePeriodTo = player.GracePeriodTo
                });
            }

            // Process List and apply to session
            foreach (MyIdentity identity in MySession.Static.Players.GetAllIdentities())
            {
                foreach (var playerData in PlayersList.PlayerList)
                {
                    if (playerData.PlayerId == identity.IdentityId)
                        identity.LastLoginTime = playerData.GracePeriodTo;
                }
            }
        }

        /// <summary>
        /// Set LastLoginTime to DateTime.now if player is deleted. 
        /// </summary>
        public static void Remove(long pid)
        {
            foreach (MyIdentity identity in MySession.Static.Players.GetAllIdentities())
            {
                if (pid == identity.IdentityId)
                    identity.LastLoginTime = DateTime.Now; 
            }
        }

        /// <summary>
        /// Checks if any of the players grace period has expired and removes them from the list 
        /// </summary>
        public static void AutoRemove()
        {
            // Load Players from file
            PlayersList.PlayerList.Clear();
            foreach (var player in GraceControl.Plugin.Config.PlayersOnLeave)
            {
                // Add Players from file to List<PlayerData>
                PlayersList.PlayerList.Add(new PlayerData
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName,
                    GraceGrantedAt = player.GraceGrantedAt,
                    GracePeriodTo = player.GracePeriodTo
                });
            }

            if (PlayersList.PlayerList == null)
                return;

            foreach (var playerData in PlayersList.PlayerList.ToList())
            {
                if (playerData.GracePeriodTo > DateTime.Now)
                {
                    PlayersList.PlayerList.Remove(playerData);
                }
            }

            // Add all the reminding players back to config file
            GraceControl.ConfWriter(PlayersList.PlayerList);

            // Apply the changes to session manager
            Refresh();
        }
    }
}
