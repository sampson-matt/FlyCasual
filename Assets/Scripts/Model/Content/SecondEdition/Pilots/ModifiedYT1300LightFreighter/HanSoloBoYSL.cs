using BoardTools;
using System;
using System.Collections.Generic;
using Content;
using UpgradesList.SecondEdition;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ModifiedYT1300LightFreighter
    {
        public class HanSoloBoYSL : ModifiedYT1300LightFreighter
        {
            public HanSoloBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Han Solo",
                    6,
                    105,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.HanSoloBoYPilotAbility),
                    charges: 4,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Crew,
                        UpgradeType.Illicit,
                        UpgradeType.Title,
                        UpgradeType.Configuration
                    },
                    isStandardLayout: true
                );

                MustHaveUpgrades.Add(typeof(ChewbaccaBoY));
                MustHaveUpgrades.Add(typeof(RiggedCargoChute));
                MustHaveUpgrades.Add(typeof(MillenniumFalcon));
                MustHaveUpgrades.Add(typeof(L337sProgramming));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/5/5e/Hansolo-battleofyavin.png";

                PilotNameCanonical = "hansolo-battleofyavin-sl";
            }
        }
    }
}
