using BoardTools;
using Ship;
using System;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class RoilingAnger : GenericUpgrade
    {
        public RoilingAnger() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Roiling Anger",
                UpgradeType.ForcePower,
                cost: 3,
                restriction: new TagRestriction(Content.Tags.DarkSide),
                abilityType: typeof(Abilities.SecondEdition.RoilingAngerAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/RoilingAnger.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class RoilingAngerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= CheckAbility;
        }

        private void CheckAbility()
        {
            if (HostShip.State.Force < HostShip.State.MaxForce &&
                isInEnemyArc()) 
            {
                RegisterAbilityTrigger
                    (
                        TriggerTypes.OnCombatPhaseStart,
                        AskToRecoverCharge,
                        customTriggerName: $"{HostShip.PilotInfo.PilotName}: {HostUpgrade.UpgradeInfo.Name}"
                    );
            }
        }

        private bool isInEnemyArc()
        {
            foreach (GenericShip enemyShip in HostShip.Owner.AnotherPlayer.Ships.Values)
            {
                if (enemyShip.SectorsInfo.IsShipInSector(HostShip, Arcs.ArcType.Front))
                {
                    return true;
                }
            }
            return false;
        }

        private void AskToRecoverCharge(object sender, EventArgs e)
        {
            AskToUseAbility
            (
                HostUpgrade.UpgradeInfo.Name,
                ShouldAiUseAbility,
                UseRoilingAngerAbility,
                descriptionLong: "You may gain 1 strain token to recover 1 force charge",
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private bool ShouldAiUseAbility()
        {
            return HostShip.State.Force < HostShip.State.MaxForce;
        }

        private void UseRoilingAngerAbility(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName} gains 1 strain token to recover 1 force charge");
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), RecoverForce);
        }

        private void RecoverForce()
        {
            HostShip.State.RestoreForce();
            Triggers.FinishTrigger();
        }
    }
}