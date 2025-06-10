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
        public class WedgeAntillesBoE : T65XWing
        {
            public WedgeAntillesBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Wedge Antilles",
                    6,
                    70,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    abilityType: typeof(Abilities.FirstEdition.WhisperAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    }
                );
                ShipAbilities.Add(new LockedSFoils());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(BoostAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction)));
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(Predator));
                MustHaveUpgrades.Add(typeof(AdvProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(R2A3BoE));

                PilotNameCanonical = "wedgeantilles-battleoverendor";
                ModelInfo.SkinName = "Wedge Antilles";
            }
        }
    }
}
