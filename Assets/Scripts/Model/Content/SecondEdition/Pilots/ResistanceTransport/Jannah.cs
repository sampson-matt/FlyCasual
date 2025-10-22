using Abilities.SecondEdition;
using ActionsList;
using Ship;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ResistanceTransport
    {
        public class Jannah : ResistanceTransport
        {
            public Jannah() : base()
            {
                IsHidden = true;
                PilotInfo = new PilotCardInfo(
                    "Jannah",
                    5,
                    42,
                    isLimited: true,
                    abilityText: "After you perform an action added to your action bar by a crew upgrade, you may perform a reinforce action.",
                    abilityType: typeof(JannahAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "jannah-wat1";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class JannahAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckConditions;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckConditions;
        }

        private void CheckConditions(GenericAction action)
        {
            if (HostShip.UpgradeBar.HasUpgradeTypeInstalled(UpgradeType.Crew) 
                && action.Source != null 
                && action.Source.Slot.Type == UpgradeType.Crew)
            {
                HostShip.OnActionDecisionSubphaseEnd += DoReinforceAction;
            }
        }

        private void DoReinforceAction(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= DoReinforceAction;

            RegisterAbilityTrigger(TriggerTypes.OnFreeAction, PerformAction);
        }

        private void PerformAction(object sender, System.EventArgs e)
        {
            HostShip.AskPerformFreeAction(
                new ReinforceAction(),
                Triggers.FinishTrigger,
                HostShip.PilotInfo.PilotName,
                "After you perform an action added to your action bar by a crew upgrade, you may perform a reinforce action.",
                HostShip
            );
        }
    }
}
