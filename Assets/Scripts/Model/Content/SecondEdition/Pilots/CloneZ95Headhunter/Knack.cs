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
        public class Knack : CloneZ95Headhunter
        {
            public Knack() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "\"Knack\"",
                    5,
                    26,
                    pilotTitle: "Incautious Instructor",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.KnackAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/knack.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class KnackAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}