using ActionsList;
using Bombs;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class TomaxBrenSW98LSL : TIESaBomber
        {
            public TomaxBrenSW98LSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Tomax Bren",
                    5,
                    38,
                     tags: new List<Tags>
                    {
                        Tags.LsL
                    },
                    isLimited: true,
                    charges: 2,
                    regensCharges: 1,
                    abilityType: typeof(Abilities.SecondEdition.TomaxBrenLSLAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "tomaxbren-swz98-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you perform a Barrel Roll action, you may spend 2 charges. If you do, gain a focus token.
    public class TomaxBrenLSLAbility : GenericAbility
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
            if (action is BarrelRollAction && HostShip.State.Charges > 1)
            {
                HostShip.OnActionDecisionSubphaseEnd += RegisterActionTrigger;
            }
        }

        private void RegisterActionTrigger(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= RegisterActionTrigger;

            RegisterAbilityTrigger(TriggerTypes.OnFreeAction, AskToUseOwnAbility);
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                ActivateOwnAbility,
                descriptionLong: "Do you want to spend 2 charges to gain a focus token?",
                imageHolder: HostUpgrade
            );
        }

        private void ActivateOwnAbility(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.Tokens.AssignToken(typeof(Tokens.FocusToken), Cleanup);
        }

        private void Cleanup()
        {
            HostShip.SpendCharges(2);
            Triggers.FinishTrigger();
        }
    }
}