using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class AutomatedTargetPriority : GenericUpgrade
    {
        public AutomatedTargetPriority() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Automated Target Priority",
                UpgradeType.Tech,
                cost: 1,
                abilityType: typeof(Abilities.SecondEdition.AutomatedTargetPriorityAbility)
            );
        }

        public override bool IsAllowedForShip(GenericShip ship)
        {
            return ship.PilotInfo.Initiative <= 3;
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AutomatedTargetPriorityAbility : GenericAbility
    {
        private int StoredTokens;

        public override void ActivateAbility()
        {
            HostShip.OnTargetForAttackIsAllowed += RestrictByRange;
            HostShip.OnAttackMissedAsAttacker += RegisterStoreToken;
            HostShip.OnCombatActivation += RegisterGainToken;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTargetForAttackIsAllowed -= RestrictByRange;
            HostShip.OnAttackMissedAsAttacker -= RegisterStoreToken;
            HostShip.OnCombatActivation -= RegisterGainToken;
        }

        private void RestrictByRange(GenericShip target, ref bool isAllowed)
        {
            int minRange = int.MaxValue;
            foreach (GenericShip enemyShip in HostShip.Owner.EnemyShips.Values)
            {
                ShotInfo shotInfo = new ShotInfo(HostShip, enemyShip, Combat.ChosenWeapon);
                if (shotInfo.IsShotAvailable && shotInfo.Range < minRange)
                {
                    minRange = shotInfo.Range;
                }
            }

            DistanceInfo targetRange = new(HostShip, target);
            if (targetRange.Range > minRange)
            {
                if (HostShip.Owner.PlayerType != Players.PlayerType.Ai)
                {
                    Messages.ShowError($"Automated Target Priority: You must attack target at range {minRange} instead");
                }
                isAllowed = false;
            }
        }

        private void RegisterStoreToken()
        {
            RegisterAbilityTrigger(TriggerTypes.OnAttackMissed, StoreToken);
        }

        private void StoreToken(object sender, EventArgs e)
        {
            Messages.ShowInfo("Automated Target Priority: Calculate token is stored");
            StoredTokens++;

            HostUpgrade.NamePostfix = $"({StoredTokens})";
            Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);

            Triggers.FinishTrigger();
        }

        private void RegisterGainToken(GenericShip ship)
        {
            if (StoredTokens > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatActivation, AskToGainToken);
            }
        }

        private void AskToGainToken(object sender, EventArgs e)
        {
            AskToUseAbility(
                "Automated Target Priority",
                UseIfNoFocusAndHasShots,
                GainToken,
                descriptionLong: "Do you want to gain stored Calculate token?",
                imageHolder: HostUpgrade,
                showSkipButton: true,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private bool UseIfNoFocusAndHasShots()
        {
            return !HostShip.Tokens.HasToken<FocusToken>()
                && (ActionsHolder.HasTarget(HostShip) || ActionsHolder.CountEnemiesTargeting(HostShip) > 0);
        }

        private void GainToken(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            Messages.ShowInfo("Automated Target Priority: Calculate token is gained");
            StoredTokens--;

            HostUpgrade.NamePostfix = $"({StoredTokens})";
            Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);

            HostShip.Tokens.AssignToken(typeof(CalculateToken), Triggers.FinishTrigger);
        }
    }
}