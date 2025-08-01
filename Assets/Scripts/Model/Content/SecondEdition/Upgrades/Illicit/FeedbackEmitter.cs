using Ship;
using System;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class FeedbackEmitter : GenericUpgrade
    {
        public FeedbackEmitter() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Feedback Emitter",
                UpgradeType.Illicit,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.FeedbackEmitterAbility),
                charges: 1
            );

            IsHidden = true;

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/FeedbackEmitter.jpg";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class FeedbackEmitterAbility : GenericAbility
    {
        private GenericShip ObjectForAbility;

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
            if (HostUpgrade.State.Charges > 0
                && token is BlueTargetLockToken
                && ((token as BlueTargetLockToken).OtherTargetLockTokenOwner as GenericShip)?.ShipId == HostShip.ShipId)
            {
                ObjectForAbility = (token.Host == HostShip) ? (token as BlueTargetLockToken).OtherTargetLockTokenOwner as GenericShip : ship;
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, AskToJam);
            }
        }

        private void AskToJam(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                AlwaysUseByDefault,
                JamIt,
                descriptionLong: "Do you want to spend 1 charge to jam " + ObjectForAbility.PilotInfo.PilotName + "?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void JamIt(object sender, EventArgs e)
        {
            
            if (ObjectForAbility is GenericShip)
            {
                Messages.ShowInfo($"Feedback Emitter: {ObjectForAbility.PilotInfo.PilotName} is Jammed");

                HostUpgrade.State.LoseCharge();

                ObjectForAbility.Tokens.AssignToken(
                    new JamToken(ObjectForAbility, HostShip.Owner),
                    SubPhases.DecisionSubPhase.ConfirmDecision
                );
            }
            else
            {
                Messages.ShowInfo($"Feedback Emitter: non-ship object is not Jammed");
                SubPhases.DecisionSubPhase.ConfirmDecision();
            }
            
        }
    }
}