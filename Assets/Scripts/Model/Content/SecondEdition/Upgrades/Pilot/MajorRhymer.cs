using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class MajorRhymerPilotAbility : GenericUpgrade
    {
        public MajorRhymerPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Major Rhymer Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.MajorRhymerAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/majorrhymer.png";
        }


    }
}