using ActionsList;
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.AttackShuttle
    {
        public class SabineWren : AttackShuttle
        {
            public SabineWren() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sabine Wren",
                    3,
                    41,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.SabineWrenPilotAbility),
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian,
                        Tags.Spectre
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}