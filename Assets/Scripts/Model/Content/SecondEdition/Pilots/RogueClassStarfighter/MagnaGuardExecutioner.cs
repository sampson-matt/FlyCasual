using System.Collections.Generic;
using Upgrade;
using System.Linq;
using Abilities.SecondEdition;

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
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                DeadToRights oldAbility = (DeadToRights)ShipAbilities.First(n => n.GetType() == typeof(DeadToRights));
                oldAbility.DeactivateAbility();
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/magnaguardexecutioner.png";
            }
        }
    }
}