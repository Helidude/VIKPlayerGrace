using Sandbox.Game.World;
using System;

namespace VIKPlayerGrace
{
    public static class SessionPatches
    {
        /// <summary>
        /// Loads all players from config file and applies to the running session 
        /// </summary>
        public static void ApplySession()
        {
            if (GraceControl.Plugin.Config.PlayersOnLeave == null || MySession.Static == null)
                return;

            PlayersList.PlayerList.Clear();
            foreach (var player in GraceControl.Plugin.Config.PlayersOnLeave) 
            {
                // Add Players from file to List<PlayerData>
                PlayersList.PlayerList.Add(new PlayerData
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName,
                    GraceGrantedAt = player.GraceGrantedAt
                });
            }

            // Process List and apply to session
            foreach (MyIdentity identity in MySession.Static.Players.GetAllIdentities())
            {
                foreach (var playerData in PlayersList.PlayerList)
                {
                    if (playerData.PlayerId == identity.IdentityId)
                        identity.LastLoginTime = DateTime.Now;
                }
            }
        }
    }
}
