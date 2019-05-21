using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIKPlayerGrace
{
    class PlayerControl
    {
        public static void Remove(long pid)
        {
            var itemToRemove = PlayersList.PlayerList.SingleOrDefault(r => r.PlayerId == pid);
            if (itemToRemove != null)
                PlayersList.PlayerList.Remove(itemToRemove);

            GraceControl.ConfWriter(PlayersList.PlayerList);
        }

        public static void Add(string pName)
        {
            var playerId = GetFromSession.GetPlayerIdByName(pName);
            if (DupeCheck.IsDupe(playerId))
                return;

            PlayersList.PlayerList.Add(new PlayerData
            {
                PlayerId = playerId,
                PlayerName = pName,
                GraceGrantedAt = DateTime.Now
            });

            GraceControl.ConfWriter(PlayersList.PlayerList);
        }
    }
}
