
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class Maul : GauntletFighter
        {
            public Maul() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Maul",
                    5,
                    70,
                    pilotTitle: "Lord of the Shadow Collective",
                    isLimited: true,
                    force: 3,
                    abilityType: typeof(Abilities.SecondEdition.MaulAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.ForcePower, UpgradeType.Illicit },
                    factionOverride: Faction.Scum
                );

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/a/a2/Maulgauntlet.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MaulAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}