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
        public class MausMonareBoE : TIEInterceptor
        {
            public MausMonareBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Maus Monare",
                    3,
                    56,
                    isLimited: true,
                    abilityType: typeof(MausMonareAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    }
                );
                PilotNameCanonical = "mausmonare-battleoverendor";
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(NoEscape));
                MustHaveUpgrades.Add(typeof(Outmaneuver));
                MustHaveUpgrades.Add(typeof(FuelInjectionOverrideBoE));
            }
        }
    }
}