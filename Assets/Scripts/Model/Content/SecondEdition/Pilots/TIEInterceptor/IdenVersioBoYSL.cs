using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class IdenVersioBoYSL : TIEInterceptor
        {
            public IdenVersioBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Iden Versio",
                    4,
                    67,
                    isLimited: true,
                    charges: 2,
                    regensCharges: 1,
                    abilityType: typeof(IdenVersioBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    isStandardLayout: true
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Predator));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Fanatic));

                PilotNameCanonical = "idenversio-battleofyavin-sl";
                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/7/76/Idenversio-battleofyavin.png";
            }
        }
    }
}