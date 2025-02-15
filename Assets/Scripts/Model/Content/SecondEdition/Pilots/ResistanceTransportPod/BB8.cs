﻿using Abilities.SecondEdition;
using ActionsList;
using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.ResistanceTransportPod
{
    public class BB8 : ResistanceTransportPod
    {
        public BB8()
        {
            PilotInfo = new PilotCardInfo(
                "BB-8",
                3,
                23,
                isLimited: true,
                abilityType: typeof(BB8TransportPodAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );

            ShipInfo.ActionIcons.SwitchToDroidActions();
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BB8TransportPodAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnSystemsAbilityActivation += RegisterOwnTrigger;
            HostShip.OnCheckSystemsAbilityActivation += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnSystemsAbilityActivation -= RegisterOwnTrigger;
            HostShip.OnCheckSystemsAbilityActivation -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, ref bool flag)
        {
            flag = true;
        }

        private void RegisterOwnTrigger(GenericShip ship)
        {
            // Always register
            RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToPerformReposition);
        }

        private void AskToPerformReposition(object sender, EventArgs e)
        {
            Sounds.PlayShipSound("BB-8-Sound");

            HostShip.AskPerformFreeAction(
                new List<GenericAction>()
                {
                    new BarrelRollAction(){Color = Actions.ActionColor.Red},
                    new BoostAction(){Color = Actions.ActionColor.Red}
                },
                Triggers.FinishTrigger,
                HostShip.PilotInfo.PilotName,
                "During the System Phase, you may perform a red Barrel Roll or Boost action",
                HostShip
            );
        }
    }
}