using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.V19TorrentStarfighter
{
    public class Tucker : V19TorrentStarfighter
    {
        public Tucker()
        {
            PilotInfo = new PilotCardInfo(
                "\"Tucker\"",
                2,
                25,
                true,
                abilityType: typeof(Abilities.SecondEdition.TuckerAbility)
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    //After a friendly ship at range 1-2 performs an attack against an enemy ship in your front arc, you may perform a focus action.
    public class TuckerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackFinishGlobal += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackFinishGlobal -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            var range = new BoardTools.DistanceInfo(HostShip, Combat.Attacker).Range;

            if (Tools.IsFriendly(Combat.Attacker, HostShip) 
                && Combat.Defender.Owner != HostShip.Owner
                && HostShip.SectorsInfo.IsShipInSector(Combat.Defender, Arcs.ArcType.Front)
                && range >= 1 && range <= 2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskPerformFocusAction);
            }
        }

        private void AskPerformFocusAction(object sender, EventArgs e)
        {
            GenericShip previousActiveShip = Selection.ThisShip;
            Selection.ChangeActiveShip(HostShip);

            HostShip.AskPerformFreeAction(
                new FocusAction(),
                () => {
                    Selection.ChangeActiveShip(previousActiveShip);
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "After a friendly ship at range 1-2 performs an attack against an enemy ship in your Standard Front Arc, you may perform a Focus action",
                HostShip
            );
        }
    }
}
