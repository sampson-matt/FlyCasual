
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class ShadowCollectiveOperator : GauntletFighter
        {
            public ShadowCollectiveOperator() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Shadow Collective Operator",
                    1,
                    52,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Illicit },
                    factionOverride: Faction.Scum
                );

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/8/8f/Shadowcollectiveoperator.png";
            }
        }
    }
}