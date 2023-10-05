using Upgrade;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.LancerClassPursuitCraft
    {
        public class SabineWren : LancerClassPursuitCraft
        {
            public SabineWren() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sabine Wren",
                    3,
                    56,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.SabineWrenLancerPilotAbility),
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian,
                        Tags.BountyHunter
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "sabinewren-lancerclasspursuitcraft";
            }
        }
    }
}
