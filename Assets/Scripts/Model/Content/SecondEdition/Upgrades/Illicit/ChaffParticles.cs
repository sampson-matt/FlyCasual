using ActionsList;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class ChaffParticles : GenericUpgrade
    {
        public ChaffParticles() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Chaff Particles",
                UpgradeType.Illicit,
                cost: 3,
                charges: 1,
                restriction: new BaseSizeRestriction(BaseSize.Small),
                abilityType: typeof(Abilities.SecondEdition.ChaffParticlesAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/chaff-particles.jpg";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class ChaffParticlesAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnAfterNeutralizeResults += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAfterNeutralizeResults -= CheckAbility;
        }

        private void CheckAbility()
        {
            if (HostUpgrade.State.Charges > 0 && 
                (HostShip.Tokens.HasTokenByColor(TokenColors.Red) || HostShip.Tokens.HasTokenByColor(TokenColors.Orange)) && 
                (Combat.DiceRollDefence.Focuses > 0 || Combat.DiceRollDefence.Successes > Combat.DiceRollAttack.Successes))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAfterNeutralizeResults, AskToRemove);
            }
        }

        private void AskToRemove(object sender, EventArgs e)
        {

            AskToUseAbility(
                    HostUpgrade.UpgradeInfo.Name,
                    AlwaysUseByDefault,
                    UseAbility,
                    descriptionLong: "Do you want to spend 1 Charge and 1 Focus or Evade result to remove 1 red or orange token?",
                    imageHolder: HostUpgrade
                );
        }

        private void UseAbility(object sender, EventArgs e)
        {
            ChaffPartilcesAbilitySubphase subphase = Phases.StartTemporarySubPhaseNew<ChaffPartilcesAbilitySubphase>(
                "Remove Token",
                DecisionSubPhase.ConfirmDecision
            );

            subphase.Name = HostShip.PilotInfo.PilotName;
            subphase.DescriptionShort = "Select a red or orange token to remove";
            subphase.ImageSource = HostShip;

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            

            List<GenericToken> tokensToRemove = HostShip.Tokens.GetTokensByColor(TokenColors.Red,TokenColors.Orange);

            foreach (GenericToken token in tokensToRemove)
            {
                subphase.AddDecision(
                    token.Name + ((token.GetType() == typeof(RedTargetLockToken)) ? " \"" + (token as RedTargetLockToken).Letter + "\"" : ""),
                    delegate {
                        tokensToRemove.Add(token);
                        ActionsHolder.RemoveTokens(tokensToRemove, delegate { HostUpgrade.State.SpendCharge(); DecisionSubPhase.ConfirmDecision(); });
                    }
                );
            }
            subphase.DefaultDecisionName = subphase.GetDecisions().First().Name;
            subphase.Start();
        }

        private class ChaffPartilcesAbilitySubphase : DecisionSubPhase { }
    }
}

