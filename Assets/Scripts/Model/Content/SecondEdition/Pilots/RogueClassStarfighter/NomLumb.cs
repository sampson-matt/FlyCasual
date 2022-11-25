using System.Collections.Generic;
using Upgrade;
using System;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class NomLumb : RogueClassStarfighter
        {
            public NomLumb() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "Nom Lumb",
                    1,
                    35,
                    pilotTitle: "Laughing Bandit",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.NomLumbRogueAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                PilotNameCanonical = "nomlumb-rogueclassstarfighter";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/nomlumb-rogueclassstarfighter.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class NomLumbRogueAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}