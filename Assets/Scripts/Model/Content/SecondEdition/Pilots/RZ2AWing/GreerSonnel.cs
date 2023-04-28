using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ2AWing
    {
        public class GreerSonnel : RZ2AWing
        {
            public GreerSonnel() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Greer Sonnel",
                    4,
                    35,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.GreerSonnelAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GreerSonnelAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += CheckGreerSonnelAbilityPilotAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= CheckGreerSonnelAbilityPilotAbility;
        }

        protected void CheckGreerSonnelAbilityPilotAbility(GenericShip ship)
        {
            if (BoardTools.Board.IsOffTheBoard(ship)) return;

            RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, PerformAction);
        }

        private void PerformAction(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                UseGreerSonnelAbility,
                descriptionLong: "Do you want to rotate your turret arc indicator?",
                imageHolder: HostShip
            );
        }

        private void UseGreerSonnelAbility(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            new RotateArcAction().DoOnlyEffect(Triggers.FinishTrigger);
        }
    }
}