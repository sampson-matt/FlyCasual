using Abilities.SecondEdition;
using Ship;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.NabooRoyalN1Starfighter
    {
        public class DineeEllberger : NabooRoyalN1Starfighter
        {
            public DineeEllberger() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Dineé Ellberger",
                    3,
                    31,
                    isLimited: true,
                    abilityText: "While you defend or perform an attack, if the speed of your revealed maneuver is the same as the enemy ship's, that ship's dice cannot be modified.",
                    abilityType: typeof(DineeEllbergerAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DineeEllbergerAbility : GenericAbility
    {
        GenericShip EnemyShip;
        public override void ActivateAbility()
        {
            HostShip.OnDefenceStartAsAttacker += CheckDineeEllbergerAbility;
            HostShip.OnAttackStartAsDefender += CheckDineeEllbergerAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDefenceStartAsAttacker -= CheckDineeEllbergerAbility;
            HostShip.OnAttackStartAsDefender -= CheckDineeEllbergerAbility;
        }

        private void CheckDineeEllbergerAbility()
        {
            if (Combat.Defender.ShipId == HostShip.ShipId)
            {
                EnemyShip = Combat.Attacker;
            }
            else
            {
                EnemyShip = Combat.Defender;
            }
            if (HostShip.RevealedManeuver == null || EnemyShip.RevealedManeuver == null) return;

            if (HostShip.RevealedManeuver.Speed == EnemyShip.RevealedManeuver.Speed)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": Enemy's dice cannot be modified.");
                EnemyShip.OnTryAddAvailableDiceModification += UseDiceRestriction;
                HostShip.OnTryAddDiceModificationOpposite += UseDiceRestriction;
                EnemyShip.OnAttackFinish += RemoveOmegaLeaderPilotAbility;
            }
        }

        private void UseDiceRestriction(GenericShip ship, ActionsList.GenericAction diceModification, ref bool canBeUsed)
        {
            if (!diceModification.IsNotRealDiceModification)
            {
                Messages.ShowErrorToHuman(HostShip.PilotInfo.PilotName + ": Enemy's dice cannot be modified.");
                canBeUsed = false;
            }
        }

        private void RemoveOmegaLeaderPilotAbility(GenericShip ship)
        {
            ship.OnTryAddAvailableDiceModification -= UseDiceRestriction;
            HostShip.OnTryAddDiceModificationOpposite -= UseDiceRestriction;
            ship.OnAttackFinish -= RemoveOmegaLeaderPilotAbility;
        }
    }
}
