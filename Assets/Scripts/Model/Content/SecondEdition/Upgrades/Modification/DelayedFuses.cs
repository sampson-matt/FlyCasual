using System;
using ActionsList;
using Bombs;
using Ship;
using SubPhases;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class DelayedFuses : GenericUpgrade
    {
        public DelayedFuses() : base()
        {
            UpgradeInfo = new UpgradeCardInfo("Delayed Fuses",
                UpgradeType.Modification,
                cost: 1,
                abilityType: typeof(Abilities.SecondEdition.DelayedFusesAbility)
            );
        }
        
    }
}

namespace Abilities.SecondEdition
{
    // After dropping, launching or placing a bomb or mine you may place 1 fuse marker on that device.
    public class DelayedFusesAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnBombWasDropped += OnDeviceDropped;
            HostShip.OnBombWasLaunched += OnDeviceLaunched;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnBombWasDropped -= OnDeviceDropped;
            HostShip.OnBombWasLaunched -= OnDeviceLaunched;
        }

        private void OnDeviceLaunched()
        {
            RegisterAbilityTrigger(TriggerTypes.OnBombWasLaunched, (s, e) => AskPlaceFuseMarker());
        }

        private void OnDeviceDropped()
        {
            RegisterAbilityTrigger(TriggerTypes.OnBombWasDropped, (s, e) => AskPlaceFuseMarker());
        }

        private void AskPlaceFuseMarker()
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                useByDefault: AlwaysUseByDefault,
                useAbility: AddFuseMarker,
                descriptionLong: $"Do you want to place a Fuse marker on {BombsManager.CurrentDevice.UpgradeInfo.Name}?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        protected void AddFuseMarker(object sender, EventArgs e)
        {
            BombsManager.CurrentBombObject.Fuses++;
            DecisionSubPhase.ConfirmDecision();
        }

    }
}
