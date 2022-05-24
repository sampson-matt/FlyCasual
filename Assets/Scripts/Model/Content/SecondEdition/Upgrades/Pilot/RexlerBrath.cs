using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;
using System;

namespace UpgradesList.SecondEdition
{
    public class RexlerBrathPilotAbility : GenericUpgrade
    {
        public RexlerBrathPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Rexler Brath Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.RexlerBrathHotacAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/rexlerbrath.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class RexlerBrathHotacAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnSufferHullDamageGlobal += CheckConditions;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnSufferHullDamageGlobal -= CheckConditions;
        }

        private void CheckConditions(GenericShip ship, ref bool isCrit, EventArgs e)
        {
            if ((e as DamageSourceEventArgs) == null) return;

            GenericShip damageSourceShip = (e as DamageSourceEventArgs).Source as GenericShip;
            if (damageSourceShip == null) return;

            if (damageSourceShip.ShipId == HostShip.ShipId)
            {
                isCrit = true;
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " deals all Damage cards face-up.");
            }
        }
    }
}