using ActionsList;
using Arcs;
using Ship;
using SubPhases;
using System.Collections.Generic;
using Tokens;

namespace Ship
{
    namespace SecondEdition.UpsilonClassCommandShuttle
    {
        public class LieutenantTavson : UpsilonClassCommandShuttle
        {
            public LieutenantTavson() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lieutenant Tavson",
                    3,
                    64,
                    isLimited: true,
                    charges: 2,
                    regensCharges: 1,
                    abilityType: typeof(Abilities.SecondEdition.LieutenantTavsonPilotAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you suffer damage, you may spend 1 charge to perform an action.
    public class LieutenantTavsonPilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnDamageWasSuccessfullyDealt += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDamageWasSuccessfullyDealt -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship, bool flag)
        {
            if (HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnDamageWasSuccessfullyDealt, PerformAction);
            }
        }
        private void PerformAction(object sender, System.EventArgs e)
        {
            var previousSelectedShip = Selection.ThisShip;
            Selection.ChangeActiveShip(HostShip);

            Messages.ShowInfoToHuman(HostName + ": you may spend 1 charge to perform an action");

            HostShip.AskPerformFreeAction(
                HostShip.GetAvailableActions(),
                delegate
                {
                    Selection.ChangeActiveShip(previousSelectedShip);
                    CleanUp();
                },
                HostShip.PilotInfo.PilotName,
                "After you suffer damage, you may spend 1 Charge to perform an action",
                HostShip,
                skipActionCallback: Triggers.FinishTrigger
            );
        }

        private void CleanUp()
        {
            HostShip.SpendCharge();
            Triggers.FinishTrigger();
        }
    }
}
