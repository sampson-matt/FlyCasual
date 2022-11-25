
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class GarSaxon : GauntletFighter
        {
            public GarSaxon() : base()
            {

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Gar Saxon",
                    3,
                    59,
                    pilotTitle: "Treacherous Viceroy",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.GarSaxonAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Imperial
                );

                ModelInfo.SkinName = "CIS Dark";

                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/garsaxon.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GarSaxonAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}