
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class RookKast : GauntletFighter
        {
            public RookKast() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Rook Kast",
                    3,
                    61,
                    pilotTitle: "Stoic Super Commando",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.RookKastAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                    factionOverride: Faction.Scum
                );

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/e/e1/Rookkast.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class RookKastAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}