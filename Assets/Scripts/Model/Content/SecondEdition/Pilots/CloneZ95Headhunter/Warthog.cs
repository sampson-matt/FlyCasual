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
        public class Warthog : CloneZ95Headhunter
        {
            public Warthog() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Warthog\"",
                    3,
                    29,
                    pilotTitle: "Veteran of Kadavo",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.WarthogAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                PilotNameCanonical = "warthog-clonez95headhunter";
            }
        }
    }
}