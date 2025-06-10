using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class YendorBoE : T65XWing
        {
            public YendorBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Yendor",
                    5,
                    50,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    abilityType: typeof(Abilities.SecondEdition.YendorAbility)
                );
                ShipAbilities.Add(new LockedSFoils());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(BoostAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction)));
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(PlasmaTorpedoes));
                MustHaveUpgrades.Add(typeof(StabilizingAstromech));

                PilotNameCanonical = "yendor-battleoverendor";
            }
        }
    }
}