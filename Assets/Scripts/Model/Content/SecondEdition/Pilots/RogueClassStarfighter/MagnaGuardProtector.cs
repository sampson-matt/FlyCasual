using System.Collections.Generic;
using Upgrade;
using System;
using Abilities.SecondEdition;
using System.Linq;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class MagnaGuardProtector : RogueClassStarfighter
        {
            public MagnaGuardProtector() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "MagnaGuard Protector",
                    4,
                    40,
                    limited: 2,
                    pilotTitle: "Implacable Escort",
                    abilityType: typeof(Abilities.SecondEdition.MagnaGuardProtectorAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                DeadToRights oldAbility = (DeadToRights)ShipAbilities.First(n => n.GetType() == typeof(DeadToRights));
                oldAbility.DeactivateAbility();
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new NetworkedCalculationsAbility());

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