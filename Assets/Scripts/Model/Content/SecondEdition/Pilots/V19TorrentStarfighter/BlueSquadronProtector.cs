using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class BlueSquadronProtector : V19TorrentStarfighter
    {
        public BlueSquadronProtector()
        {
            PilotInfo = new PilotCardInfo(
                "Blue Squadron Protector",
                3,
                26,
                extraUpgradeIcon: UpgradeType.Talent
            );
        }
    }
}