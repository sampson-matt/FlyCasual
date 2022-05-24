using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class ZertikStromPilotAbility : GenericUpgrade
    {
        public ZertikStromPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Zertik Strom Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.ZertikStromAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/zertikstrom.png";
        }


    }
}