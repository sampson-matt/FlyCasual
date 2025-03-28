using Abilities.SecondEdition;
using Content;
using System.Collections.Generic;
using System.Linq;
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
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Separatists,
                    tags: new List<Tags>
                    {
                        Tags.Droid
                    }
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                DeadToRights oldAbility = (DeadToRights)ShipAbilities.First(n => n.GetType() == typeof(DeadToRights));
                oldAbility.DeactivateAbility();
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new NetworkedCalculationsAbility());
            }
        }
    }
}