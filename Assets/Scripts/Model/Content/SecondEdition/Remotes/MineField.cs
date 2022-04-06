using BoardTools;
using Players;
using Remote;
using Ship;
using SubPhases;
using SubPhases.SecondEdition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upgrade;

namespace Remote
{
    public class MineField : GenericRemote
    {
        public MineField(GenericPlayer owner) : base(owner)
        {
            RemoteInfo = new RemoteInfo(
                "Mine Field",
                0, 1, 3,
                "https://vignette.wikia.nocookie.net/xwing-miniatures-second-edition/images/3/38/Remote_BuzzDroidSwarm.png",
                typeof(Abilities.SecondEdition.MineFieldAbiliy)
            );
        }

        public override Dictionary<string, Vector3> BaseEdges
        {
            get
            {
                return new Dictionary<string, Vector3>()
                {
                    { "R0", new Vector3(0f, 0f, -2.5f) },
                    { "R2", new Vector3(0.487725f, 0f, -2.4519625f) },
                    { "R4", new Vector3(0.9567075f, 0f, -2.3097f) },
                    { "R6", new Vector3(1.388925f, 0f, -2.078675f) },
                    { "R8", new Vector3(1.7677675f, 0f, -1.7677675f) },
                    { "R10", new Vector3(2.078675f, 0f, -1.388925f) },
                    { "R12", new Vector3(2.3097f, 0f, -0.9567075f) },
                    { "R14", new Vector3(2.4519625f, 0f, -0.487725f) },
                    { "R16", new Vector3(2.5f, 0f, -0f) },
                    { "R18", new Vector3(2.4519625f, 0f, 0.487725f) },
                    { "R20", new Vector3(2.3097f, 0f, 0.9567075f) },
                    { "R22", new Vector3(2.078675f, 0f, 1.388925f) },
                    { "R24", new Vector3(1.7677675f, 0f, 1.7677675f) },
                    { "R26", new Vector3(1.388925f, 0f, 2.078675f) },
                    { "R28", new Vector3(0.9567075f, 0f, 2.3097f) },
                    { "R30", new Vector3(0.487725f, 0f, 2.4519625f) },
                    { "R32", new Vector3(-0f, 0f, 2.5f) },
                    { "R34", new Vector3(-0.487725f, 0f, 2.4519625f) },
                    { "R36", new Vector3(-0.9567075f, 0f, 2.3097f) },
                    { "R38", new Vector3(-1.388925f, 0f, 2.078675f) },
                    { "R40", new Vector3(-1.7677675f, 0f, 1.7677675f) },
                    { "R42", new Vector3(-2.078675f, 0f, 1.388925f) },
                    { "R44", new Vector3(-2.3097f, 0f, 0.9567075f) },
                    { "R46", new Vector3(-2.4519625f, 0f, 0.487725f) },
                    { "R48", new Vector3(-2.5f, 0f, 0f) },
                    { "R50", new Vector3(-2.4519625f, 0f, -0.487725f) },
                    { "R52", new Vector3(-2.3097f, 0f, -0.9567075f) },
                    { "R54", new Vector3(-2.078675f, 0f, -1.388925f) },
                    { "R56", new Vector3(-1.7677675f, 0f, -1.7677675f) },
                    { "R58", new Vector3(-1.388925f, 0f, -2.078675f) },
                    { "R60", new Vector3(-0.9567075f, 0f, -2.3097f) },
                    { "R62", new Vector3(-0.487725f, 0f, -2.4519625f) },

                };
            }
        }

        public override Transform GetModelTransform()
        {
            return ShipAllParts.Find("ShipBase/model");
        }
    }
}

namespace SubPhases.SecondEdition
{
    public class MineFieldCheckSubPhase : DiceRollCheckSubPhase
    {
        public GenericShip HostShip;

        public override void Prepare()
        {
            DiceKind = DiceKind.Attack;
            DiceCount = 2;

            AfterRoll = FinishAction;
        }

        protected override void FinishAction()
        {
            HideDiceResultMenu();

            CurrentDiceRoll.RemoveAllFailures();
            CurrentDiceRoll.AddDice(DieSide.Success);
            if (!CurrentDiceRoll.IsEmpty)
            {
                SufferDamage();
            }
            else
            {
                NoDamage();
            }

        }

        private void SufferDamage()
        {
            Messages.ShowInfo("Mine Field: " + Selection.ActiveShip.PilotInfo.PilotName + " suffers damage.");

            DamageSourceEventArgs proximityDamage = new DamageSourceEventArgs()
            {
                Source = HostShip,
                DamageType = DamageTypes.BombDetonation
            };

            Selection.ActiveShip.Damage.TryResolveDamage(CurrentDiceRoll.DiceList, proximityDamage, CallBack);
        }

        private void NoDamage()
        {
            Messages.ShowInfo("Mine Field: " + Selection.ActiveShip.PilotInfo.PilotName + " suffers no damage.");
            CallBack();
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MineFieldAbiliy : GenericAbility
    {


        public override void ActivateAbility()
        {
            GenericShip.OnPositionFinishGlobal += CheckRemoteOverlapping;
            HostShip.OnTryDamagePrevention += RegisterConvertCritsToDoubleDamage;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnPositionFinishGlobal -= CheckRemoteOverlapping;
            HostShip.OnTryDamagePrevention -= RegisterConvertCritsToDoubleDamage;
        }

        private void RegisterConvertCritsToDoubleDamage(GenericShip ship, DamageSourceEventArgs e)
        {
            RegisterAbilityTrigger(TriggerTypes.OnTryDamagePrevention, ConvertCritsToDoubleDamage);

        }

        private void ConvertCritsToDoubleDamage(object sender, EventArgs e)
        {
            int critsCount = HostShip.AssignedDamageDiceroll.CriticalSuccesses;

            if (critsCount > 0)
            {
                Messages.ShowInfo("A Critical Hit causes " + HostShip.PilotInfo.PilotName + " to suffer 1 additional damage");
                for (int i = 0; i < critsCount; i++)
                {
                    HostShip.AssignedDamageDiceroll.RemoveType(DieSide.Crit);
                    HostShip.AssignedDamageDiceroll.AddDice(DieSide.Success);
                    HostShip.AssignedDamageDiceroll.AddDice(DieSide.Success);
                }
            }
            Triggers.FinishTrigger();
        }

        private void CheckRemoteOverlapping(GenericShip ship)
        {
            // Only for real ships
            if (ship is GenericRemote) return;

            if (ship.Owner.PlayerNo == HostShip.Owner.PlayerNo) return;

            if (ship.RemotesOverlapped.Contains(HostShip) || ship.RemotesMovedThrough.Contains(HostShip))
            {
                RegisterAbilityTrigger(TriggerTypes.OnPositionFinish, delegate { RollAttack(ship, HostShip); });
                HostShip.DestroyShipForced(delegate { });
            }
        }

        private void RollAttack(GenericShip sufferedShip, GenericShip HostShip)
        {
            MineFieldCheckSubPhase subphase = Phases.StartTemporarySubPhaseNew<MineFieldCheckSubPhase>(
               "Damage from Mine Field",
               delegate {
                   Phases.FinishSubPhase(typeof(MineFieldCheckSubPhase));
                   Triggers.FinishTrigger();
               }
           );
            subphase.HostShip = HostShip;
            subphase.Start();
        }
    }

}