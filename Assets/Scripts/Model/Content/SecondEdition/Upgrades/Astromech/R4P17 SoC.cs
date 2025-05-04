using Ship;
using SubPhases;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class R4P17SoC : GenericUpgrade
    {
        public R4P17SoC() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "R4-P17",
                UpgradeType.Astromech,
                cost: 0,
                charges: 2,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.R4P17SoCAbility),
                restriction: new FactionRestriction(Faction.Republic)
            );

            NameCanonical = "r4p17-soc";
        }
    }
}

namespace Abilities.SecondEdition
{
    //When you would be dealt a damage card, if you are not defending, you may spend 1 charge and gain 1 strain to discard it instead.
    public class R4P17SoCAbility : GenericAbility
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
            if (HostUpgrade.State.Charges > 0 
                && ((Phases.CurrentPhase is MainPhases.CombatPhase && !Tools.IsSameShip(ship, Combat.Defender))
                || !(Phases.CurrentPhase is MainPhases.CombatPhase)))
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
                descriptionLong: "Do you want to spend 1 Charge to discard current Damage card?",
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