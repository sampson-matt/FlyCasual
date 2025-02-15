﻿using Movement;
using Ship;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Aggressor
{
    public class NavigationResult
    {
        public bool isOffTheBoard;
        public bool isLandedOnObstacle;

        public int enemiesInShotRange;
        public int enemiesTargetingThisShip;

        public int obstaclesHit;
        public int minesHit;

        public float distanceToNearestEnemy;
        public float distanceToNearestEnemyInShotRange;
        public float angleToNearestEnemy;

        public bool isOffTheBoardNextTurn;
        public bool isHitAsteroidNextTurn;

        public bool isBumped;

        public bool isFleeing;
        public bool isEscaped;

        public GenericMovement movement;

        public int Priority { get; private set; }

        public ShipPositionInfo FinalPositionInfo { get; set; }

        public void CalculatePriority()
        {
            if (isFleeing && isEscaped)
            {
                Priority = int.MaxValue;
                return;
            }
            if (isOffTheBoard)
            {
                Priority = int.MinValue;
                return;
            }

            if (isLandedOnObstacle) Priority -= 20000;

            if (isOffTheBoardNextTurn) Priority -= 40000;

            Priority += enemiesInShotRange * 1000;
            // Allow up to two enemies targeting us to equal one enemy in our attack range.
            // Priority -= enemiesTargetingThisShip * 500;

            Priority -= minesHit * 2000;

            int asteroidAvoidPriority = (BoardTools.Board.DistanceToRange(distanceToNearestEnemy) < 4) ? 1 : 10;

            Priority -= obstaclesHit * 2000 * asteroidAvoidPriority;
            if (isHitAsteroidNextTurn) Priority -= 1000 * asteroidAvoidPriority;

            if (isBumped) Priority -= 500;

            if (Selection.ThisShip.Damage.HasCrit(typeof(DamageDeckCardSE.LooseStabilizer)) && movement.Bearing != ManeuverBearing.Straight)
            {
                if (Selection.ThisShip.State.HullCurrent + Selection.ThisShip.State.ShieldsCurrent == 1)
                {
                    Priority -= 20000;
                }
                else
                {
                    Priority -= 1000;
                }
            }

            switch (movement.ColorComplexity)
            {
                case MovementComplexity.Easy:
                    if (Selection.ThisShip.IsStressed) Priority += 500;
                    break;
                case MovementComplexity.Complex:
                    if (Selection.ThisShip.IsStressed)
                    {
                        Priority = int.MinValue;
                    }
                    else
                    {
                        Priority -= 500;
                    }
                    break;
                case MovementComplexity.None: // Impossible maneuvers
                    Priority = int.MinValue;
                    break;
                default:
                    break;
            }

            //distance is 0..10, result 0..100
            Priority += (10 - (int)distanceToNearestEnemy) * 10;

            //angle is 0..180, result 0..180
            Priority += (180 - Mathf.RoundToInt(angleToNearestEnemy));
        }

        public override string ToString()
        {
            string result = "";

            result += Priority + " = ";

            result += "distance:" + distanceToNearestEnemy + " ";
            result += "distanceShot:" + distanceToNearestEnemyInShotRange + " ";
            result += "angle:" + angleToNearestEnemy + " ";

            if (isOffTheBoard) result += "OffBoard ";
            if (isLandedOnObstacle) result += "LandedOnObstacle ";
            if (isBumped) result += "Bumped ";
            

            if (enemiesInShotRange > 0) result += "enemiesToShoot:" + enemiesInShotRange + " ";

            if (obstaclesHit > 0) result += "obstaclesHit:" + obstaclesHit + " ";
            if (minesHit > 0) result += "minesHit:" + obstaclesHit + " ";

            return result;
        }
    }
}
