using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class MajorMiandaBoE : TIELnFighter
        {
            public MajorMiandaBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Major Mianda",
                    5,
                    48,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    abilityType: typeof(MajorMiandaAbility),
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    }
                );
                ShipAbilities.Add(new FormedUpBoEAbility());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(EvadeAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(CoordinateAction), ActionColor.Red));
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);

                MustHaveUpgrades.Add(typeof(Ruthless));
                MustHaveUpgrades.Add(typeof(SwarmTactics));
                MustHaveUpgrades.Add(typeof(NoEscape));

                PilotNameCanonical = "majormianda-battleoverendor";
            }
        }
    }
}