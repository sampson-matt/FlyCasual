using Upgrade;
using Ship;
using System;
using System.Collections.Generic;
using UnityEngine;
using SubPhases;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class KorkieKryze : GenericUpgrade
    {
        public KorkieKryze() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Korkie Kryze",
                UpgradeType.Crew,
                cost: 7,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Republic),
                abilityType: typeof(Abilities.SecondEdition.KorkieKryzeAbility)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class KorkieKryzeAbility : GenericAbility
    {
        //After a friendly ship in your Full Front Arc at range 1-2 becomes the defender,
        //you may transfer 1 green token to it.

        //While a friendly ship in your Full Front Arc at range 1-2 defends,
        //if you obstruct the attack, the defender rolls 1 additional defense die.
        public override void ActivateAbility()
        {
            GenericShip.OnAttackStartAsDefenderGlobal += CheckTransferAbility;
            HostShip.OnShotObstructedByMe += CheckObstructedAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackStartAsDefenderGlobal += CheckTransferAbility;
            HostShip.OnShotObstructedByMe -= CheckObstructedAbility;
        }

        private void CheckTransferAbility()
        {
            if(HostShip.Tokens.GetTokensByColor(Tokens.TokenColors.Green).Count > 0 && BoardTools.Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.FullFront, new Vector2(1, 2), Team.Type.Friendly).Contains(Combat.Defender))
            {
                RegisterAbilityTrigger(TriggerTypes.OnDefenseStart, AskToTransferToken);
            }

        }

        private void AskToTransferToken(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                NeverUseByDefault,
                AgreeToTransferToken,
                descriptionLong: "Do you want to transfer 1 green token to " +Combat.Defender.PilotInfo.PilotName+ "?",
                imageHolder: HostUpgrade
            );
        }

        private void AgreeToTransferToken(object sender, EventArgs e)
        {
            SelectTokenToReassignSubphase subphase = Phases.StartTemporarySubPhaseNew<SelectTokenToReassignSubphase>(
                "Reassign Token",
                DecisionSubPhase.ConfirmDecision
            );

            subphase.Name = HostUpgrade.UpgradeInfo.Name;
            subphase.DescriptionShort = "Select a token to transfer to the target";
            subphase.ImageSource = HostUpgrade;

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            foreach (GenericToken token in HostShip.Tokens.GetTokensByColor(Tokens.TokenColors.Green))
            {
                subphase.AddDecision(
                    token.Name,
                    delegate { ActionsHolder.ReassignToken(token, HostShip, Combat.Defender, DecisionSubPhase.ConfirmDecision); }
                );
            }

            if (subphase.GetDecisions().Count > 0)
            {
                subphase.Start();
            }
            else
            {
                Phases.GoBack();
                Messages.ShowInfoToHuman("Korkie Kryze No tokens to transfer to the target");
                Triggers.FinishTrigger();
            }
        }

        

        private void CheckObstructedAbility(GenericShip attacker, ref int count)
        {
            if(Combat.Defender != null && BoardTools.Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.FullFront, new Vector2(1,2), Team.Type.Friendly).Contains(Combat.Defender))
            {
                Messages.ShowInfo(string.Format("{0}: The attack is obstructed by {1}, giving the defender +1 defense die", HostUpgrade.UpgradeInfo.Name, HostShip.PilotInfo.PilotName));
                count++;
            }
        }

        private class SelectTokenToReassignSubphase : DecisionSubPhase { }

    }
}