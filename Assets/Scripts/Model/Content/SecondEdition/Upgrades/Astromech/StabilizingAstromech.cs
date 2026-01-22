using Actions;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class StabilizingAstromech : GenericUpgrade
    {
        public StabilizingAstromech() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Stabilizing Astromech",
                UpgradeType.Astromech,
                cost: 0,
                charges: 1,
                abilityType: typeof(Abilities.SecondEdition.StabilizingAstromechAbility)
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you fully execute a maneuver, you may spend 1 charge to perform a white action, even while stressed.
    public class StabilizingAstromechAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, PerformFreeAction);
            }
        }

        private void PerformFreeAction(object sender, EventArgs e)
        {
            HostShip.BeforeActionIsPerformed += RegisterSpendChargeTrigger;
            HostShip.OnCanPerformActionWhileStressed += ConfirmThatIsPossible;

            List<GenericAction> actions = HostShip.GetAvailableActions();
            List<GenericAction> whiteActions = actions
                .Where(n => n.IsInActionBar && n.Color == ActionColor.White)
                .Select(n => n.AsPerformWhileStressedAction)
                .ToList();

            Selection.ThisShip.AskPerformFreeAction(whiteActions,
                CleanUp,
                HostUpgrade.UpgradeInfo.Name,
                "After you fully execute a maneuver, you may spend 1 charge to perform a white action, even while stressed.",
                HostUpgrade
            );
        }

        private void ConfirmThatIsPossible(GenericAction action, ref bool isAllowed)
        {
            isAllowed = action.Color == Actions.ActionColor.White;
        }

        private void RegisterSpendChargeTrigger(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            RegisterAbilityTrigger(
                TriggerTypes.OnFreeAction,
                delegate {
                    HostUpgrade.State.SpendCharge();
                    Triggers.FinishTrigger();
                }
            );
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= RegisterSpendChargeTrigger;
            HostShip.OnCanPerformActionWhileStressed -= ConfirmThatIsPossible;
            Triggers.FinishTrigger();
        }
    }
}