using Upgrade;
using Ship;
using ActionsList;
using System;
using SubPhases;
using Tokens;
using BoardTools;
using System.Linq;
using UnityEngine;
using Movement;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class PrimeMinisterAlmec : GenericDualUpgrade
    {
        public PrimeMinisterAlmec() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Prime Minister Almec",
                UpgradeType.Crew,
                cost: 9,
                restriction: new FactionRestriction(Faction.Republic, Faction.Scum),
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.PrimeMinisterAlmecAbility)
            );

            SelectSideOnSetup = false;
            AnotherSide = typeof(AlmecMaulsPuppet);
        }
    }

    public class AlmecMaulsPuppet : GenericDualUpgrade
    {
        public AlmecMaulsPuppet() : base()
        {
            IsHidden = true; // Hidden in Squad Builder only
            NameCanonical = "primeministeralmec-sideb";

            UpgradeInfo = new UpgradeCardInfo(
                "Almec Maul's Puppet",
                UpgradeType.Crew,
                cost: 9,
                abilityType: typeof(Abilities.SecondEdition.AlmecMaulsPuppetAbility)
            );

            AnotherSide = typeof(PrimeMinisterAlmec);
            IsSecondSide = true;
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PrimeMinisterAlmecAbility : GenericAbility
    {
        GenericShip friendlyShip = null;
        //After a friendly ship at range 0-2 reveals a white maneuver,
        //if it has no green tokens, it may gain 1 stress token to gain 1 calculate token.
        //During the end phase, if you have 2 or more stress tokens, flip this card.
        public override void ActivateAbility()
        {
            GenericShip.OnManeuverIsRevealedGlobal += TryRegisterAbility;
            Phases.Events.OnEndPhaseStart_Triggers += RegisterFlipAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnManeuverIsRevealedGlobal -= TryRegisterAbility;
            Phases.Events.OnEndPhaseStart_Triggers -= RegisterFlipAbility;
        }

        private void TryRegisterAbility(GenericShip ship)
        {
            if (Board.GetShipsAtRange(HostShip, new Vector2(0, 2), Team.Type.Friendly).Contains(ship)
                && ship.Tokens.GetTokensByColor(TokenColors.Green).Count == 0
                && Selection.ThisShip.RevealedManeuver.ColorComplexity == MovementComplexity.Normal
            )
            {
                friendlyShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnManeuverIsRevealed, AskToUseAbility);
            }
        }

        private void AskToUseAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                "Prime Minister Almec",
                NeverUseByDefault,
                GainCalculate,
                descriptionLong: friendlyShip.PilotInfo.PilotName + ": Do you want to gain 1 stress token to gain 1 calculate token?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void GainCalculate(object sender, EventArgs e)
        {
            friendlyShip.Tokens.AssignToken(typeof(StressToken), delegate { });

            friendlyShip.Tokens.AssignToken(typeof(CalculateToken), DecisionSubPhase.ConfirmDecision);
        }

        private void RegisterFlipAbility()
        {
            if (HostShip.Tokens.CountTokensByType(typeof(StressToken)) >= 2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnEndPhaseStart, FlipCard);
            }
        }

        private void FlipCard(object sender, EventArgs e)
        {
            (HostUpgrade as GenericDualUpgrade).Flip();
            Triggers.FinishTrigger();
        }
    }

    public class AlmecMaulsPuppetAbility : GenericAbility
    {
        GenericShip friendlyShip = null;
        //After a friendly ship at range 0-2 fully executes a red maneuver,
        //that ship may perform a Action: Calculate or Action: Focus action on its action bar, even while stressed. 
        public override void ActivateAbility()
        {
            GenericShip.OnMovementFinishSuccessfullyGlobal += TryRegisterAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnMovementFinishSuccessfullyGlobal -= TryRegisterAbility;
        }

        private void TryRegisterAbility(GenericShip ship)
        {
            if (Board.GetShipsAtRange(HostShip, new Vector2(0, 2), Team.Type.Friendly).Contains(ship)
                && Selection.ThisShip.RevealedManeuver.ColorComplexity == MovementComplexity.Complex
            )
            {
                friendlyShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskToUseAbility);
            }
        }

        private void AskToUseAbility(object sender, EventArgs e)
        {
            var previousSelectedShip = Selection.ThisShip;
            List<GenericAction> actions = GetPossibleActions();
            actions.ForEach(n => n.CanBePerformedWhileStressed = true);

            HostShip.AskPerformFreeAction(
                actions,
                delegate
                {
                    Selection.ThisShip = previousSelectedShip;
                    Triggers.FinishTrigger();
                },
                HostUpgrade.UpgradeInfo.Name,
                "After a friendly ship at range 0-2 fully executes a red maneuver, that ship may perform a Action: Calculate or Action: Focus action on its action bar, even while stressed.",
                HostUpgrade
            );
        }

        private List<GenericAction> GetPossibleActions()
        {
            return friendlyShip.ActionBar.AllActions.Where(n => n.GetType() == typeof(CalculateAction) || n.GetType() == typeof(FocusAction)).ToList();
            //return friendlyShip.GetAvailableActions().Where(n => n.GetType() == typeof(CalculateAction) || n.GetType() == typeof(FocusAction)).ToList();
        }


    }
}

