using Abilities.SecondEdition;
using Content;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sapphire2BoE : TIEInterceptor
        {
            public Sapphire2BoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sapphire 2",
                    1,
                    53,
                    isLimited: true,
                    abilityType: typeof(Saphire2Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Tech
                    }
                );
                PilotNameCanonical = "sapphire2-battleoverendor";
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(NoEscape));
                MustHaveUpgrades.Add(typeof(Reckless));
                MustHaveUpgrades.Add(typeof(TargetingMatrix));
                MustHaveUpgrades.Add(typeof(PrimedThrusters));
            }
        }
    }
}