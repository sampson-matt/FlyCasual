using ActionsList;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Movement;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class IonManeuveringJet : GenericUpgrade
    {
        public IonManeuveringJet() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Ion Maneuvering Jet",
                UpgradeType.Modification,
                cost: 0,
                charges: 2,
                abilityType: typeof(Abilities.SecondEdition.IonManeuveringJetAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/ChaffParticles.jpg";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class IonManeuveringJetAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinish += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinish -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            if (ship.AssignedManeuver != null && ship.AssignedManeuver.Bearing == ManeuverBearing.KoiogranTurn)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, UseAbility);
            }
        }

        private void UseAbility(object sender, EventArgs e)
        {
            if (HostShip.AssignedManeuver.Bearing == ManeuverBearing.KoiogranTurn && HostUpgrade.State.Charges > 0)
            {
                HostShip.BeforeActionIsPerformed += SpendCharge;

                HostShip.OnCanPerformActionWhileStressed += TemporaryAllowAnyActionsWhileStressed;
                HostShip.OnCheckCanPerformActionsWhileStressed += TemporaryAllowActionsWhileStressed;
                HostShip.OnActionIsPerformed += DisallowActionsWhileStressed;
                HostShip.OnActionIsSkipped += DisallowActionsWhileStressedAlt;

                List<GenericAction> actions = HostShip.GetAvailableActions();

                HostShip.AskPerformFreeAction(
                    actions,
                    delegate
                    {
                        Triggers.FinishTrigger();
                    },
                    HostUpgrade.UpgradeInfo.Name,
                    "After you fully execute a Koigran Turn, you may spend 1 Charge to perform an action, even while stressed",
                    HostUpgrade
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void SpendCharge(GenericAction action, ref bool isFreeAction)
        {
            HostUpgrade.State.SpendCharge();
            HostShip.BeforeActionIsPerformed -= SpendCharge;
        }

        private void DisallowActionsWhileStressed(GenericAction action)
        {
            HostShip.OnCanPerformActionWhileStressed -= TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed -= TemporaryAllowActionsWhileStressed;
            HostShip.OnActionIsPerformed -= DisallowActionsWhileStressed;
        }

        private void DisallowActionsWhileStressedAlt(GenericShip ship)
        {
            HostShip.OnCanPerformActionWhileStressed -= TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed -= TemporaryAllowActionsWhileStressed;
            HostShip.OnActionIsPerformed -= DisallowActionsWhileStressed;
            HostShip.OnActionIsSkipped -= DisallowActionsWhileStressedAlt;
        }

        private void TemporaryAllowAnyActionsWhileStressed(GenericAction action, ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void TemporaryAllowActionsWhileStressed(ref bool isAllowed)
        {
            isAllowed = true;
        }

    }
}

