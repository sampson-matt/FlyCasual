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
        public class CaptainYorrBoE : TIEDDefender
        {
            public CaptainYorrBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Yorr",
                    4,
                    85,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Abilities.SecondEdition.CaptainYorrBattleOverEndorAbility),
                    extraUpgradeIcons: new List<UpgradeType>()
                    { 
                        UpgradeType.Talent, 
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    }
                );
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(EvadeAction), typeof(BarrelRollAction)));
                FullThrottleAbility oldAbility = (FullThrottleAbility)ShipAbilities.First(n => n.GetType() == typeof(FullThrottleAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new ChissEngineeringAbility());

                MustHaveUpgrades.Add(typeof(NoEscapeBoE));
                MustHaveUpgrades.Add(typeof(Predator));
                MustHaveUpgrades.Add(typeof(IonCannon));
                MustHaveUpgrades.Add(typeof(ComputerAssistedHandling));

                PilotNameCanonical = "captainyorr-battleoverendor";
            }
        }
    }
}