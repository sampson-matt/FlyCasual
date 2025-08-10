using Abilities.SecondEdition;
using ActionsList;
using Ship;
using SubPhases;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class AntocMerrick : T65XWing
        {
            public AntocMerrick() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Antoc Merrick",
                    4,
                    48,
                    isLimited: true,
                    abilityType: typeof(AntocMerrickAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Blue";


                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/IMG_0653.jpg";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AntocMerrickAbility : GenericAbility
    {
        private GenericAction selectedAction;

        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckAbility;
        }

        private void CheckAbility(GenericAction action)
        {
            if (action is BoostAction || action is BarrelRollAction)
            {
                selectedAction = action;
                selectedAction.Color = Actions.ActionColor.White;
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, SelectTargetForAbility);
            }
        }

        private void SelectTargetForAbility(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                GrantFreeFocusAction,
                FilterTargets,
                GetAiPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                $"Choose a friendly ship at range 1-3, it may perform a free {selectedAction.Name} action",
                HostShip
            );
        }

        private void GrantFreeFocusAction()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();
            Selection.ThisShip = TargetShip;
            selectedAction.HostShip = TargetShip;
            TargetShip.AskPerformFreeAction(
                selectedAction,
                AfterFreeFocusAction,
                HostShip.PilotInfo.PilotName,
                $"You may perform a {selectedAction.Name} action",
                HostShip
            );
        }

        private void AfterFreeFocusAction()
        {
            Selection.ThisShip = HostShip;
            Triggers.FinishTrigger();
        }

        private bool FilterTargets(GenericShip ship)
        {
            return FilterByTargetType(ship, TargetTypes.AnyFriendly)
                && FilterTargetsByRange(ship, 1, 3)
                && ship.ActionBar.HasAction(selectedAction.GetType());
        }

        private int GetAiPriority(GenericShip ship)
        {
            return 0;
        }
    }
}
