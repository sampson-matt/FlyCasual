using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class NightBeastPilotAbility : GenericUpgrade
    {
        public NightBeastPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Night Beast Pilot Ability",
                UpgradeType.Pilot,

                cost: 4,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        2
                    ),
                abilityType: typeof(Abilities.FirstEdition.KathScarletEmpireAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/nightbeast.png";
        }


    }
}