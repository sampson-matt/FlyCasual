
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class MandalorianResistancePilot : GauntletFighter
        {
            public MandalorianResistancePilot() : base()
            {

                PilotInfo = new PilotCardInfo
                (
                    "Mandalorian Resistance Pilot",
                    2,
                    53,
                    pilotTitle: "Clan Loyalist",
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Rebel
                );

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}