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
        public class Scythe6BoE : TIELnFighter
        {
            public Scythe6BoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Scythe 6",
                    2,
                    53,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    abilityType: typeof(Scythe6Ability),
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    }
                );
                ShipAbilities.Add(new FormedUpBoEAbility());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(EvadeAction)));
                ShipInfo.Hull++;

                MustHaveUpgrades.Add(typeof(NoEscapeBoE));
                MustHaveUpgrades.Add(typeof(Predator));
                MustHaveUpgrades.Add(typeof(IonManeuveringJet));
                MustHaveUpgrades.Add(typeof(TargetingMatrix));

                PilotNameCanonical = "scythe6-battleoverendor";
            }
        }
    }
}