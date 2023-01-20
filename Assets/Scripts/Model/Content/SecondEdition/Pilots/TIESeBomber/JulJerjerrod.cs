using ActionsList;
using Tokens;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESeBomber
    {
        public class JulJerjerrod : TIESeBomber
        {
            public JulJerjerrod() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Jul Jerjerrod",
                    4,
                    35,
                    isLimited: true,
                    charges: 3,
                    pilotTitle: "Security Commander",
                    abilityType: typeof(Abilities.SecondEdition.JulJerjerrodPilotAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/juljerjerrod.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class JulJerjerrodPilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckAbilityAfterBoost;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckAbilityAfterBoost;
        }

        private void CheckAbilityAfterBoost(GenericAction action)
        {
            if (action is BoostAction && HostShip.State.Charges > 0 && HostShip.Tokens.GetNonLockRedOrangeTokens().Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskUseAbility);
            }
        }
        private void AskUseAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                UseAbility,
                descriptionLong: "Do you want to spend 1 Charge to remove 1 non-lock red or orange token?",
                imageHolder: HostShip
            );
        }
        private void UseAbility(object sender, EventArgs e)
        {
            JulJerjerrodAbilitySubphase subphase = Phases.StartTemporarySubPhaseNew<JulJerjerrodAbilitySubphase>(
                "Remove Token",
                DecisionSubPhase.ConfirmDecision
            );

            subphase.Name = HostShip.PilotInfo.PilotName;
            subphase.DescriptionShort = "Select a non-lock red or orange token to remove";
            subphase.ImageSource = HostShip;

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            HostShip.SpendCharges(1);

            List<GenericToken> tokensToRemove = HostShip.Tokens.GetNonLockRedOrangeTokens();

            foreach (GenericToken token in tokensToRemove)
            {
                subphase.AddDecision(
                    token.Name, 
                    delegate {
                        tokensToRemove.Add(token);
                        ActionsHolder.RemoveTokens(tokensToRemove, DecisionSubPhase.ConfirmDecision);
                    }
                );
            }


            if (subphase.GetDecisions().Count > 0)
            {
                subphase.Start();
            }
            else
            {
                Phases.GoBack();
                Messages.ShowInfoToHuman("Jul Jerjerrod: No tokens to remove");
                Triggers.FinishTrigger();
            }
        }

        private class JulJerjerrodAbilitySubphase : DecisionSubPhase { }
    }
}
