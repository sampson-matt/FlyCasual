using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class GoldSquadronTrooper : V19TorrentStarfighter
    {
        public GoldSquadronTrooper()
        {
            PilotInfo = new PilotCardInfo(
                "Gold Squadron Trooper",
                2,
                25
            );
        }
    }
}