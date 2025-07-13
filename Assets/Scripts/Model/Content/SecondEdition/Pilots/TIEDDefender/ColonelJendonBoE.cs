using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIEDDefender
    {
        public class ColonelJendonBoE : TIEDDefender
        {
            public ColonelJendonBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Colonel Jendon",
                    6,
                    87,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ColonelJendonBattleOverEndorAbility),
                    extraUpgradeIcons: new List<UpgradeType>()
                    { 
                        UpgradeType.Talent, 
                        UpgradeType.Talent,
                        UpgradeType.Modification,
                        UpgradeType.Cannon
                    }
                );
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(EvadeAction), typeof(BarrelRollAction)));
                FullThrottleAbility oldAbility = (FullThrottleAbility)ShipAbilities.First(n => n.GetType() == typeof(FullThrottleAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new ChissEngineeringAbility());

                MustHaveUpgrades.Add(typeof(NoEscape));
                MustHaveUpgrades.Add(typeof(PushTheLimitBoE));
                MustHaveUpgrades.Add(typeof(ProtonCannons));
                MustHaveUpgrades.Add(typeof(ComputerAssistedHandling));

                PilotNameCanonical = "coloneljendon-battleoverendor";
            }
        }
    }
}