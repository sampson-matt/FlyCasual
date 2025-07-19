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
    public class TargetingMatrix : GenericUpgrade
    {
        public TargetingMatrix() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Targeting Matrix",
                UpgradeType.Modification,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.TargetingMatrixAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/ChaffParticles.jpg";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class TargetingMatrixAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnAfterNeutralizeResultsAttacker += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAfterNeutralizeResultsAttacker -= CheckAbility;
        }

        private void CheckAbility()
        {
            if (Combat.DiceRollAttack.Focuses > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAfterNeutralizeResultsAttacker, AskToStrain);
            }
        }

        private void AskToStrain(object sender, EventArgs e)
        {

            AskToUseAbility(
                    HostUpgrade.UpgradeInfo.Name,
                    AlwaysUseByDefault,
                    UseAbility,
                    descriptionLong: "Do you want to spend one focus result to assign a strain token to the defender?",
                    imageHolder: HostUpgrade
                );
        }

        private void UseAbility(object sender, EventArgs e)
        {
            Combat.Defender.Tokens.AssignToken(new StrainToken(Combat.Defender), DecisionSubPhase.ConfirmDecision);
        }
    }
}

