using BoardTools;
using Ship;
using SubPhases;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.Fireball
    {
        public class TorraDoza : Fireball
        {
            public TorraDoza() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Torra Doza",
                    3,
                    27,
                    isLimited: true,
                    abilityText: "While a friendly ship at range 0-3 exposes a damage card, you may gain a deplete or stress token to repair that card without resolving its effects.",
                    abilityType: typeof(Abilities.SecondEdition.TorraDozaAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Astromech }
                );
                PilotNameCanonical = "torradoza-wat1";
            }
        }
    }
}

namespace Abilities.SecondEdition
{

    public class TorraDozaAbility : GenericAbility
    {
        GenericShip damagedShip;
        GenericDamageCard critCard;
        private bool abilityUsed = false;
        public override void ActivateAbility()
        {
            GenericShip.OnCritExposedGlobal += RegisterTorraDozaTrigger;
            HostShip.OnTokenIsRemoved += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnCritExposedGlobal -= RegisterTorraDozaTrigger;
            HostShip.OnTokenIsRemoved -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, GenericToken token)
        {
            if (token.TokenColor == TokenColors.Red && !abilityUsed)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsRemoved, delegate { removeAdditionalToken(token); }) ;
            }
        }

        private void removeAdditionalToken(GenericToken token)
        {
            abilityUsed = true;
            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: removes 1 additional {token.Name}.");
            HostShip.Tokens.RemoveToken(token.GetType(), CleanupAbilityUsed); 
        }

        private void CleanupAbilityUsed()
        {
            Triggers.FinishTrigger();
            abilityUsed = false;
        }

        private bool hasAdditionalTokensOfType(GenericToken token)
        {
            return HostShip.Tokens.CountTokensByType(token.GetType()) > 0;
        }

        private void RegisterTorraDozaTrigger(GenericShip ship, bool isFaceUp)
        {
            if (isFaceUp
                && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0,3),Team.Type.Friendly).Contains(ship))
            {
                damagedShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnCritExposed, AskUseTorraDozaAbility);
            }
        }        

        private void AskUseTorraDozaAbility(object sender, System.EventArgs e)
        {
            TorraDozaDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<TorraDozaDecisionSubPhase>("Torra Doza Decision Subphase", Triggers.FinishTrigger);

            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "You may gain a deplete or stress token to flip that card facedown instead of resolving its effects:";
            subphase.ImageSource = HostShip;

            subphase.AddDecision("Gain a deplete", delegate { HandleFaceupCardDeplete(); });
            subphase.AddDecision("Gain a stress", delegate { HandleFaceupCardStress(); });

            subphase.DefaultDecisionName = "Gain a deplete";

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            subphase.Start();
        }

        private void HandleFaceupCardDeplete()
        {
            HostShip.Tokens.AssignToken(typeof(DepleteToken), cleanup);
        }

        private void HandleFaceupCardStress()
        {
            HostShip.Tokens.AssignToken(typeof(StressToken), cleanup);
        }

        private void cleanup()
        {
            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: {Combat.CurrentCriticalHitCard.Name} is flipped facedown instead of resolving its effects");
            Combat.CurrentCriticalHitCard.IsFaceup = false;
            DecisionSubPhase.ConfirmDecision();
        }

        private class TorraDozaDecisionSubPhase : DecisionSubPhase { }
    }
}