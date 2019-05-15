﻿using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIKPlayerGrace
{
     public class GetFromSession
    {
        public static List<MyIdentity> GetAllPlayers()
        {
            // Populate the list when plugin loads
            var idents = MySession.Static.Players.GetAllIdentities().ToList();
            var npcs = MySession.Static.Players.GetNPCIdentities().ToList();

            return idents.Where(i => !npcs.Any(n => n == i.IdentityId)).ToList();
        }

        public static long GetPlayerIdByName(string name)
        {
            if (!long.TryParse(name, out var id))
            {
                foreach (var identity in MySession.Static.Players.GetAllIdentities())
                {
                    if (identity.DisplayName == name)
                    {
                        return identity.IdentityId;
                    }
                }
            }

            return 0;
        }
    }
}
