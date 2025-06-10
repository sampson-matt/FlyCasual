using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class AdonFoxBoE : ASF01BWing
        {
            public AdonFoxBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Adon Fox",
                    1,
                    65,
                    isLimited: true,
                    abilityType: typeof(AdonFoxBattleOverEndorAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Missile,
                        UpgradeType.Device
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    charges: 2,
                    regensCharges: 1
                );
                ShipAbilities.Add(new GyroCockpit());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(TargetLockAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ReloadAction), ActionColor.Red));

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(PartingGift));
                MustHaveUpgrades.Add(typeof(ProtonRockets));
                MustHaveUpgrades.Add(typeof(ProtonBombs));

                ModelInfo.SkinName = "Blue";
                PilotNameCanonical = "adonfox-battleoverendor";
            }
        }
    }
}
