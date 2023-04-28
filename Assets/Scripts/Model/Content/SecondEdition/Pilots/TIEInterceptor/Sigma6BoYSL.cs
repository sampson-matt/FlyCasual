using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Abilities;
using ActionsList;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma6BoYSL : TIEInterceptor
        {
            public Sigma6BoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma6",
                    4,
                    48,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma6Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    isStandardLayout: true
                );

                PilotNameCanonical = "sigma6-battleofyavin-sl";

                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Daredevil));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AfterBurners));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/3/3f/Sigma6-battleofyavin.png";
            }
        }
    }
}