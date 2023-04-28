using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.SecondEdition;
using Upgrade;
using ActionsList;
using Tokens;

namespace Ship.SecondEdition.ResistanceTransport
{
    public class NodinChavdri : ResistanceTransport
    {
        public NodinChavdri()
        {
            PilotInfo = new PilotCardInfo(
                "Nodin Chavdri",
                2,
                34,
                isLimited: true,
                abilityType: typeof(NodinChavdriGoodeAbility)
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class NodinChavdriGoodeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckAfterCoordinateAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckAfterCoordinateAbility;
        }

        private void CheckAfterCoordinateAbility(GenericAction action)
        {
            if (HasFewStressTokens())
            {
                if (action is CoordinateAction || action.IsCoordinatedAction)
                {
                    RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskPerformActionAsRed);
                }
            }            
        }

        private void AskPerformActionAsRed(object sender, EventArgs e)
        {
            List<GenericAction> actions = HostShip.ActionBar.AllActions.Select(n => n.AsRedAction).ToList();
            actions.ForEach(n => n.CanBePerformedWhileStressed = true);

            HostShip.AskPerformFreeAction(
                actions,
                Triggers.FinishTrigger,
                descriptionShort: HostShip.PilotInfo.PilotName,
                descriptionLong: "You may perform 1 action on your action bar as red action",
                imageHolder: HostShip
            );
        }

        private bool HasFewStressTokens()
        {
            return HostShip.Tokens.CountTokensByType(typeof(StressToken)) <= 2;
        }
    }
}