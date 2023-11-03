using Obstacles;
using Ship;
using SubPhases;
using System;
using Tokens;
using Bombs;
using UnityEngine;

namespace Obstacles
{
    public class ElectroChaffCloud : GenericObstacle
    {
        protected int fuses = 0;

        public int Fuses
        {
            get => fuses;
            set
            {
                var oldValue = fuses;
                var newValue = value;
                FusesChanging?.Invoke(this, oldValue, ref newValue);
                fuses = newValue;
            }
        }
        public delegate void DeviceValueChanging(GenericObstacle deviceGameObject, int oldValue, ref int newValue);
        public event DeviceValueChanging FusesChanging;
        public bool IsFused => Fuses > 0;
        public ElectroChaffCloud(string name, string shortName) : base(name, shortName)
        {

        }

        public override string GetTypeName => "Electro-Chaff Cloud";

        public override void OnHit(GenericShip ship)
        {
            Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " hit Electro-Chaff Cloud during movement, All lock are broken, Jam token is assigned");
            BreakAllLocks(ship, delegate {
                ship.Tokens.AssignToken(
                typeof(Tokens.JamToken), () => StartToRoll(ship), ship.Owner); });
            //ship.Tokens.AssignToken(
             //   typeof(Tokens.JamToken),
            //    delegate { BreakAllLocks(ship, () => StartToRoll(ship)); },
             //   ship.Owner
            //);
        }

        private void BreakAllLocks(GenericShip ship, Action callback)
        {
            ship.Tokens.RemoveAllTokensByType(typeof(Tokens.BlueTargetLockToken), delegate { });
            ship.Tokens.RemoveAllTokensByType(typeof(Tokens.RedTargetLockToken), callback);
        }

        private void StartToRoll(GenericShip ship)
        {
            Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " hit Electro-Chaff Cloud during movement, rolling for effect");

            ElectroChaffCloudHitCheckSubPhase newPhase = (ElectroChaffCloudHitCheckSubPhase)Phases.StartTemporarySubPhaseNew(
                "Stress from Electro-Chaff Cloud collision",
                typeof(ElectroChaffCloudHitCheckSubPhase),
                delegate
                {
                    Phases.FinishSubPhase(typeof(ElectroChaffCloudHitCheckSubPhase));
                    Triggers.FinishTrigger();
                });
            newPhase.TheShip = ship;
            newPhase.TheObstacle = this;
            newPhase.Start();
        }

        public override void OnShotObstructedExtra(GenericShip attacker, GenericShip defender, ref int result)
        {
            Messages.ShowInfo("Attack is obstructed by Electro-Chaff Cloud the defender rolls 1 extra defense dice");
            result += 1;
        }

       

        public override void AfterObstacleRoll(GenericShip ship, DieSide side, Action callback)
        {
            if (side == DieSide.Crit || (side == DieSide.Success)
            )
            {
                Messages.ShowErrorToHuman($"{ship.PilotInfo.PilotName} gains a Stress token");
                ship.Tokens.AssignToken(typeof(StressToken), delegate { ship.CallOnRedTokenGainedFromOverlappingObstacle(ship.Tokens.GetToken(typeof(Tokens.StressToken)), callback); });
            }
            else
            {
                NoEffect(callback);
            }
        }

        private void NoEffect(Action callback)
        {
            Messages.ShowInfoToHuman("No damage");
            callback();
        }
    }
}

namespace SubPhases
{

    public class ElectroChaffCloudHitCheckSubPhase : DiceRollCheckSubPhase
    {
        private GenericShip prevActiveShip = Selection.ActiveShip;
        public GenericObstacle TheObstacle { get; set; }

        public override void Prepare()
        {
            DiceKind = DiceKind.Attack;
            DiceCount = 1;

            AfterRoll = FinishAction;
            Selection.ActiveShip = TheShip;
        }

        protected override void FinishAction()
        {
            HideDiceResultMenu();
            Selection.ActiveShip = prevActiveShip;

            TheObstacle.AfterObstacleRoll(TheShip, CurrentDiceRoll.DiceList[0].Side, CallBack);
        }
    }
}


