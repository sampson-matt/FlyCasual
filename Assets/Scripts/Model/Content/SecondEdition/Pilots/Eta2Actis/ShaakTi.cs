using Abilities.Parameters;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;
using Content;

namespace Ship.SecondEdition.Eta2Actis
{
    public class ShaakTi : Eta2Actis
    {
        public ShaakTi()
        {
            PilotInfo = new PilotCardInfo(
                "Shaak Ti",
                4,
                45,
                true,
                force: 2,
                abilityType: typeof(Abilities.SecondEdition.ShaakTiAbility),
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.Talent
            );

            ModelInfo.SkinName = "Red";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ShaakTiAbility : TriggeredAbility
    {
        private List<GenericToken> TokensToKeep = new List<GenericToken>();

        public override TriggerForAbility Trigger => new AtTheStartOfPhase(typeof(SubPhases.EndStartSubPhase));

        public override AbilityPart Action => new EachShipCanDoAction
        (
            eachShipAction: SelectFocusOrEvadeToken,
            conditions: new ConditionsBlock
            (
                new TeamCondition(ShipTypes.Friendly),
                new RangeToHostCondition(minRange: 0, maxRange: 2),
                new CanSpendForceCondition(),
                new HasTokenCondition(tokensList: new List<Type>() {typeof(FocusToken), typeof(EvadeToken)})
            ),
            description: new AbilityDescription
            (
                "Shaak Ti",
                "Spend Force - friendly ships may choose Focus or Evade token to keep",
                imageSource: HostShip
            )
        );

        private void SelectFocusOrEvadeToken(GenericShip ship, Action callback)
        {
            HostShip.State.SpendForce(
                1,
                delegate
                {
                    if (TargetShip.Tokens.HasToken<FocusToken>() && TargetShip.Tokens.HasToken<EvadeToken>())
                    {
                        AskToUseAbility(
                            "Choose token to keep",
                            AlwaysUseByDefault,
                            useAbility: delegate { KeepFocus(callback); },
                            dontUseAbility: delegate { KeepEvade(callback); },
                            descriptionLong: "Do you want to keep Focus token (otherwise Evade token will be kept)",
                            showSkipButton: false,
                            requiredPlayer: HostShip.Owner.PlayerNo
                        );
                    }
                    else
                    {
                        if (TargetShip.Tokens.HasToken<FocusToken>())
                        {
                            KeepToken(TargetShip.Tokens.GetToken<FocusToken>());
                        }
                        else if (TargetShip.Tokens.HasToken<EvadeToken>())
                        {
                            KeepToken(TargetShip.Tokens.GetToken<EvadeToken>());
                        }

                        callback();
                    }
                }
            );            
        }

        private void KeepEvade(Action callback)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            KeepToken(TargetShip.Tokens.GetToken<EvadeToken>());
            callback();
        }

        private void KeepFocus(Action callback)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            KeepToken(TargetShip.Tokens.GetToken<FocusToken>());
            callback();
        }

        private void KeepToken(GenericToken token)
        {
            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: {token.Host.PilotInfo.PilotName} doesn't remove {token.Name} during End Phase");

            token.Temporary = false;
            TokensToKeep.Add(token);

            Phases.Events.OnPlanningPhaseStart += Cleanup;
        }

        private void Cleanup()
        {
            Phases.Events.OnPlanningPhaseStart -= Cleanup;

            List<GenericToken> TokensToKeepCopy = new List<GenericToken>(TokensToKeep);
            foreach (GenericToken token in TokensToKeepCopy)
            {
                token.Temporary = true;
                TokensToKeep.Remove(token);
            }
        }
    }
}
