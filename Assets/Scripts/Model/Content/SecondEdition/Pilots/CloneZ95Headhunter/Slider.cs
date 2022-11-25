using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.CloneZ95Headhunter
    {
        public class Slider : CloneZ95Headhunter
        {
            public Slider() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "\"Slider\"",
                    4,
                    27,
                    pilotTitle: "Evasive Aviator",
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Abilities.SecondEdition.SliderAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/slider.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class SliderAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}