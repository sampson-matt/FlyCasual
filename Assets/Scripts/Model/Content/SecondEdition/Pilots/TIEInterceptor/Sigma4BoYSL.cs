using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using System.Collections.Generic;
using ActionsList;
using Content;
using System;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma4BoYSL : TIEInterceptor
        {
            public Sigma4BoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma4",
                    4,
                    52,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma4Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Tech
                    },
                    isStandardLayout: true
                );

                PilotNameCanonical = "sigma4-battleofyavin-sl";

                ShipInfo.Hull++;
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Disciplined));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.PrimedThrusters));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/a/ae/Sigma4-battleofyavin.png";
            }
        }
    }
}