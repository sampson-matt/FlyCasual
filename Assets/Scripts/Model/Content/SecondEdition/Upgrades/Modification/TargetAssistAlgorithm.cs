using BoardTools;
using Ship;
using System;
using Tokens;
using System.Linq;
using UnityEngine;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class TargetAssistAlgorithm : GenericUpgrade
    {
        public TargetAssistAlgorithm() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Target-Assist Algorithm",
                UpgradeType.Modification,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.TargetAssistAlgorithmAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/TargetAssistAlgorithm.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TargetAssistAlgorithmAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCombatActivation += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCombatActivation -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatActivation, CheckConditions);
        }

        private void CheckConditions(object sender, EventArgs e)
        {
            if (!HostShip.Tokens.HasGreenTokens && Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.Front, new Vector2(0, 3),Team.Type.Enemy).Count > 0)
            {
                Messages.ShowInfo("Target-Assist Algorithm: " + HostShip.PilotInfo.PilotName + " gains a Calculate token)");
                HostShip.Tokens.AssignTokens(CreateCalculateToken, 1, Triggers.FinishTrigger);
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private GenericToken CreateCalculateToken()
        {
            return new CalculateToken(HostShip);
        }
    }
}