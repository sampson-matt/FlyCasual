using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class MagnaGuardExecutioner : RogueClassStarfighter
        {
            public MagnaGuardExecutioner() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "MagnaGuard Executioner",
                    3,
                    37,
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                ShipAbilities.Add(new Abilities.SecondEdition.NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/magnaguardexecutioner.png";
            }
        }
    }
}