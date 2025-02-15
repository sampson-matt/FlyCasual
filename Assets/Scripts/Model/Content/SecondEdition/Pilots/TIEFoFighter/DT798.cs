﻿using ActionsList;
using Ship;
using SubPhases;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class DT798 : TIEFoFighter
        {
            public DT798() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "DT-798",
                    4,
                    34,
                    pilotTitle: "Jace Rucklin",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DT798PilotAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DT798PilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnShotStartAsAttacker += CheckAbilityTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnShotStartAsAttacker -= CheckAbilityTrigger;
        }

        private void CheckAbilityTrigger()
        {
            if (IsAvailable())
            {
                RegisterAbilityTrigger(TriggerTypes.OnShotStart, AskUseAbility);
            }
        }

        private bool IsAvailable()
        {
            if (HostShip.IsStrained) return false;
            if (Combat.ChosenWeapon.WeaponType != WeaponTypes.PrimaryWeapon) return false;

            return true;
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                AlwaysUseByDefault,
                UseAbilityDecision,
                descriptionLong: "Do you want to gain 1 Strain to roll 1 additional attack die?",
                imageHolder: HostShip
            );
        }

        private void UseAbilityDecision(object sender, EventArgs e)
        {
            AllowRollAdditionalDie();
            HostShip.Tokens.AssignToken(
                typeof(Tokens.StrainToken),
                DecisionSubPhase.ConfirmDecision
            );
        }

        private void AllowRollAdditionalDie()
        {
            HostShip.AfterGotNumberOfAttackDice += RollExtraDie;
        }

        protected void RollExtraDie(ref int diceCount)
        {
            HostShip.AfterGotNumberOfAttackDice -= RollExtraDie;
            Messages.ShowInfo(HostName + ": +1 attack die");
            diceCount++;
        }
    }
}