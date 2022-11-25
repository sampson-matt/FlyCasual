using System.Collections.Generic;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class CadBaneSeparatist : RogueClassStarfighter
        {
            public CadBaneSeparatist() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "Cad Bane",
                    4,
                    44,
                    pilotTitle: "Needs No Introduction",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CadBaneSeparatistAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Separatists
                );

                PilotNameCanonical = "cadbane-separatistalliance";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/cadbane-separatistalliance.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CadBaneSeparatistAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}