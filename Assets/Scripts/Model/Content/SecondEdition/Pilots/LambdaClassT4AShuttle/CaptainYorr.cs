using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Tokens;
using Players;

namespace Ship
{
    namespace SecondEdition.LambdaClassT4AShuttle
    {
        public class CaptainYorr : LambdaClassT4AShuttle
        {
            public CaptainYorr() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Yorr",
                    2,
                    47,
                    charges: 2,
                    regensCharges: 1,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaptainYorrAbility)
                );
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/CaptainYorr.png";
                PilotNameCanonical = "captainyorr-lambda";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CaptainYorrAbility : GenericAbility
    {
        GenericToken CurrentToken;
        //When a friendly ship at range 0-3 would receive a non-lock red or orange token, if you have no matching tokens, you may spend 2 [charge] to gain that token instead.
        public override void ActivateAbility()
        {
            GenericShip.BeforeTokenIsAssignedGlobal += CaptainYorrPilotAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.BeforeTokenIsAssignedGlobal -= CaptainYorrPilotAbility;
        }

        private void CaptainYorrPilotAbility(GenericShip ship, GenericToken token)
        {
            if (IsOrangeOrRedNonLock(token)
                && HostShip.State.Charges >= 2
                && Tools.IsFriendly(ship, HostShip)
                && ship != HostShip
                && !hasTokensOfType(token))
            {
                BoardTools.DistanceInfo positionInfo = new BoardTools.DistanceInfo(ship, HostShip);
                if (positionInfo.Range <= 3)
                {
                    CurrentToken = token;
                    TargetShip = ship;
                    RegisterAbilityTrigger(TriggerTypes.OnBeforeTokenIsAssigned, ShowDecision);
                }
            }
        }

        private bool hasTokensOfType(GenericToken token)
        {
            foreach (GenericToken hostToken in HostShip.Tokens.GetNonLockRedOrangeTokens())
            {
                if (hostToken.Name == token.Name) return true;
            }
            return false;
        }

        private bool IsOrangeOrRedNonLock(GenericToken token)
        {
            if (token is null) return false;
            if (token.TokenColor == TokenColors.Orange) return true;
            if (token.TokenColor == TokenColors.Red && !(token is RedTargetLockToken)) return true;

            return false;
        }

        private void ShowDecision(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                NeverUseByDefault,
                UseCaptainYorrAbility,
                descriptionLong: $"Do you want to spend 2 charges to receive {CurrentToken.Name} instead of the {TargetShip.PilotInfo.PilotName}?",
                imageHolder: HostShip
            );
        }

        private void UseCaptainYorrAbility(object sender, System.EventArgs e)
        {
            GenericPlayer assigner = determineAssigner();
            HostShip.SpendCharges(2);
            HostShip.Tokens.AssignToken(TargetShip.Tokens.TokenToAssign, delegate {
                TargetShip.Tokens.TokenToAssign = null;
                TargetShip = null;
                DecisionSubPhase.ConfirmDecision();
            });
        }

        private GenericPlayer determineAssigner()
        {
            if(CurrentToken is JamToken)
            {
                return (CurrentToken as JamToken).Assigner;
            }
            if (CurrentToken is TractorBeamToken)
            {
                return (CurrentToken as TractorBeamToken).Assigner;
            }
            return null;
        }
    }
}
