
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class CaptainHark : GauntletFighter
        {
            public CaptainHark() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Captain Hark",
                    3,
                    51,
                    pilotTitle: "Obedient Underling",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaptainHarkAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Imperial
                );

                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/1/10/Captainhark.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CaptainHarkAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}