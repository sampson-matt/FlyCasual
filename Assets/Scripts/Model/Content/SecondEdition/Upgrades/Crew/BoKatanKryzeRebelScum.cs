using Ship;
using Upgrade;
using System;
using Tokens;
using SubPhases;
using ActionsList;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class BoKatanKryzeRebelScum : GenericUpgrade
    {
        public BoKatanKryzeRebelScum() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Bo-Katan Kryze",
                UpgradeType.Crew,
                cost: 2,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Rebel, Faction.Scum),
                abilityType: typeof(Abilities.SecondEdition.BoKatanKryzeRebelScumAbility)
            );

            NameCanonical = "bokatankryze-rebel-scum";
        }
    }
}


namespace Abilities.SecondEdition
{
    public class BoKatanKryzeRebelScumAbility : GenericAbility
    {
        private GenericShip PreviousCurrentShip { get; set; }
        //After you perform an attack, if the defender was destroyed,
        //each friendly ship at range 0-2 may remove 1 red or orange token. 
        public override void ActivateAbility()
        {
            GenericShip.OnShipIsDestroyedGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnShipIsDestroyedGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, bool flag)
        {
            if (Tools.IsSameShip(ship, Combat.Defender)
                && Tools.IsSameShip(HostShip, Combat.Attacker))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskEachToRemoveToken);
            }
        }

        private void AskEachToRemoveToken(object sender, EventArgs e)
        {
            PreviousCurrentShip = Selection.ThisShip;

            EachShipCanDoAction action = new EachShipCanDoAction
            (
                EachShipAction,
                onFinish: FinishAbility,
                conditions: new ConditionsBlock
                (
                    new TeamCondition(ShipTypes.Friendly),
                    new RangeToHostCondition(0, 2),
                    new OrCondition
                    (
                        new HasTokenCondition(tokenColor: TokenColors.Orange),
                        new HasTokenCondition(tokenColor: TokenColors.Red)
                    )
                ),
                description: new Abilities.Parameters.AbilityDescription
                (
                    HostUpgrade.UpgradeInfo.Name,
                    $"Each friendly ship at range 0-2 may remove 1 red or orange token",
                    HostUpgrade
                )
            );

            action.DoAction(this);
        }

        private void EachShipAction(GenericShip ship, Action callback)
        {
            Selection.ChangeActiveShip(ship);
            AskToSelectToken(callback);
        }

        private void AskToSelectToken(Action callback)
        {
            BoKatanKryzeDecisionSubphase subPhase = Phases.StartTemporarySubPhaseNew<BoKatanKryzeDecisionSubphase>("Bo-Katan Kryze Decision Subphase", callback);

            subPhase.DescriptionShort = HostUpgrade.UpgradeInfo.Name;
            subPhase.DescriptionLong = "You may remove 1 red or or token from yourself";
            subPhase.ImageSource = HostUpgrade;

            foreach (GenericToken token in Selection.ThisShip.Tokens.GetTokensByColor(TokenColors.Red, TokenColors.Orange))
            {
                if (!subPhase.GetDecisions().Any(n => n.Name == GetRemoveTokenDescription(token)))
                {
                    subPhase.AddDecision(
                        GetRemoveTokenDescription(token),
                        delegate { RemoveToken(token); }
                    );
                }
            }

            subPhase.DecisionOwner = HostShip.Owner;
            subPhase.DefaultDecisionName = subPhase.GetDecisions().Last().Name;
            subPhase.ShowSkipButton = true;

            subPhase.Start();
        }

        private void RemoveToken(GenericToken token)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            Selection.ThisShip.Tokens.RemoveToken(token, Triggers.FinishTrigger);
        }

        private string GetRemoveTokenDescription(GenericToken token)
        {
            string lockLetter = (token is RedTargetLockToken) ? $" ({(token as RedTargetLockToken).Letter})" : "";
            return $"Remove {token.Name}{lockLetter}";
        }

        private void FinishAbility()
        {
            Selection.ChangeActiveShip(PreviousCurrentShip);
            Triggers.FinishTrigger();
        }

        private class BoKatanKryzeDecisionSubphase : DecisionSubPhase { }

    }
}