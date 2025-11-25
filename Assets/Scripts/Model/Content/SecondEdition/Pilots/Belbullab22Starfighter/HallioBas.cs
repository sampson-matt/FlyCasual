using Ship;
using SubPhases;
using System;
using System.Linq;
using Tokens;

namespace Ship.SecondEdition.Belbullab22Starfighter
{
    public class HallioBas : Belbullab22Starfighter
    {
        public HallioBas()
        {
            PilotInfo = new PilotCardInfo(
                "Hallio Bas",
                5,
                43,
                true,
                charges: 1,
                regensCharges: 1,
                abilityType: typeof(Abilities.SecondEdition.HallioBasAbility)
            );
            PilotNameCanonical = "halliobas-wat1";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class HallioBasAbility : GenericAbility
    {
        Boolean abilitySkipped = false;
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += RegisterHallioBasAbility;
            HostShip.OnAttackStartAsDefender += RegisterHallioBasAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsAttacker -= RegisterHallioBasAbility;
            HostShip.OnAttackStartAsDefender += RegisterHallioBasAbility;
        }

        private void RegisterHallioBasAbility()
        {
            abilitySkipped = false;
            var noFriendlyShipsInRange0to1 = true;

            foreach (var friendlyShip in HostShip.Owner.Ships)
            {
                if (friendlyShip.Value != HostShip && Tools.IsFriendly(friendlyShip.Value, HostShip))
                {
                    BoardTools.DistanceInfo distanceInfo = new BoardTools.DistanceInfo(HostShip, friendlyShip.Value);
                    if (distanceInfo.Range < 2)
                    {
                        noFriendlyShipsInRange0to1 = false;
                        break;
                    }
                }
            }

            if (noFriendlyShipsInRange0to1 && HostShip.State.Charges > 0 && HostShip.Tokens.GetNonLockRedOrangeTokens().Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackStart, ChooseRemoveTokens);
            }
        }

        private void ChooseRemoveTokens(object sender, EventArgs e)
        {
            if (HostShip.State.Charges > 0 && HostShip.Tokens.GetNonLockRedOrangeTokens().Count > 0 && !abilitySkipped)
            {
                HallioBasDecisonSubphase subphase = Phases.StartTemporarySubPhaseNew<HallioBasDecisonSubphase>(
                "Hallio Bas remove non-lock red or orange token decision",
                () => ChooseRemoveTokens(sender, e)
                );

                subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
                subphase.DescriptionLong = "You may spend one charge to remove one non-lock red or orange token.";
                subphase.ImageSource = HostShip;
                subphase.HostShip = HostShip;
                subphase.DecisionOwner = HostShip.Owner;
                subphase.ShowSkipButton = true;
                subphase.OnSkipButtonIsPressed = () => abilitySkipped = true;
                subphase.Start();
            }
            else
            {
                Triggers.FinishTrigger();
            }

            
        }

        private class HallioBasDecisonSubphase : DecisionSubPhase
        {
            public GenericShip HostShip { get; set; }
            public override void PrepareDecision(System.Action callBack)
            {
                DecisionViewType = DecisionViewTypes.TextButtons;

                foreach (GenericToken token in HostShip.Tokens.GetNonLockRedOrangeTokens())
                {
                    if (!GetDecisions().Any(n => n.Name == GetRemoveTokenDescription(token)))
                    {
                        AddDecision(
                            GetRemoveTokenDescription(token),
                            delegate { RemoveToken(token); }
                        );
                    }
                }
                DefaultDecisionName = GetDecisions().First().Name;
                callBack();
            }
            private string GetRemoveTokenDescription(GenericToken token)
            {
                return $"Remove {token.Name}";
            }
            private void RemoveToken(GenericToken token)
            {
                HostShip.SpendCharge();
                HostShip.Tokens.RemoveToken(token, DecisionSubPhase.ConfirmDecision);
            }
        }

        
    }
}
