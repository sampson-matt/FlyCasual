using Actions;
using ActionsList;
using Upgrade;
using Ship;
using System;
using SubPhases;
using BoardTools;

namespace UpgradesList.SecondEdition
{
    public class AntiPursuitLasers : GenericUpgrade
    {
        public AntiPursuitLasers() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Anti-Pursuit Lasers",
                UpgradeType.Modification,
                cost: 4,
                abilityType: typeof(Abilities.SecondEdition.AntiPursuitLasersAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/modification/antipursuitlasers.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AntiPursuitLasersAbility : GenericAbility
    {
        private GenericShip targetShip;
        public override void ActivateAbility()
        {
            HostShip.OnMovementBumped += RegisterAntiPursuitLasersAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementBumped -= RegisterAntiPursuitLasersAbility;
        }

        private void RegisterAntiPursuitLasersAbility(GenericShip ship)
        {
            if (ship.Owner.Id == HostShip.Owner.Id)
                return;
            targetShip = ship;
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = HostShip.PilotInfo.PilotName,
                TriggerType = TriggerTypes.OnMovementFinish,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = StartRollDiceSubphase
            });
        }

        private void StartRollDiceSubphase(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostUpgrade.UpgradeInfo.Name + " firing at " + targetShip.PilotInfo.PilotName);
            PerformDiceCheck(
                HostShip.PilotInfo.PilotName + ": Facedown damage card on hit",
                DiceKind.Attack,
                1,
                FinishAction,
                Triggers.FinishTrigger
            );
        }

        private void FinishAction()
        {
            if (DiceCheckRoll.Successes > 0)
            {
                SufferNegativeEffect(AbilityDiceCheck.ConfirmCheck);
            }
            else
            {
                AbilityDiceCheck.ConfirmCheck();
            }
        }

        protected virtual void SufferNegativeEffect(Action callback)
        {
            targetShip.Damage.SufferRegularDamage(
                new DamageSourceEventArgs()
                {
                    Source = HostShip,
                    DamageType = DamageTypes.CardAbility
                },
                callback
            );
        }
    }
}