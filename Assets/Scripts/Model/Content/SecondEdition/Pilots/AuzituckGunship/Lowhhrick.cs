using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.AuzituckGunship
    {
        public class Lowhhrick : AuzituckGunship
        {
            public Lowhhrick() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lowhhrick",
                    3,
                    49,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.LowhhrickAbility),
                    tags: new List<Tags>
                    {
                        Tags.Wookie,
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Lowhhrick";
            }
        }
    }
}