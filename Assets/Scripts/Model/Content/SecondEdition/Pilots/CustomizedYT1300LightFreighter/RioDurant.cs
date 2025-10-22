using ActionsList;
using Ship;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.CustomizedYT1300LightFreighter
    {
        public class RioDurant : CustomizedYT1300LightFreighter
        {
            public RioDurant() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Rio Durant",
                    3,
                    42,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.RioDurantAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "riodurant-wat1";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class RioDurantAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker += RegisterRioDurantAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker -= RegisterRioDurantAbility;
        }

        private void RegisterRioDurantAbility(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskRioDurantAbility);
        }

        private void AskRioDurantAbility(object sender, System.EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);

            AskToUseAbility(
                "Rio Durant",
                NeverUseByDefault,
                UseRioDurantAbility,
                descriptionLong: "Do you want to rotate your turret arc indicator?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void UseRioDurantAbility(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            new RotateArcAction().DoOnlyEffect(Triggers.FinishTrigger);
        }
    }
}