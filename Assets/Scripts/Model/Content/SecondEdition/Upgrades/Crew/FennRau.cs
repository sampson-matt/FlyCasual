using Upgrade;
using Ship;
using System;
using SubPhases;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class FennRau : GenericUpgrade
    {
        public FennRau() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Fenn Rau",
                UpgradeType.Crew,
                cost: 6,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Rebel, Faction.Scum),
                abilityType: typeof(Abilities.SecondEdition.FennRauCrewAbility)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class FennRauCrewAbility : GenericAbility
    {
        //Before a friendly ship at range 0-2 engages,
        //if its revealed maneuver is speed 1 or higher
        //and there is an enemy ship in its Front Arc at range 1,
        //that friendly ship can remove 1 non-lock red token. 
        public override void ActivateAbility()
        {
            GenericShip.OnCombatActivationGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnCombatActivationGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            if (Tools.IsFriendly(HostShip, ship)
                && BoardState.IsInRange(HostShip, ship, 0, 2)
                && ship.RevealedManeuver.Speed >= 1
                && ship.Tokens.GetNonLockRedTokens().Count >= 1
                && HasEnemyInFrontAtR1(ship))
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatActivation, AskToRemoveRedNonLockToken);
            }
        }

        private bool HasEnemyInFrontAtR1(GenericShip ship)
        {
            foreach (GenericShip enemyShip in ship.Owner.EnemyShips.Values)
            {
                if (ship.SectorsInfo.RangeToShipBySector(enemyShip, Arcs.ArcType.Front) == 1) return true;
            }

            return false;
        }

        private void AskToRemoveRedNonLockToken(object sender, EventArgs e)
        {
            FennRauRebelRemoveRedTokenAbilityDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<FennRauRebelRemoveRedTokenAbilityDecisionSubPhase>(
                "Fenn Rau: You may remove 1 non-lock red token",
                Triggers.FinishTrigger
            );
            subphase.ImageSource = HostUpgrade;
            subphase.AbilityHostShip = HostShip;
            subphase.RemoveOnlyNonLocks = true;
            subphase.Start();
        }

        private class FennRauRebelRemoveRedTokenAbilityDecisionSubPhase : RemoveRedTokenDecisionSubPhase
        {
            public GenericShip AbilityHostShip;

            public override void PrepareCustomDecisions()
            {
                DescriptionShort = AbilityHostShip.PilotInfo.PilotName;
                DescriptionLong = "You may remove 1 non-lock red token";

                DecisionOwner = Selection.ThisShip.Owner;
                DefaultDecisionName = decisions.First().Name;
            }
        }
    }
}