using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;
using Content;

namespace Ship.SecondEdition.Delta7Aethersprite
{
    public class ObiWanKenobi : Delta7Aethersprite
    {
        public ObiWanKenobi()
        {
            PilotInfo = new PilotCardInfo(
                "Obi-Wan Kenobi",
                5,
                47,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.ObiWanKenobiAbility),
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.ForcePower
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    //After a friendly ship at range 0-2 spends a focus token, you may spend force. If you do, that ship gains 1 focus token.
    public class ObiWanKenobiAbility : GenericAbility
    {
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
                && token is FocusToken
                && new BoardTools.DistanceInfo(ship, HostShip).Range < 3)
            {
                TargetShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsSpent, AskToUseObiWanAbility);
            }
        }

        private void AskToUseObiWanAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
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
