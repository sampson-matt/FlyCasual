﻿using BoardTools;
using Movement;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;
using Content;

namespace Ship.SecondEdition.Delta7Aethersprite
{
    public class MaceWindu : Delta7Aethersprite
    {
        public MaceWindu()
        {
            PilotInfo = new PilotCardInfo(
                "Mace Windu",
                4,
                42,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.MaceWinduAbility),
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.ForcePower
            );

            ModelInfo.SkinName = "Mace Windu";
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you fully execute a red maneuver, recover 1 force.
    public class MaceWinduAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinish += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinish -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            if (HostShip.GetLastManeuverColor() == MovementComplexity.Complex && !(Board.IsOffTheBoard(HostShip) || HostShip.IsBumped))
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AssignTokens);
            }
        }
        
        private void AssignTokens(object sender, EventArgs e)
        {
            HostShip.State.RestoreForce();
            Triggers.FinishTrigger();
        }
    }
}
