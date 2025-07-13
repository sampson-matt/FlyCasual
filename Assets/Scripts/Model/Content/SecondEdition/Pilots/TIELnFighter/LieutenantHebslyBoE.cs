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
    namespace SecondEdition.TIELnFighter
    {
        public class LieutenantHebslyBoE : TIELnFighter
        {
            public LieutenantHebslyBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lieutenant Hebsly",
                    3,
                    51,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    abilityType: typeof(LieutenantHebsly),
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    }
                );
                ShipAbilities.Add(new FormedUpBoEAbility());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(EvadeAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction), ActionColor.Red));
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);

                MustHaveUpgrades.Add(typeof(NoEscape));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Elusive));
                MustHaveUpgrades.Add(typeof(Collected));

                PilotNameCanonical = "lieutenanthebsly-battleoverendor";           
            }
        }
    }
}