using Upgrade;
using System;
using System.Collections.Generic;
using Ship;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class ObiWanKenobi : GenericUpgrade
    {
        public ObiWanKenobi() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Obi-Wan Kenobi",
                UpgradeType.Crew,
                cost: 9,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Republic),
                abilityType: typeof(Abilities.SecondEdition.ObiWanKenobiCrewAbility),
                addForce: 1
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ObiWanKenobiCrewAbility : GenericAbility
    {
        //After a friendly ship at range 0-2 spends a focus or evade token, you may spend 1 Force.
        //If you do, that ship gains 1 focus token. 
        public override void ActivateAbility()
        {
            GenericShip.OnTokenIsSpentGlobal += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnTokenIsSpentGlobal -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship, GenericToken token)
        {
            if (HostShip.State.Force > 0
                && Tools.IsFriendly(ship, HostShip)
                && (token is FocusToken || token is EvadeToken)
                && new BoardTools.DistanceInfo(ship, HostShip).Range < 3)
            {
                TargetShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsSpent, AskToUseObiWanAbility);
            }
        }

        private void AskToUseObiWanAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                AlwaysUseByDefault,
                UseAbility,
                descriptionLong: "Do you want to spend 1 Force? (If you do, that ship gains 1 Focus Token)",
                imageHolder: HostShip
            );
        }

        private void UseAbility(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": " + TargetShip.PilotInfo.PilotName + " gains Focus token");
            HostShip.State.SpendForce(
                1,
                delegate {
                    TargetShip.Tokens.AssignToken(
                        new Tokens.FocusToken(TargetShip),
                        SubPhases.DecisionSubPhase.ConfirmDecision
                    );
                }
            );
        }
    }
}