using System;
using BoardTools;
using Obstacles;
using Ship;
using Tokens;
using UnityEngine;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class GamutKey : GenericUpgrade
    {
        public GamutKey()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Gamut Key",
                UpgradeType.Crew,
                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.GamutKeyCrewAbility),
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                charges: 2,
                regensCharges: true
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GamutKeyCrewAbility : GamutKeyPilotAbility
    {
        protected override string GetAbilityName()
        {
            return HostUpgrade.UpgradeInfo.Name;
        }

        protected override IImageHolder GetAbilityImage()
        {
            return HostUpgrade;
        }

        protected override bool HasEnoughCharges()
        {
            return HostUpgrade.State.Charges >= 2;
        }

        protected override bool FilterTargets(GenericShip ship)
        {
            DistanceInfo distanceInfo = new DistanceInfo(HostShip, ship);
            return distanceInfo.Range <= 1
                && ship.Tokens.CountTokensByShape(TokenShapes.Cirular) > 0;
        }

        protected override void SpendChargesForAbility()
        {
            HostUpgrade.State.SpendCharges(2);
        }
    }
}