using Ship;
using Upgrade;
using ActionsList;
using SubPhases;
using Actions;
using Tokens;
using System;
using System.Linq;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class TheMandalorian : GenericUpgrade
    {
        public TheMandalorian() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "The Mandalorian",
                UpgradeType.Crew,
                cost: 4,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                addAction: new ActionInfo(typeof(ReinforceAction)),
                abilityType: typeof(Abilities.SecondEdition.TheMandalorianCrewAbility)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class TheMandalorianCrewAbility : GenericAbility
    {
        //During the End Phase, if you did not defend this round, recover 1 non-recurring , if able.
        bool didDefend = false;

        public override void ActivateAbility()
        {
            HostShip.OnDefenceStartAsDefender += RegisterDidDefend;
            Phases.Events.OnEndPhaseStart_Triggers += RecoverForce;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDefenceStartAsDefender -= RegisterDidDefend;
            Phases.Events.OnEndPhaseStart_Triggers -= RecoverForce;
        }

        private void RegisterDidDefend()
        {
            didDefend = true;
        }

        private void RecoverForce()
        {
            if(!didDefend && !HostShip.IsForceRecurring && HostShip.State.Force < HostShip.State.MaxForce)
            {
                Messages.ShowInfo($"{HostUpgrade.UpgradeInfo.Name}: {HostShip.PilotInfo.PilotName} recovers 1 Force");
                HostShip.State.RestoreForce();
            }
            didDefend = false;
        }
    }
}