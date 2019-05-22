using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIKPlayerGrace
{
    class DupeCheck
    {
        /// <summary>
        /// Check if player is already added
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static bool IsDupe(long pid)
        {
            if (pid == 0)
                return true;

            foreach (var playerData in PlayersList.PlayerList)
            {
                if (playerData.PlayerId == pid)
                    return true;
            }

            return false;
        }
    }
}
