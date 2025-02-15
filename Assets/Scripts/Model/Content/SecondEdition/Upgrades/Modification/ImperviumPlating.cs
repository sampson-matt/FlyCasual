﻿using Upgrade;
using Ship;
using SubPhases;

namespace UpgradesList.SecondEdition
{
    public class ImperviumPlating : GenericUpgrade
    {
        public ImperviumPlating() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Impervium Plating",
                UpgradeType.Modification,
                cost: 4,
                restriction: new ShipRestriction(typeof(Ship.SecondEdition.Belbullab22Starfighter.Belbullab22Starfighter)),
                abilityType: typeof(Abilities.SecondEdition.ImperviumPlatingAbility),
                charges: 2
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    //Before you would be dealt a faceup Ship damage card, you may spend 1 charge to discard it instead.
    public class ImperviumPlatingAbility : GenericAbility 
    {
        public override void ActivateAbility()
        {
            HostShip.OnDamageCardIsDealt += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDamageCardIsDealt -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            if (HostUpgrade.State.Charges > 0 && Combat.CurrentCriticalHitCard.IsFaceup && Combat.CurrentCriticalHitCard.Type == CriticalCardType.Ship)
            {
                Triggers.RegisterTrigger(new Trigger()
                {
                    Name = HostName,
                    TriggerType = TriggerTypes.OnDamageCardIsDealt,
                    TriggerOwner = ship.Owner.PlayerNo,
                    EventHandler = AskUseAbility,
                    Sender = ship
                });
            }
        }

        private void AskUseAbility(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                AlwaysUseByDefault,
                DiscardDamage,
                null,
                Triggers.FinishTrigger,
                descriptionLong: "Do you want to spend 1 Charge to discard current Faceup Ship Damage card?",
                imageHolder: HostUpgrade
            );
        }

        private void DiscardDamage(object sender, System.EventArgs e)
        {
            Messages.ShowInfo(HostName + " discards " + Combat.CurrentCriticalHitCard.Name);
            Combat.CurrentCriticalHitCard = null;
            HostUpgrade.State.SpendCharge();
            DecisionSubPhase.ConfirmDecision();
        }
    }
}