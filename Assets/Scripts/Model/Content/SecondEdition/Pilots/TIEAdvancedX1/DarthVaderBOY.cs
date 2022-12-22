using Abilities.SecondEdition;
using ActionsList;
using Ship;
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEAdvancedX1
    {
        public class DarthVaderBOY : TIEAdvancedX1
        {
            public DarthVaderBOY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Darth Vader",
                    6,
                    73,
                    isLimited: true,
                    abilityType: typeof(DarthVaderDefenderAbility),
                    force: 3,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcon: UpgradeType.ForcePower
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/darthvader-boy.png";
                PilotNameCanonical = "darthvader-boy";
                ModelInfo.SkinName = "Blue";
            }
        }
    }
}