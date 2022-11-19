
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
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Mandalorian Resistance Pilot",
                    2,
                    53,
                    pilotTitle: "Clan Loyalist",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ChopperPilotAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Rebel
                );

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/a/a6/Mandalorianresistancepilot.png";
            }
        }
    }
}