using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class IG101 : RogueClassStarfighter
        {
            public IG101() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "IG-101",
                    4,
                    39,
                    pilotTitle: "Tenacious Bodyguard",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.IG101Ability),
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                ShipAbilities.Add(new Abilities.SecondEdition.NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/ig101.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class IG101Ability : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}