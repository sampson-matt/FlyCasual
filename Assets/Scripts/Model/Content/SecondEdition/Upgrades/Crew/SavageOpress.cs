using Ship;
using Upgrade;
using Tokens;
using System;
using SubPhases;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class SavageOpress : GenericUpgrade
    {
        public SavageOpress() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Savage Opress",
                UpgradeType.Crew,
                cost: 12,
                isLimited: true,
                addForce: 1,
                restriction: new FactionRestriction(Faction.Scum, Faction.Separatists),
                abilityType: typeof(Abilities.SecondEdition.SavageOpressAbility)
            );
        }
    }
}


namespace Abilities.SecondEdition
{
    //After a friendly ship in your Front Arc at range 1-2 gains a stress or strain token,
    //you may spend 1 Force. If you do, that ship gains 1 focus token. 
    public class SavageOpressAbility : GenericAbility
    {
        GenericShip friendlyShip = null;
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
            if (HostShip.State.Force < 1) return;

            if (!BoardTools.Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.Front, new Vector2(1, 2), Team.Type.Friendly).Contains(ship)) return;

            if (token.GetType() != typeof(StressToken) && token.GetType() != typeof(StrainToken)) return;

            friendlyShip = ship;

            RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, AskAssignToken);
        }

        private void AskAssignToken(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                NeverUseByDefault,
                AssignToken,
                descriptionLong: "Do you want to spend 1 Force in order for " + friendlyShip.PilotInfo.PilotName + " to gain 1 focus token?",
                imageHolder: HostUpgrade
            );
        }

        private void AssignToken(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            Messages.ShowInfo(friendlyShip.PilotInfo.PilotName + ": Focus Token is assigned");
            HostShip.State.SpendForce(1, delegate { });
            friendlyShip.Tokens.AssignToken(typeof(FocusToken), Triggers.FinishTrigger);
        }


    }
}