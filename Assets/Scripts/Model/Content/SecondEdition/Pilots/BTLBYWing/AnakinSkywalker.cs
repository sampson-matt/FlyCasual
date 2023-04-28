using System.Collections.Generic;
using Upgrade;
using Content;

namespace Ship
{
    namespace SecondEdition.BTLBYWing
    {
        public class AnakinSkywalker : BTLBYWing
        {
            public AnakinSkywalker() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Anakin Skywalker",
                    6,
                    49,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.ForcePower, UpgradeType.Astromech },
                    force: 3,
                    abilityText: "After you fully execute a maneuver, if there is an enemy ship in your standard front arc at range 0-1 or in your bullseye arc, you may spend 1 force to remove 1 stress token.",
                    abilityType: typeof(Abilities.SecondEdition.AnakinSkywalkerAbility),
                    tags: new List<Tags>
                    {
                        Tags.LightSide,
                        Tags.Jedi,
                        Tags.YWing
                    }
                );

                PilotNameCanonical = "anakinskywalker-btlbywing";
            }
        }
    }
}
