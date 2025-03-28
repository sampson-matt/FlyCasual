using Players;
using RulesList;
using Ship;
using System;
using System.Collections.Generic;
using Tokens;

namespace Obstacles
{
    public class ElectroChaffCloud : GenericObstacle
    {
        protected int fuses = 0;

        public GenericPlayer Assigner;

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
        public ElectroChaffCloud(string name, string shortName, GenericPlayer owner) : base(name, shortName)
        {
            Assigner = owner;
            RulesList.TargetLocksRule.OnCheckTargetLockIsDisallowed += DisallowTargetLocks;
            RulesList.JamRule.OnCheckJamIsDisallowed += DisallowJams;
        }

        public override string GetTypeName => "Electro-Chaff Cloud";

        public override void OnLanded(GenericShip ship)
        {
            ship.OnCanBeCoordinated += denyCoordinate;
        }

        private void DisallowJams(ref List<JamIsNotAllowedReasons> blockReasons, GenericShip jamSource, GenericShip jamTarget)
        {
            if(jamTarget.IsLandedOnObstacle && jamTarget.ObstaclesLanded.Contains(this))
            {
                blockReasons.Add(JamIsNotAllowedReasons.Obstacle);
            }
        }

        private void denyCoordinate(GenericShip ship, ref bool canBeCoordinated)
        {
            if (ship.IsLandedOnObstacle && ship.ObstaclesLanded.Contains(this))
            {
                canBeCoordinated = false;
            }            
        }

        private void DisallowTargetLocks(ref bool result, GenericShip attacker, ITargetLockable defender)
        {
            if (result)
            {
                if (defender != null && defender is GenericShip ship && ship.IsLandedOnObstacle && ship.ObstaclesLanded.Contains(this))
                {
                    result = false;
                }
            }
        }

        public override void OnHit(GenericShip ship)
        {
            Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " hit Electro-Chaff Cloud during movement, All lock are broken, Jam token is assigned");
            if (!Selection.ThisShip.CanPerformActionsWhenOverlapping
                && Editions.Edition.Current.RuleSet.GetType() == typeof(Editions.RuleSets.RuleSet20))
            {
                Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " hit Electro-Chaff Cloud during movement, their action subphase is skipped");
                Selection.ThisShip.IsSkipsActionSubPhase = true;
            }
            BreakAllLocks(ship, delegate {
                ship.Tokens.AssignToken(new JamToken(ship, Assigner), Triggers.FinishTrigger);
            });
        }

        private void BreakAllLocks(GenericShip ship, Action callback)
        {
            ship.Tokens.RemoveAllTokensByType(typeof(GenericTargetLockToken), callback, '*');
        }

        public override void OnShotObstructedExtra(GenericShip attacker, GenericShip defender, ref int result)
        {
            // Only default effect
        }

        public override void AfterObstacleRoll(GenericShip ship, DieSide side, Action callback)
        {
        }
    }
}


