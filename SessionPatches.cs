using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIKPlayerGrace
{
    public static class SessionPatches
    {

        public static void Refresh()
        {         
            PlayersList.PlayerList.Clear();
            foreach (var player in GraceControl.Plugin.Config.PlayersOnLeave) // Loads Players from config file
            {
                PlayersList.PlayerList.Add(new PlayerData // Add Players from file to List
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName,
                    GraceGrantedAt = player.GraceGrantedAt,
                    GracePeriodTo = player.GracePeriodTo
                });
            }

            // Update LastLoginTime for users in List. Also check if users has expired
            foreach (MyIdentity identity in MySession.Static.Players.GetAllIdentities())
            {
                foreach (var playerData in PlayersList.PlayerList)
                {
                    if (playerData.PlayerId == identity.IdentityId)
                        identity.LastLoginTime = playerData.GracePeriodTo;

                    if (playerData.GracePeriodTo > DateTime.Now)
                        PlayersList.PlayerList.Remove(playerData);
                }
            }
        }

        public static void Remove(long pid)
        {
            // Set LastLoginTime to DateTime.now if player is deleted. 
            foreach (MyIdentity identity in MySession.Static.Players.GetAllIdentities())
            {
                if (pid == identity.IdentityId)
                    identity.LastLoginTime = DateTime.Now; 
            }
        }

        public static void AutoRemove()
        {
            // Checks if any of the players grace period has expired and removes them from the list 
            if (PlayersList.PlayerList == null)
                return;

            foreach (var playerData in PlayersList.PlayerList)
            {
                if (playerData.GracePeriodTo > DateTime.Now)
                {
                    PlayersList.PlayerList.Remove(playerData);
                }
            }

            GraceControl.Plugin.Config.PlayersOnLeave.; // Clear the config file for Players On Leave
            foreach (var playerData in PlayersList.PlayerList) // Add all the reminding players back to config file
            {
                GraceControl.Plugin.Config.PlayersOnLeave.Add(playerData);
            }

            GraceControl.Plugin.Save(); // Save Config file
            Refresh(); // Apply the changes to session manager
        }
    }
}
