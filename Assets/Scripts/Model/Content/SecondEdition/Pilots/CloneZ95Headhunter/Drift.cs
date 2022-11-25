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
        public class Drift : CloneZ95Headhunter
        {
            public Drift() : base()
            {
                RequiredMods = new List<Type>() { typeof(Mods.ModsList.UnreleasedContentMod) };

                PilotInfo = new PilotCardInfo(
                    "\"Drift\"",
                    3,
                    31,
                    pilotTitle: "CT-1020",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DriftAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/drift.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DriftAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}