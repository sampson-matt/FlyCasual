using BoardTools;
using Bombs;
using Movement;
using Ship;
using SubPhases;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class PartingGiftBoE : GenericUpgrade
    {
        public PartingGiftBoE() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Parting Gift",
                UpgradeType.Talent,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.PartingGiftBoEAbility)
            );

            IsHidden = true;

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/PartingGiftBoE.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PartingGiftBoEAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnShipIsDestroyed += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnShipIsDestroyed -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship, bool isFled)
        {
            if (!isFled && HasBombsToDrop())
            {
                RegisterAbilityTrigger(TriggerTypes.OnShipIsDestroyed, AskToUsePartingGiftAbility);
            }
        }

        private bool HasBombsToDrop()
        {
            return HostShip.UpgradeBar.GetUpgradesAll().Any(n =>
                n is GenericBomb
                && (n as GenericBomb).UpgradeInfo.SubType == UpgradeSubType.Bomb
                && n.State.Charges > 0
            );
        }

        private void AskToUsePartingGiftAbility(object sender, System.EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);
            AskToUseAbility
            (
                descriptionShort: HostUpgrade.UpgradeInfo.Name,
                descriptionLong: "Do you want to drop or launch a bomb?",
                useByDefault: NeverUseByDefault,
                useAbility: DropBomb,
                imageHolder: HostUpgrade
            );
        }

        private void DropBomb(object sender, System.EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.OnGetAvailableBombDropTemplatesOneCondition += AddNimbleBomberTemplates;
            HostShip.OnGetAvailableBombLaunchTemplates += TrajectorySimulatorTemplate;

            BombsManager.RegisterBombDropTriggerIfAvailable(
                HostShip,
                TriggerTypes.OnAbilityDirect,
                subType: UpgradeSubType.Bomb,
                isRealDrop: false
            );

            Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, Triggers.FinishTrigger);
        }

        private void AddNimbleBomberTemplates(List<ManeuverTemplate> availableTemplates, GenericUpgrade upgrade)
        {
            if (upgrade.UpgradeInfo.SubType != UpgradeSubType.Bomb) return;

            List<ManeuverTemplate> templatesCopy = new List<ManeuverTemplate>(availableTemplates);

            foreach (ManeuverTemplate existingTemplate in templatesCopy)
            {
                if (existingTemplate.Bearing == ManeuverBearing.Straight && existingTemplate.Direction == ManeuverDirection.Forward)
                {
                    List<ManeuverTemplate> newTemplates = new List<ManeuverTemplate>()
                    {
                        new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Right, existingTemplate.Speed, isBombTemplate: true),
                        new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Left, existingTemplate.Speed, isBombTemplate: true),
                    };

                    foreach (ManeuverTemplate newTemplate in newTemplates)
                    {
                        if (!availableTemplates.Any(t => t.Name == newTemplate.Name))
                        {
                            availableTemplates.Add(newTemplate);
                        }
                    }
                }
            }
        }

        protected virtual void TrajectorySimulatorTemplate(List<ManeuverTemplate> availableTemplates, GenericUpgrade upgrade)
        {
            if (upgrade.UpgradeInfo.SubType != UpgradeSubType.Bomb) return;

            List<ManeuverTemplate> newTemplates = new List<ManeuverTemplate>()
            {
                new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Right, ManeuverSpeed.Speed1),
                new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Left, ManeuverSpeed.Speed1),
                new ManeuverTemplate(ManeuverBearing.Straight, ManeuverDirection.Forward, ManeuverSpeed.Speed1),
            };
            foreach (ManeuverTemplate newTemplate in newTemplates)
            {
                if (!availableTemplates.Any(t => t.Name == newTemplate.Name))
                {
                    availableTemplates.Add(newTemplate);
                }
            }
        }
    }
}