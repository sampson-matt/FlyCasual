using Abilities.SecondEdition;
using ActionsList;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class PopsKrail : BTLA4YWing
        {
            public PopsKrail() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Pops\" Krail",
                    3,
                    36,
                    pilotTitle: "Gold Five",
                    isLimited: true,
                    abilityType: typeof(PopsKrailAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Modification }
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PopsKrailAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += CheckPopsKrailPilotAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= CheckPopsKrailPilotAbility;
        }

        private void CheckPopsKrailPilotAbility(GenericShip ship)
        {
            if (BoardTools.Board.IsOffTheBoard(ship)) return;

            if (ship.AssignedManeuver.ColorComplexity == Movement.MovementComplexity.Normal)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, PopsKrailPilotAbility);
            }
        }

        private void PopsKrailPilotAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                GrantFreeFocusAction,
                FilterAbilityTargets,
                GetAiAbilityPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                "Choose a ship.\nIt may perform a focus action.",
                HostShip
            );
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            return FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.AnyFriendly }) && FilterTargetsByRange(ship, 0, 1);
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int result = 0;

            result += NeedTokenPriority(ship);
            result += ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost);

            return result;
        }

        private int NeedTokenPriority(GenericShip ship)
        {
            if (!ship.Tokens.HasToken(typeof(FocusToken)) && !ship.IsStressed) return 100;
            return 0;
        }

        private void GrantFreeFocusAction()
        {
            Selection.ThisShip = TargetShip;

            RegisterAbilityTrigger(TriggerTypes.OnFreeActionPlanned, PerformFreeFocusAction);

            Triggers.ResolveTriggers(TriggerTypes.OnFreeActionPlanned, SelectShipSubPhase.FinishSelection);
        }

        protected virtual void PerformFreeFocusAction(object sender, System.EventArgs e)
        {
            TargetShip.AskPerformFreeAction(
                new FocusAction(),
                delegate {
                    Selection.ThisShip = HostShip;
                    Phases.CurrentSubPhase.Resume();
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "You may perform a focus action",
                HostShip
            );
        }
    }
}