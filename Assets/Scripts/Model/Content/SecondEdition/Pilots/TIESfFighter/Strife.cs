using BoardTools;
using Ship;
using SubPhases;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESfFighter
    {
        public class Strife : TIESfFighter
        {
            public Strife() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Strife\"",
                    3,
                    34,
                    isLimited: true,
                    extraUpgradeIcon: UpgradeType.Talent,
                    abilityType: typeof(Abilities.SecondEdition.StrifeAbility)
                );

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/Strife.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class StrifeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackFinishGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackFinishGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            if (Tools.IsAnotherTeam(Combat.Defender, HostShip)
                && !HostShip.IsStrained
                && HostShip.ArcsInfo.HasShipInTurretArc(Combat.Defender))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskToGainStrainToLock);
            }
        }

        private void AskToGainStrainToLock(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                GainStrainFirst,
                descriptionLong: "Do you want to gain Strain token to acquire a lock on the defender?",
                imageHolder: HostShip,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void GainStrainFirst(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.Tokens.AssignToken(
                typeof(Tokens.StrainToken),
                AcquireLockOnDefender
            );
        }

        private void AcquireLockOnDefender()
        {
            ActionsHolder.AcquireTargetLock(
                HostShip,
                Combat.Defender,
                Triggers.FinishTrigger,
                Triggers.FinishTrigger
            );
        }

    }
}
