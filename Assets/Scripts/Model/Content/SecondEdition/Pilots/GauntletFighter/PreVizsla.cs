using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class PreVizsla : GauntletFighter
        {
            public PreVizsla() : base()
            {

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Pre Vizsla",
                    3,
                    59,
                    pilotTitle: "Leader of Death Watch",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.PreVizslaAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                    factionOverride: Faction.Separatists
                );

                ModelInfo.SkinName = "CIS Light";

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/1/1f/Previzsla.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PreVizslaAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}