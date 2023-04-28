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
        public class Sigma5BoYSL : TIEInterceptor
        {
            public Sigma5BoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma5",
                    4,
                    50,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma5Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    isStandardLayout: true
                );

                PilotNameCanonical = "sigma5-battleofyavin-sl";

                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.SensorJammerBoY));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Elusive));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/8/85/Sigma5-battleofyavin.png";
            }
        }
    }
}