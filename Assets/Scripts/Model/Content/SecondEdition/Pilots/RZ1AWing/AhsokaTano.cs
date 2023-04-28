using Mods.ModsList;
using Ship;
using SubPhases;
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class AhsokaTano : RZ1AWing
        {
            public AhsokaTano() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Ahsoka Tano",
                    5,
                    50,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.AhsokaTanoRebelAbility),
                    tags: new List<Tags>
                    {
                        Tags.LightSide
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.ForcePower, UpgradeType.ForcePower },
                    force: 3,
                    abilityText: "After you fully execute a maneuver, you may choose a friendly ship at range 0-1 and spend 1 Force. That ship may perform an action, even if it is stressed."
                );

                PilotNameCanonical = "ahsokatano-rz1awing";

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AhsokaTanoRebelAbility: AhsokaTanoAbility
    {
        protected override int ForceCost => 2;
        protected override int MinRange => 1;
        protected override int MaxRange => 2;
    }
}