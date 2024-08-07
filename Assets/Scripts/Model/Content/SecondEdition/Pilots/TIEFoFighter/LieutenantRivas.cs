﻿using BoardTools;
using Ship;
using SubPhases;
using System;
using Tokens;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class LieutenantRivas : TIEFoFighter
        {
            public LieutenantRivas() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lieutenant Rivas",
                    1,
                    28,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.LieutenantRivasAbility)
                );
            }
        }
    }
}


namespace Abilities.SecondEdition
{
    public class LieutenantRivasAbility : GenericAbility
    {
        private GenericShip ShipWithAssignedToken;

        public override void ActivateAbility()
        {
            GenericShip.OnTokenIsAssignedGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnTokenIsAssignedGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, GenericToken token)
        {
            // To avoid infinite loop
            if (IsAbilityUsed) return;

            if (Tools.IsSameTeam(ship, HostShip)) return;

            if (ActionsHolder.HasTargetLockOn(HostShip, ship)) return;

            if (token.TokenColor != TokenColors.Red && token.TokenColor != TokenColors.Orange) return;

            DistanceInfo distInfo = new DistanceInfo(HostShip, ship);
            if (distInfo.Range == 1 || distInfo.Range == 2)
            {
                ShipWithAssignedToken = ship;
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, AskAcquireTargetLock);
            }
        }

        private void AskAcquireTargetLock(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                ShouldAbilityBeUsed,
                AcquireTargetLock,
                descriptionLong: "Do you want to acquire a Lock on " + ShipWithAssignedToken.PilotInfo.PilotName + "?",
                imageHolder: HostShip
            );
        }

        private bool ShouldAbilityBeUsed()
        {
            return (!HostShip.Tokens.HasToken<BlueTargetLockToken>(letter: '*'));
        }

        private void AcquireTargetLock(object sender, System.EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            IsAbilityUsed = true;
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " acquired a Lock on " + ShipWithAssignedToken.PilotInfo.PilotName);
            ActionsHolder.AcquireTargetLock(HostShip, ShipWithAssignedToken, FinishAbility, FinishAbility);
        }

        private void FinishAbility()
        {
            IsAbilityUsed = false;
            Triggers.FinishTrigger();
        }
    }
}