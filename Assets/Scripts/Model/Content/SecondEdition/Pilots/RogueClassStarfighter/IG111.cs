using System.Collections.Generic;
using Upgrade;
using System;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class IG111 : RogueClassStarfighter
        {
            public IG111() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "IG-111",
                    1,
                    38,
                    pilotTitle: "One Eye",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.IG111Ability),
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                ShipAbilities.Add(new Abilities.SecondEdition.NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/ig111.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class IG111Ability : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}