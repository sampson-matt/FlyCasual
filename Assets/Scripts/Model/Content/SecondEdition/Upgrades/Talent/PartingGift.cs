using BoardTools;
using Bombs;
using Movement;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class PartingGift : GenericUpgrade
    {
        public PartingGift() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Parting Gift",
                UpgradeType.Talent,
                cost: 1,
                abilityType: typeof(Abilities.SecondEdition.PartingGiftAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/partinggift.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PartingGiftAbility : GenericAbility
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
                descriptionLong: "Do you want to place 1 bomb in the play area touching your ship?",
                useByDefault: NeverUseByDefault,
                useAbility: DropBomb,
                imageHolder: HostUpgrade
            );
        }

        private void PlaceBomb()
        {
            BombsManager.IsOverriden = true;

            PlaceBombTokenSubphase subphase = Phases.StartTemporarySubPhaseNew<PlaceBombTokenSubphase>("Place the bomb", Triggers.FinishTrigger);
            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "Place the bomb touching your ship";
            subphase.ImageSource = HostShip;

            subphase.Start();
        }

        private void DropBomb(object sender, System.EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.OnBombWillBeDropped += PlaceBomb;

            BombsManager.RegisterBombDropTriggerIfAvailable(
                HostShip,
                TriggerTypes.OnAbilityDirect,
                subType: UpgradeSubType.Bomb,
                isRealDrop: false
            );

            Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, delegate { });
        }
    }
}