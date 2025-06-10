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
        public class BraylenStrammBoE : ASF01BWing
        {
            public BraylenStrammBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Braylen Stramm",
                    4,
                    68,
                    isLimited: true,
                    abilityType: typeof(BraylenStrammBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    charges: 2,
                    regensCharges: 1,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Missile,
                        UpgradeType.Device
                    },
                    isStandardLayout: true
                );
                ShipAbilities.Add(new GyroCockpit());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(TargetLockAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ReloadAction), ActionColor.Red));

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(HomingMissiles));
                MustHaveUpgrades.Add(typeof(DelayedFuses));
                MustHaveUpgrades.Add(typeof(ProtonBombs));

                ModelInfo.SkinName = "Dark Blue";
                PilotNameCanonical = "braylenstramm-battleoverendor";
            }
        }
    }
}