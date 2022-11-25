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
        public class Killer : CloneZ95Headhunter
        {
            public Killer() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "\"Killer\"",
                    3,
                    25,
                    pilotTitle: "Dependable Closer",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.KillerAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/killer.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class KillerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}