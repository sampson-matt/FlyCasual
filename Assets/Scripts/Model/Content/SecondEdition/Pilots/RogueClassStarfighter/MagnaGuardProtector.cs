using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class MagnaGuardProtector : RogueClassStarfighter
        {
            public MagnaGuardProtector() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "MagnaGuard Protector",
                    4,
                    40,
                    limited: 2,
                    pilotTitle: "Implacable Escort",
                    abilityType: typeof(Abilities.SecondEdition.MagnaGuardProtectorAbility),
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                ShipAbilities.Add(new Abilities.SecondEdition.NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/magnaguardprotector.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MagnaGuardProtectorAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}