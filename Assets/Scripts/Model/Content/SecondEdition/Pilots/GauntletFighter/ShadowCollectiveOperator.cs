
using Content;
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
                    53,
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Illicit },
                    factionOverride: Faction.Scum
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}