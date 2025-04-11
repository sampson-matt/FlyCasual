using Conditions;
using Upgrade;
using Content;
using Abilities.SecondEdition;
using System.Collections.Generic;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class WedgeAntillesBoYSL : T65XWing
        {
            public WedgeAntillesBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Wedge Antilles",
                    5,
                    65,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Torpedo,
                        UpgradeType.Astromech
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.SL
                    },
                    abilityType: typeof(Abilities.SecondEdition.WedgeAntillesBoYAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(AttackSpeed));
                MustHaveUpgrades.Add(typeof(Marksmanship));
                MustHaveUpgrades.Add(typeof(ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(R2A3BoY));
                PilotNameCanonical = "wedgeantilles-battleofyavin";
                ModelInfo.SkinName = "Wedge Antilles";
            }
        }
    }
}