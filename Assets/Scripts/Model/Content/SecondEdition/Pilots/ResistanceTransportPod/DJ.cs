using Abilities.SecondEdition;
using ActionsList;
using Tokens;
using Upgrade;

namespace Ship.SecondEdition.ResistanceTransportPod
{
    public class DJ : ResistanceTransportPod
    {
        public DJ()
        {
            PilotInfo = new PilotCardInfo(
                "DJ",
                2,
                25,
                isLimited: true,
                abilityType: typeof(DJAbility),
                extraUpgradeIcon: UpgradeType.Illicit
            );
            PilotNameCanonical = "dj-wat1";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DJAbility : GenericAbility
    {
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
            if (action is JamAction)
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskToCloak);
            }
        }

        private void AskToCloak(object sender, System.EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);

            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                GainCloakToken,
                descriptionLong: "Do you want to gain a Cloak Token?"
            );
        }

        private void GainCloakToken(object sender, System.EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.Tokens.AssignToken(typeof(CloakToken), Triggers.FinishTrigger);
        }
    }
}