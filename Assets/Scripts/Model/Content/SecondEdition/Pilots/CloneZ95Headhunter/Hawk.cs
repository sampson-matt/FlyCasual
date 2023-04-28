using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.CloneZ95Headhunter
    {
        public class Hawk : CloneZ95Headhunter
        {
            public Hawk() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Hawk\"",
                    4,
                    25,
                    pilotTitle: "Valkyrie 2929",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.HawkAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent }
                );

                PilotNameCanonical = "hawk-clonez95headhunter";
            }
        }
    }
}