using System.Collections.Generic;
using Upgrade;
using System;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class IG102 : RogueClassStarfighter
        {
            public IG102() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "IG-102",
                    4,
                    39,
                    pilotTitle: "Dueling Droid",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.IG102Ability),
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                ShipAbilities.Add(new Abilities.SecondEdition.NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/ig102.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class IG102Ability : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}