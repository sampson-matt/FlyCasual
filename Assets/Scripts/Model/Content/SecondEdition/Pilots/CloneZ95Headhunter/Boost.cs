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
        public class Boost : CloneZ95Headhunter
        {
            public Boost() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "\"Boost\"",
                    3,
                    25,
                    pilotTitle: "CT-4860",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BoostAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/boost.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BoostAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}