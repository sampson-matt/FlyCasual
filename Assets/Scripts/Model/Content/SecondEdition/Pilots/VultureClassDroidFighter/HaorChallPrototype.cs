using ActionsList;
using Arcs;
using Ship;
using System;
using System.Collections.Generic;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class HaorChallPrototype : VultureClassDroidFighter
    {
        public HaorChallPrototype()
        {
            PilotInfo = new PilotCardInfo(
                "Haor Chall Prototype",
                1,
                22,
                limited: 2,
                abilityType: typeof(Abilities.SecondEdition.HaorChallPrototypeAbility),
                pilotTitle: "Xi Char Offering"
            );

            ModelInfo.SkinName = "Gray";
        }
    }
}

namespace Abilities.SecondEdition
{
    //After an Enemy ship in your bullseye arc at range 0-2 declares another friendly ship as the defender, 
    //you may perform a calculate or lock action.
    public class HaorChallPrototypeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackStartAsAttackerGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackStartAsAttackerGlobal -= CheckAbility;
        }

        private void CheckAbility()
        {
            if (Tools.IsFriendly(Combat.Defender, HostShip)
                && Combat.Defender != HostShip
                && HostShip.SectorsInfo.IsShipInSector(Combat.Attacker, ArcType.Bullseye)
                && new BoardTools.DistanceInfo(HostShip, Combat.Attacker).Range <= 2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackStart, AskPerformAction);
            }
        }

        private void AskPerformAction(object sender, EventArgs e)
        {
            GenericShip previousActiveShip = Selection.ThisShip;
            Selection.ChangeActiveShip(HostShip);

            List<GenericAction> actions = new List<GenericAction>() { new CalculateAction(), new TargetLockAction() };
            HostShip.AskPerformFreeAction(
                actions,
                delegate {
                    Selection.ChangeActiveShip(previousActiveShip);
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "After an Enemy ship in your bullseye arc at range 0-2 declares another friendly ship as the defender, you may perform a Calculate or Lock action",
                HostShip
            );
        }
    }
}
