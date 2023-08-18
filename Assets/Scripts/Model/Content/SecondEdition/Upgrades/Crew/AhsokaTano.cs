using Ship;
using Upgrade;
using System;
using SubPhases;
using Actions;
using ActionsList;

namespace UpgradesList.SecondEdition
{
    public class AhsokaTano : GenericUpgrade
    {
        public AhsokaTano() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ahsoka Tano",
                UpgradeType.Crew,
                cost: 10,
                isLimited: true,
                addForce: 1,
                restriction: new FactionRestriction(Faction.Scum, Faction.Republic),
                abilityType: typeof(Abilities.SecondEdition.AhsokaTanoCrewAbility)
            );

            NameCanonical = "ahsokatano-crew";
        }
    }
}


namespace Abilities.SecondEdition
{
    public class AhsokaTanoCrewAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, SelectTargetForAbility);
        }

        private void SelectTargetForAbility(object sender, EventArgs e)
        {
            if (HostShip.State.Force >= 1 && HasTargetsForAbility())
            {
                SelectTargetForAbility(
                    GrantAction,
                    FilterTargets,
                    GetAiPriority,
                    HostShip.Owner.PlayerNo,
                    HostShip.PilotInfo.PilotName,
                    "You may spend 1 Force to choose 1 friendly ship in your Full Rear Arc at range 1-2. If you do, that ship may perform a red Focus action, even while stressed. ",
                    HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void GrantAction()
        {
            TargetShip.BeforeActionIsPerformed += PayForceCost;

            SelectShipSubPhase.FinishSelectionNoCallback();
            Selection.ThisShip = TargetShip;

            TargetShip.OnCheckCanPerformActionsWhileStressed += ConfirmThatIsPossible;
            TargetShip.OnCanPerformActionWhileStressed += AlwaysAllow;

            TargetShip.AskPerformFreeAction(
                new FocusAction() { Color = ActionColor.Red },
                delegate {
                    TargetShip.OnCheckCanPerformActionsWhileStressed -= ConfirmThatIsPossible;
                    TargetShip.OnCanPerformActionWhileStressed -= AlwaysAllow;

                    Selection.ThisShip = HostShip;
                    TargetShip.BeforeActionIsPerformed -= PayForceCost;
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "You may perform an action, even if you is stressed.",
                HostShip
            );
        }

        private void ConfirmThatIsPossible(ref bool isAllowed)
        {
            AlwaysAllow(null, ref isAllowed);
        }

        private void AlwaysAllow(GenericAction action, ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void PayForceCost(GenericAction action, ref bool isFreeAction)
        {
            TargetShip.BeforeActionIsPerformed -= PayForceCost;

            RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, SpendForce);
        }

        private void SpendForce(object sender, EventArgs e)
        {
            HostShip.State.SpendForce(1, Triggers.FinishTrigger);
        }

        private bool HasTargetsForAbility()
        {
            foreach (GenericShip ship in HostShip.Owner.Ships.Values)
            {
                if (FilterTargets(ship)) return true;
            }

            return false;
        }

        private bool FilterTargets(GenericShip ship)
        {
            return BoardTools.Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.FullRear, new UnityEngine.Vector2(1, 2), Team.Type.Friendly).Contains(ship);
        }

        private int GetAiPriority(GenericShip ship)
        {
            int priority = 0;

            if (!ship.Tokens.HasToken(typeof(Tokens.FocusToken))) priority += 100;

            priority += ship.PilotInfo.Cost;

            return priority;
        }
    }
}