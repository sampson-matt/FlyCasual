using Ship;
using System;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class FalseTransponderCodes : GenericUpgrade
    {
        public FalseTransponderCodes() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "False Transponder Codes",
                UpgradeType.Illicit,
                cost: 3,
                abilityType: typeof(Abilities.SecondEdition.FalseTransponderCodesAbility),
                charges: 1
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class FalseTransponderCodesAbility : GenericAbility
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
                && (((token as BlueTargetLockToken).Host.ShipId == HostShip.ShipId)
                    || ((token as BlueTargetLockToken).OtherTargetLockTokenOwner as GenericShip)?.ShipId == HostShip.ShipId)
            )
            {
                ObjectForAbility = (token.Host == HostShip) ? (token as BlueTargetLockToken).OtherTargetLockTokenOwner as GenericShip : ship;
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, JamIt);
            }
        }

        private void JamIt(object sender, EventArgs e)
        {
            if (ObjectForAbility is GenericShip)
            {
                Messages.ShowInfo($"False Transponder Codes: {ObjectForAbility.PilotInfo.PilotName} is Jammed");

                HostUpgrade.State.LoseCharge();

                ObjectForAbility.Tokens.AssignToken(
                    new JamToken(ObjectForAbility, HostShip.Owner),
                    Triggers.FinishTrigger
                );
            }
            else
            {
                Messages.ShowInfo($"False Transponder Codes: non-ship object is not Jammed");
                Triggers.FinishTrigger();
            }
            
        }
    }
}