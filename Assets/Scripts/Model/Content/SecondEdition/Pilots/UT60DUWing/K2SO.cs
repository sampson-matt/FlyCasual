using Abilities.SecondEdition;
using ActionsList;
using Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.UT60DUWing
    {
        public class K2SO : UT60DUWing
        {
            public K2SO() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "K-2SO",
                    3,
                    46,
                    isLimited: true,
                    abilityType: typeof(K2SOPilotAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class K2SOPilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnTokenIsAssigned += RegisterK2SOAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsAssigned -= RegisterK2SOAbility;
        }

        private void RegisterK2SOAbility(GenericShip ship, GenericToken token)
        {
            if (token is StressToken)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, AssignToken);
            }
        }

        private void AssignToken(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": Calculate Token is assigned");
            HostShip.Tokens.AssignToken(typeof(CalculateToken), Triggers.FinishTrigger);
        }
    }
}
