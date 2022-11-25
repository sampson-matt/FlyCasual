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

        public class Stub : CloneZ95Headhunter
        {
            public Stub() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "\"Stub\"",
                    3,
                    31,
                    pilotTitle: "Scrappy Flier",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.StubAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/stub.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class StubAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}