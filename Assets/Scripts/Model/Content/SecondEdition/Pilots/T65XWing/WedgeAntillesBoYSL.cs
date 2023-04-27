using Conditions;
using Ship;
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
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(Abilities.SecondEdition.WedgeAntillesBoYAbility),
                    isStandardLayout: true
                );
                ShipAbilities.Add(new HopeAbility());

                MustHaveUpgrades.Add(typeof(AttackSpeed));
                MustHaveUpgrades.Add(typeof(Marksmanship));
                MustHaveUpgrades.Add(typeof(ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(R2A3BoY));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/a/a4/Wedgeantilles-battleofyavin.png";
                PilotNameCanonical = "wedgeantilles-battleofyavin-sl";
                ModelInfo.SkinName = "Wedge Antilles";
            }
        }
    }
}