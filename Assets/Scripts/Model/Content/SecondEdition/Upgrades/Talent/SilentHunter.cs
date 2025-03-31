using Ship;
using SubPhases;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class SilentHunter : GenericUpgrade
    {
        public SilentHunter() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Silent Hunter",
                UpgradeType.Talent,
                cost: 3,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.SilentHunterAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/silenthunter.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class SilentHunterAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnDecloak += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDecloak -= RegisterAbility;
        }

        private void RegisterAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnDecloak, AskAbility);
        }

        private void AskAbility(object sender, System.EventArgs e)
        {
            if (HasTargetsForAbility())
            {
                SelectTargetForAbility(
                    GrantFreeTargetLock,
                    FilterAbilityTargets,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostName,
                    "You may aquire a lock on an enemy in your bullseye arc",
                    HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private bool HasTargetsForAbility()
        {
            return BoardTools.Board.GetShipsInBullseyeArc(HostShip, Team.Type.Enemy).Count > 0;
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            var result = 0;

            var range = new BoardTools.DistanceInfo(HostShip, ship).Range;

            result += (3 - range) * 100;

            result += ship.PilotInfo.Cost;

            return result;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            return BoardTools.Board.GetShipsInBullseyeArc(HostShip, Team.Type.Enemy).Contains(ship);
        }

        private void GrantFreeTargetLock()
        {
            if (TargetShip != null)
            {
                ActionsHolder.AcquireTargetLock(HostShip, TargetShip, SelectShipSubPhase.FinishSelection, SelectShipSubPhase.FinishSelection);
            }
            else
            {
                SelectShipSubPhase.FinishSelection();
            }
        }
    }
}