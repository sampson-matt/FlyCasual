using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class KendyIdeleBoE : T65XWing
        {
            public KendyIdeleBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Kendy Idele",
                    4,
                    57,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Missile,
                        UpgradeType.Device
                    },
                    isStandardLayout: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    abilityType: typeof(Abilities.SecondEdition.KendyIdeleAbility)
                );
                ShipAbilities.Add(new LockedSFoils());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(BoostAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction)));
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(IonMissiles));
                MustHaveUpgrades.Add(typeof(ModifiedR4PUnit));
                MustHaveUpgrades.Add(typeof(ChaffParticlesBoE));

                PilotNameCanonical = "kendyidele-battleoverendor";
                ModelInfo.SkinName = "Luke Skywalker";
            }
        }
    }
}