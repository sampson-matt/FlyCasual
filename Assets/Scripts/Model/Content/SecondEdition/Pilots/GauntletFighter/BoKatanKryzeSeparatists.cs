using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class BoKatanKryzeSeparatists : GauntletFighter
        {
            public BoKatanKryzeSeparatists() : base()
            {
                //IsWIP = true;

                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo
                (
                    "Bo-Katan Kryze",
                    4,
                    56,
                    pilotTitle: "Vizsla's Lieutenant",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BoKatanKryzeSeparatistsAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                    factionOverride: Faction.Separatists
                );

                ModelInfo.SkinName = "CIS";

                PilotNameCanonical = "bokatankryze-separatistalliance";

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/bokatankryze-separatistalliance.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BoKatanKryzeSeparatistsAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}