using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BoardTools;
using Movement;
using Players;
using Ship;
using UnityEngine;
using ActionsList;

namespace AI.Aggressor
{
    public static class NavigationSubSystem
    {
        private static GenericPlayer CurrentPlayer;

        private static Dictionary<PlayerNo, VirtualBoard> VirtualBoards;
        private static VirtualBoard VirtualBoard
        {
            get { return VirtualBoards[CurrentPlayer.PlayerNo]; }
            set { VirtualBoards[CurrentPlayer.PlayerNo] = value; }
        }

        private static int OrderOfActivation;

        private static NavigationResult CurrentNavigationResult;

        public static void CalculateNavigation(Action callback)
        {
            CurrentPlayer = Roster.GetPlayer(Phases.CurrentSubPhase.RequiredPlayer);

            ConfigureVirtualBoards();

            GameManagerScript.Instance.StartCoroutine
            (
                StartCalculations(callback)
            );
        }

        private static IEnumerator StartCalculations(Action callback)
        {
            ShowCalculationsStart();

            SwitchEnemyShipsToSimpleVirtualPositions();
            yield return PredictAllFinalPositionsOfOwnShips();

            RestoreRealBoard();

            List<GenericShip> orderOfActivation = GenerateOrderOfActivation();

            yield return FindBestManeuversForShips(orderOfActivation);

            RestoreRealBoard();
            ShowCalculationsEnd();

            callback();
        }

        private static void SwitchEnemyShipsToSimpleVirtualPositions()
        {
            foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
            {
                PredictSimpleFinalPositionOfEnemyShip(ship);
            }
        }

        private static void PredictSimpleFinalPositionOfEnemyShip(GenericShip ship)
        {
            Selection.ThisShip = ship;

            GenericMovement savedMovement = ship.AssignedManeuver;

            // Decide what maneuvers to use as temporary
            string temporyManeuver = (ship.State.IsIonized) ? "1.F.S" : "2.F.S";
            bool isTemporaryManeuverAdded = false;
            if (!ship.HasManeuver(temporyManeuver))
            {
                isTemporaryManeuverAdded = true;
                ship.Maneuvers.Add(temporyManeuver, MovementComplexity.Easy);
            }
            GenericMovement movement = ShipMovementScript.MovementFromString(temporyManeuver);

            // Check maneuver
            ship.SetAssignedManeuver(movement, isSilent: true);
            movement.Initialize();
            movement.IsSimple = true;

            MovementPrediction prediction = new MovementPrediction(ship, movement);
            prediction.CalculateOnlyFinalPositionIgnoringCollisions();

            if (isTemporaryManeuverAdded)
            {
                ship.Maneuvers.Remove(temporyManeuver);
            }

            if (savedMovement != null)
            {
                ship.SetAssignedManeuver(savedMovement, isSilent: true);
            }
            else
            {
                ship.ClearAssignedManeuver();
            }

            VirtualBoard.SetVirtualPositionInfo(ship, prediction.FinalPositionInfo, temporyManeuver);
        }

        private static IEnumerator PredictAllFinalPositionsOfOwnShips()
        {
            foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
            {
                VirtualBoard.SwitchToVirtualPosition(ship);
            }

            foreach (GenericShip ship in CurrentPlayer.Ships.Values)
            {
                yield return PredictFinalPosionsOfOwnShip(ship);
            }

            foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
            {
                VirtualBoard.SwitchToRealPosition(ship);
            }
        }

        private static IEnumerator PredictFinalPosionsOfOwnShip(GenericShip ship)
        {
            Selection.ChangeActiveShip(ship);
            VirtualBoard.SwitchToRealPosition(ship);

            Dictionary<string, NavigationResult> navigationResults = new Dictionary<string, NavigationResult>();
            foreach (var maneuver in ship.GetManeuvers())
            {
                GenericMovement movement = ShipMovementScript.MovementFromString(maneuver.Key);
                ship.SetAssignedManeuver(movement, isSilent: true);
                movement.Initialize();
                movement.IsSimple = true;

                MovementPrediction prediction = new MovementPrediction(ship, movement);
                prediction.CalculateOnlyFinalPositionIgnoringCollisions();

                VirtualBoard.SetVirtualPositionInfo(ship, prediction.FinalPositionInfo, prediction.CurrentMovement.ToString());
                VirtualBoard.SwitchToVirtualPosition(ship);

                float minDistanceToEnemyShip, minDistanceToNearestEnemyInShotRange, minAngle;
                int enemiesInShotRange, enemiesTargetingThisShip;
                ProcessHeavyGeometryCalculations(ship, out minDistanceToEnemyShip, out minDistanceToNearestEnemyInShotRange, out minAngle, out enemiesInShotRange, out enemiesTargetingThisShip);


                NavigationResult result = new NavigationResult()
                {
                    movement = prediction.CurrentMovement,
                    distanceToNearestEnemy = minDistanceToEnemyShip,
                    distanceToNearestEnemyInShotRange = minDistanceToNearestEnemyInShotRange,
                    angleToNearestEnemy = minAngle,
                    enemiesInShotRange = enemiesInShotRange,
                    isBumped = prediction.IsBumped,
                    isLandedOnObstacle = prediction.IsLandedOnAsteroid,
                    isOffTheBoard = prediction.IsOffTheBoard,
                    isEscaped = determineEscaped(ship.EscapeEdge, prediction),
                    FinalPositionInfo = prediction.FinalPositionInfo,
                    isFleeing = ship.IsFleeing
                };
                result.CalculatePriority();

                navigationResults.Add(maneuver.Key, result);

                VirtualBoard.SwitchToRealPosition(ship);

                yield return true;
            }

            ship.ClearAssignedManeuver();
            VirtualBoard.UpdateNavigationResults(ship, navigationResults);
            Selection.DeselectThisShip();
        }

        private static List<GenericShip> GenerateOrderOfActivation()
        {
            OrderOfActivation = 0;

            List<GenericShip> orderOfActivation = new List<GenericShip>();

            List<GenericShip> AllShips = new List<GenericShip>(Roster.AllShips.Values.ToList());

            while (AllShips.Count > 0)
            {
                int lowestInitiative = AllShips.Min(n => n.State.Initiative);

                GenericShip shipToActivate = AllShips
                    .Where(n => n.State.Initiative == lowestInitiative)
                    .OrderBy(n => GetMinDistanceToEnemyShip(n))
                    .OrderByDescending(n => n.Owner.PlayerNo == Phases.PlayerWithInitiative)
                    .First();

                orderOfActivation.Add(shipToActivate);
                AllShips.Remove(shipToActivate);
            }

            if (DebugManager.DebugAiNavigation)
            {
                string orderOfActivationText = "";
                foreach (GenericShip ship in orderOfActivation)
                {
                    orderOfActivationText += (ship.ShipId + ", ");
                }
            }

            return orderOfActivation;
        }

        private static IEnumerator FindBestManeuversForShips(List<GenericShip> orderOfActivation)
        {
            while (orderOfActivation.Count > 0)
            {
                SetVirtualPositionsForShipsWithPreviousActivations(orderOfActivation);

                GenericShip ship = orderOfActivation.First();
                orderOfActivation.Remove(ship);

                if (ship.Owner.PlayerNo == CurrentPlayer.PlayerNo)
                {
                    yield return FindBestManeuver(ship);
                }
                else
                {
                    yield return PredictCollisionDetectionOfEnemyShip(ship);
                }
            }
        }

        private static IEnumerator FindBestManeuver(GenericShip ship)
        {
            Selection.ChangeActiveShip(ship);

            int bestPriority = int.MinValue;
            KeyValuePair<string, NavigationResult> maneuverToCheck = new KeyValuePair<string, NavigationResult>();

            do
            {
                VirtualBoard.SwitchToRealPosition(ship);

                bestPriority = VirtualBoard.Ships[ship].NavigationResults.Max(n => n.Value.Priority);
                maneuverToCheck = VirtualBoard.Ships[ship].NavigationResults.Where(n => n.Value.Priority == bestPriority).First();

                GenericMovement movement = ShipMovementScript.MovementFromString(maneuverToCheck.Key);

                ship.SetAssignedManeuver(movement, isSilent: true);
                movement.Initialize();
                movement.IsSimple = true;

                MovementPrediction prediction = new MovementPrediction(ship, movement);
                yield return prediction.CalculateMovementPredicition();

                VirtualBoard.SetVirtualPositionInfo(ship, prediction.FinalPositionInfo, prediction.CurrentMovement.ToString());
                VirtualBoard.SwitchToVirtualPosition(ship);

                CurrentNavigationResult = new NavigationResult()
                {
                    movement = prediction.CurrentMovement,
                    isBumped = prediction.IsBumped,
                    isLandedOnObstacle = prediction.IsLandedOnAsteroid,
                    obstaclesHit = prediction.AsteroidsHit.Count,
                    isOffTheBoard = prediction.IsOffTheBoard,
                    isEscaped = determineEscaped(ship.EscapeEdge, prediction),
                    minesHit = prediction.MinesHit.Count,
                    isOffTheBoardNextTurn = false, //!NextTurnNavigationResults.Any(n => !n.isOffTheBoard),
                    isHitAsteroidNextTurn = false, //!NextTurnNavigationResults.Any(n => n.obstaclesHit == 0),
                    FinalPositionInfo = prediction.FinalPositionInfo,
                    isFleeing = ship.IsFleeing
                };

                foreach (GenericShip enemyShip in CurrentPlayer.EnemyShips.Values)
                {
                    VirtualBoard.SwitchToVirtualPosition(enemyShip);
                }

                if (!prediction.IsOffTheBoard)
                {
                    yield return CheckNextTurnRecursive(ship);

                    float minDistanceToEnemyShip, minDistanceToNearestEnemyInShotRange, minAngle;
                    int enemiesInShotRange, enemiesTargetingThisShip;

                    ProcessHeavyGeometryCalculations(ship, out minDistanceToEnemyShip, out minDistanceToNearestEnemyInShotRange, out minAngle, out enemiesInShotRange, out enemiesTargetingThisShip);

                    CurrentNavigationResult.distanceToNearestEnemy = minDistanceToEnemyShip;
                    CurrentNavigationResult.distanceToNearestEnemyInShotRange = minDistanceToNearestEnemyInShotRange;
                    CurrentNavigationResult.angleToNearestEnemy = minAngle;
                    CurrentNavigationResult.enemiesInShotRange = enemiesInShotRange;
                }

                CurrentNavigationResult.CalculatePriority();

                VirtualBoard.Ships[ship].NavigationResults[maneuverToCheck.Key] = CurrentNavigationResult;

                bestPriority = VirtualBoard.Ships[ship].NavigationResults.Max(n => n.Value.Priority);

                VirtualBoard.SwitchToRealPosition(ship);

                maneuverToCheck = VirtualBoard.Ships[ship].NavigationResults.First(n => n.Key == maneuverToCheck.Key);

                foreach (GenericShip enemyShip in CurrentPlayer.EnemyShips.Values)
                {
                    VirtualBoard.SwitchToRealPosition(enemyShip);
                }

            } while (maneuverToCheck.Value.Priority != bestPriority);

            VirtualBoard.Ships[ship].SetPlannedManeuverCode(maneuverToCheck.Key, ++OrderOfActivation);
            ship.ClearAssignedManeuver();
            Selection.DeselectThisShip();
        }

        private static bool determineEscaped(string escapeEdge, MovementPrediction prediction)
        {
            switch (escapeEdge)
            {
                case null:
                    return false;
                case "north":
                    return prediction.IsOffTheBoardNorth;
                case "south":
                    return prediction.IsOffTheBoardSouth;
                case "east":
                    return prediction.IsOffTheBoardEast;
                case "west":
                    return prediction.IsOffTheBoardWest;
                default:
                    return false;
            }
        }

        private static Vector3 determineEscapeEdge(string escapeEdge)
        {
            switch (escapeEdge)
            {
                case "north":
                    return Board.GetBoard().Find("OffTheBoardHolder").Find("OffTheBoardNorth").transform.position;
                case "south":
                    return Board.GetBoard().Find("OffTheBoardHolder").Find("OffTheBoardSouth").transform.position;
                case "east":
                    return Board.GetBoard().Find("OffTheBoardHolder").Find("OffTheBoardEast").transform.position;
                case "west":
                    return Board.GetBoard().Find("OffTheBoardHolder").Find("OffTheBoardWest").transform.position;
                default:
                    return Board.GetBoard().Find("OffTheBoardHolder").Find("OffTheBoardNorth").transform.position;
            }
        }

        private static void ProcessHeavyGeometryCalculations(GenericShip ship, out float minDistanceToEnemyShip, out float minDistanceToNearestEnemyInShotRange, out float minAngle, out int enemiesInShotRange, out int enemiesTargetingThisShip)
        {
            minDistanceToEnemyShip = float.MaxValue;
            minDistanceToNearestEnemyInShotRange = 0;
            minAngle = float.MaxValue;
            enemiesInShotRange = 0;
            enemiesTargetingThisShip = 0;
            ShotInfo enemyCanShootThisShip = null;
            List<GenericShip> potentialTargets = ship.Owner.EnemyShips.Values.ToList();
            if(ship.StrikeTargets!=null && ship.StrikeTargets.Count>0)
            {
                potentialTargets = ship.StrikeTargets.Values.ToList();
            }
            foreach (GenericShip enemyShip in potentialTargets)
            {
                DistanceInfo distInfo = new DistanceInfo(ship, enemyShip);
                if (distInfo.MinDistance.DistanceReal < minDistanceToEnemyShip)
                {
                    minDistanceToEnemyShip = distInfo.MinDistance.DistanceReal;
                }

                ShotInfo shotInfo = new ShotInfo(ship, enemyShip, ship.PrimaryWeapons.First());
                if (shotInfo.IsShotAvailable)
                {
                    enemiesInShotRange++;

                    if (minDistanceToNearestEnemyInShotRange < shotInfo.DistanceReal)
                    {
                        minDistanceToNearestEnemyInShotRange = shotInfo.DistanceReal;
                    }
                }
                // See if this enemy can shoot us.
                enemyCanShootThisShip = new ShotInfo(enemyShip, ship, enemyShip.PrimaryWeapons.First());
                if (enemyCanShootThisShip.IsShotAvailable == true)
                {
                    enemiesTargetingThisShip++;
                }


                Vector3 forward = ship.GetFrontFacing();
                Vector3 toEnemyShip = enemyShip.GetCenter() - ship.GetCenter();
                float angle = Mathf.Abs(Vector3.SignedAngle(forward, toEnemyShip, Vector3.down));
                if (angle < minAngle) minAngle = angle;

                if(ship.IsFleeing)
                {
                    Vector3 escapeEdge = determineEscapeEdge(ship.EscapeEdge);
                    float DistanceToEscapeEdge = Vector3.Distance(ship.GetPosition(), escapeEdge);
                    minDistanceToEnemyShip = Vector3.Distance(ship.GetPosition(), escapeEdge);
                    minDistanceToNearestEnemyInShotRange = Vector3.Distance(ship.GetPosition(), escapeEdge);
                    toEnemyShip = escapeEdge - ship.GetCenter();
                    minAngle = Mathf.Abs(Vector3.SignedAngle(forward, toEnemyShip, Vector3.down));
                    enemiesInShotRange = 0;
                }
            }
        }



        private static void SetVirtualPositionsForShipsWithPreviousActivations(List<GenericShip> orderOfActivation)
        {
            foreach (GenericShip ship in Roster.AllShips.Values)
            {
                if (!orderOfActivation.Contains(ship))
                {
                    VirtualBoard.SwitchToVirtualPosition(ship);
                }
            }
        }

        private static IEnumerator PredictCollisionDetectionOfEnemyShip(GenericShip ship)
        {
            Selection.ThisShip = ship;

            GenericMovement savedMovement = ship.AssignedManeuver;

            // Decide what maneuvers to use as temporary
            string temporyManeuver = (ship.State.IsIonized) ? "1.F.S" : "2.F.S";
            bool isTemporaryManeuverAdded = false;
            if (!ship.HasManeuver(temporyManeuver))
            {
                isTemporaryManeuverAdded = true;
                ship.Maneuvers.Add(temporyManeuver, MovementComplexity.Easy);
            }
            GenericMovement movement = ShipMovementScript.MovementFromString(temporyManeuver);

            // Check maneuver
            ship.SetAssignedManeuver(movement, isSilent: true);
            movement.Initialize();
            movement.IsSimple = true;

            MovementPrediction prediction = new MovementPrediction(ship, movement);
            yield return prediction.CalculateMovementPredicition();

            if (isTemporaryManeuverAdded)
            {
                ship.Maneuvers.Remove(temporyManeuver);
            }

            if (savedMovement != null)
            {
                ship.SetAssignedManeuver(savedMovement, isSilent: true);
            }
            else
            {
                ship.ClearAssignedManeuver();
            }

            VirtualBoard.SetVirtualPositionInfo(ship, prediction.FinalPositionInfo, temporyManeuver);
        }

        private static IEnumerator CheckNextTurnRecursive(GenericShip ship)
        {
            VirtualBoard.RemoveCollisionsExcept(ship);

            bool HasAnyManeuverWithoutOffBoardFinish = false;
            bool HasAnyManeuverWithoutAsteroidCollision = false;

            foreach (string turnManeuver in ship.GetManeuvers().Keys)
            {
                GenericMovement movement = ShipMovementScript.MovementFromString(turnManeuver);

                ship.SetAssignedManeuver(movement, isSilent: true);
                movement.Initialize();
                movement.IsSimple = true;

                MovementPrediction prediction = new MovementPrediction(ship, movement);
                yield return prediction.CalculateMovementPredicition();

                if (!CurrentNavigationResult.isOffTheBoard || CurrentNavigationResult.isEscaped) HasAnyManeuverWithoutOffBoardFinish = true;
                if (CurrentNavigationResult.obstaclesHit == 0) HasAnyManeuverWithoutAsteroidCollision = true;
            }

            CurrentNavigationResult.isOffTheBoardNextTurn = !HasAnyManeuverWithoutOffBoardFinish;
            CurrentNavigationResult.isHitAsteroidNextTurn = !HasAnyManeuverWithoutAsteroidCollision;

            VirtualBoard.ReturnCollisionsExcept(ship);
        }

        private static List<string> GetShortestTurnManeuvers(GenericShip ship)
        {
            List<string> bestTurnManeuvers = new List<string>();

            ManeuverHolder bestTurnManeuver = ship.GetManeuverHolders()
                .Where(n =>
                    n.Bearing == ManeuverBearing.Turn
                    && n.Direction == ManeuverDirection.Left
                )
                .OrderBy(n => n.SpeedIntUnsigned)
                .FirstOrDefault();
            bestTurnManeuvers.Add(bestTurnManeuver.ToString());

            bestTurnManeuver = ship.GetManeuverHolders()
                .Where(n =>
                    n.Bearing == ManeuverBearing.Turn
                    && n.Direction == ManeuverDirection.Right
                )
                .OrderBy(n => n.SpeedIntUnsigned)
                .FirstOrDefault();
            bestTurnManeuvers.Add(bestTurnManeuver.ToString());

            return bestTurnManeuvers;
        }

        public static GenericShip GetNextShipWithoutAssignedManeuver()
        {
            return Roster.GetPlayer(Phases.CurrentSubPhase.RequiredPlayer).Ships.Values
                .Where(n => n.AssignedManeuver == null && !n.State.IsIonized && !n.State.IsDisabled)
                .OrderBy(n => VirtualBoard.Ships[n].OrderToActivate)
                .FirstOrDefault();
        }

        public static GenericShip GetNextShipWithoutFinishedManeuver()
        {
            return Roster.GetPlayer(Phases.CurrentSubPhase.RequiredPlayer).Ships.Values
                .Where(n => !n.IsManeuverPerformed)
                .OrderBy(n => VirtualBoard.Ships[n].OrderToActivate)
                .FirstOrDefault();
        }

        public static void AssignPlannedManeuver(Action callBack)
        {
            ShipMovementScript.SendAssignManeuverCommand(VirtualBoard.Ships[Selection.ThisShip].PlannedManeuverCode);
            GameManagerScript.Wait(0.2f, delegate { Selection.DeselectThisShip(); callBack(); });
        }

        // Low Priority

        private static void ConfigureVirtualBoards()
        {
            if (Phases.RoundCounter == 1) VirtualBoards = new Dictionary<PlayerNo, VirtualBoard>()
            {
                { PlayerNo.Player1, new VirtualBoard() },
                { PlayerNo.Player2, new VirtualBoard() }
            };

            VirtualBoard.Update();
        }

        private static void ShowCalculationsStart()
        {
            Roster.ToggleCalculatingStatus(Phases.CurrentSubPhase.RequiredPlayer, true);
        }

        private static void ShowCalculationsEnd()
        {
            Roster.ToggleCalculatingStatus(Phases.CurrentSubPhase.RequiredPlayer, false);
        }

        private static void RestoreRealBoard()
        {
            VirtualBoard.RestoreBoard();
        }

        private static float GetMinDistanceToEnemyShip(GenericShip ship)
        {
            float minDistanceToEnemyShip = float.MaxValue;

            foreach (GenericShip enemyShip in ship.Owner.EnemyShips.Values)
            {
                DistanceInfo distInfo = new DistanceInfo(ship, enemyShip);
                if (distInfo.MinDistance.DistanceReal < minDistanceToEnemyShip)
                {
                    minDistanceToEnemyShip = distInfo.MinDistance.DistanceReal;
                }
            }

            return minDistanceToEnemyShip;
        }

        private static bool IsActivationBeforeCurrentShip(GenericShip ship)
        {
            return ship.State.Initiative < Selection.ActiveShip.State.Initiative
                || (ship.State.Initiative == Selection.ActiveShip.State.Initiative && ship.Owner.PlayerNo == Phases.PlayerWithInitiative && ship.Owner.PlayerNo != Selection.ActiveShip.Owner.PlayerNo)
                || (ship.State.Initiative == Selection.ActiveShip.State.Initiative && ship.ShipId < Selection.ActiveShip.ShipId && ship.Owner.PlayerNo == Selection.ActiveShip.Owner.PlayerNo);
        }

        // This function will try all moves related to a boost, barrelroll, decloak, SLAM, or tractor.  If the action is from Supernatural Reflexes or Advanced Sensors,
        // it will also test the maneuver that follows it to see if that is better or worse than without taking the action before it.
        public static int TryActionPossibilities(GenericAction actionToTry, bool isBeforeManeuverPhase = false)
        {
            VirtualBoard myBoard = new VirtualBoard();
            GenericShip thisShip = Selection.ActiveShip;
            String bestBoostName = "Straight 1";
            int result = 0;

            //GenericMovement savedMovement = thisShip.AssignedManeuver;

            if (VirtualBoard.Ships[thisShip].NavigationResults == null || isBeforeManeuverPhase)
            {
                return 0;
            }
            int bestPriority = VirtualBoard.Ships[thisShip].NavigationResults.Max(n => n.Value.Priority);

            NavigationResult StartingPosition = VirtualBoard.Ships[thisShip].NavigationResults.First(n => n.Key == thisShip.AssignedManeuver.ToString()).Value;

            //foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
            //{
            //    if(!IsActivationBeforeCurrentShip(ship))
            //    {
            //        VirtualBoard.SwitchToVirtualPosition(ship);
            //    }                
            //}

            float minDistanceToEnemyShip, minDistanceToNearestEnemyInShotRange, minAngle;
            int enemiesInShotRange, enemiesTargetingThisShip;

            ProcessHeavyGeometryCalculations(thisShip, out minDistanceToEnemyShip, out minDistanceToNearestEnemyInShotRange, out minAngle, out enemiesInShotRange, out enemiesTargetingThisShip);

            StartingPosition.distanceToNearestEnemy = minDistanceToEnemyShip;
            StartingPosition.distanceToNearestEnemyInShotRange = minDistanceToNearestEnemyInShotRange;
            StartingPosition.angleToNearestEnemy = minAngle;
            StartingPosition.enemiesInShotRange = enemiesInShotRange;
            StartingPosition.enemiesTargetingThisShip = enemiesTargetingThisShip;

            //foreach(GenericShip ship in Roster.AllShips.Values)
            //{
            //    myBoard.SetVirtualPositionInfo(ship, ship.GetPositionInfo(), "");
            //}

            //foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
            //{
            //    if (!IsActivationBeforeCurrentShip(ship))
            //    {
            //        VirtualBoard.SwitchToRealPosition(ship);
            //    }
            //}

            int startingResult = CalculateBoostPositionPriority(StartingPosition);

            List<BoostMove> AvailableBoostMoves = thisShip.GetAvailableBoostTemplates(new BoostAction());

            int bestBoostResult = 0;
            GenericMovement bestBoostMove = null;
            bool bestMoveStresses = false;

            foreach (BoostMove move in AvailableBoostMoves)
            {
                string selectedBoostHelper = move.Name;
                GenericMovement boostMovement;
                // Use the name of our boost action to generate a GenericMovement of the matching type.
                switch (selectedBoostHelper)
                {
                    case "Straight 1":
                        boostMovement = new StraightBoost(1, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.None);
                        break;
                    case "Bank 1 Left":
                        boostMovement = new BankBoost(1, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.None);
                        break;
                    case "Bank 1 Right":
                        boostMovement = new BankBoost(1, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.None); ;
                        break;
                    case "Turn 1 Right":
                        boostMovement = new TurnBoost(1, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.None);
                        break;
                    case "Turn 1 Left":
                        boostMovement = new TurnBoost(1, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.None);
                        break;
                    default:
                        boostMovement = new StraightBoost(1, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.None);
                        break;
                }

                boostMovement.Initialize();

                myBoard.UpdatePositionInfo(thisShip);
                myBoard.SwitchToRealPosition(thisShip);
                MovementPrediction prediction = new MovementPrediction(thisShip, boostMovement);
                // yield return prediction.CalculateOnlyFinalPositionIgnoringCollisions();
                prediction.CalculateOnlyFinalPositionIgnoringCollisions();

                myBoard.SetVirtualPositionInfo(thisShip, prediction.FinalPositionInfo, prediction.CurrentMovement.ToString());
                myBoard.SwitchToVirtualPosition(thisShip);

                NavigationResult BoostResult = new NavigationResult()
                {
                    movement = prediction.CurrentMovement,
                    isBumped = prediction.IsBumped,
                    isLandedOnObstacle = prediction.IsLandedOnAsteroid,
                    obstaclesHit = prediction.AsteroidsHit.Count,
                    isOffTheBoard = prediction.IsOffTheBoard,
                    isEscaped = determineEscaped(thisShip.EscapeEdge, prediction),
                    minesHit = prediction.MinesHit.Count,
                    isOffTheBoardNextTurn = false, //!NextTurnNavigationResults.Any(n => !n.isOffTheBoard),
                    isHitAsteroidNextTurn = false, //!NextTurnNavigationResults.Any(n => n.obstaclesHit == 0),
                    FinalPositionInfo = prediction.FinalPositionInfo,
                    isFleeing = thisShip.IsFleeing
                };

                if (!prediction.IsOffTheBoard)
                {
                    CheckNextTurnRecursive(thisShip);

                    //foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
                    //{
                    //    if (!IsActivationBeforeCurrentShip(ship))
                    //    {
                    //        VirtualBoard.SwitchToVirtualPosition(ship);
                    //    }
                    //}


                    ProcessHeavyGeometryCalculations(thisShip, out minDistanceToEnemyShip, out minDistanceToNearestEnemyInShotRange, out minAngle, out enemiesInShotRange, out enemiesTargetingThisShip);

                    BoostResult.distanceToNearestEnemy = minDistanceToEnemyShip;
                    BoostResult.distanceToNearestEnemyInShotRange = minDistanceToNearestEnemyInShotRange;
                    BoostResult.angleToNearestEnemy = minAngle;
                    BoostResult.enemiesInShotRange = enemiesInShotRange;
                    BoostResult.enemiesTargetingThisShip = enemiesTargetingThisShip;

                    //foreach (GenericShip ship in CurrentPlayer.EnemyShips.Values)
                    //{
                    //    if (!IsActivationBeforeCurrentShip(ship))
                    //    {
                    //        VirtualBoard.SwitchToRealPosition(ship);
                    //    }
                    //}
                }

                myBoard.SwitchToRealPosition(thisShip);

                int currentBoostResult = CalculateBoostPositionPriority(BoostResult);

                if (move.IsRed || move.IsPurple)
                {
                    // Make red maneuvers a little less optimal.
                    currentBoostResult -= 10;
                }

                if (currentBoostResult > bestBoostResult)
                {
                    bestBoostResult = currentBoostResult;
                    bestBoostMove = boostMovement;
                    bestMoveStresses = move.IsRed;
                    bestBoostName = selectedBoostHelper;
                }

            }

            if (bestBoostResult > startingResult)
            {
                // Boosting is better than staying here!
                // Since the navigation results can reach a maximum of 8,000 (8 enemies in range, none targeting us), we need the range to be up to 100 to match other priorities.
                AiSinglePlan bestPlan = new AiSinglePlan();
                result = bestBoostResult;
                bestPlan.Priority = bestBoostResult;
                bestPlan.currentAction = new BoostAction();
                bestPlan.currentActionMove = bestBoostMove;
                bestPlan.actionName = bestBoostName;
                bestPlan.isRedAction = bestMoveStresses;

                // Give our ship our new plan.
                thisShip.AiPlans.AddPlan(bestPlan);
            }

            return result;
        }

        // Determine how good the position we have been passed is.
        private static int CalculateBoostPositionPriority(NavigationResult CurrentPosition)
        {
            int Priority = 0;
            if (CurrentPosition.isOffTheBoard)
            {
                return 0;
            }
            if (CurrentPosition.isLandedOnObstacle) Priority -= 20000;

            if (CurrentPosition.isOffTheBoardNextTurn) Priority -= 20000;

            // Base our priority off of how many enemies can shoot us versus how many we can shoot.
            if (CurrentPosition.enemiesInShotRange > 0)
            {
                // We have at least one enemy in shot range.  We don't need to maximize our targets.
                Priority += 20;
                if (CurrentPosition.distanceToNearestEnemyInShotRange < 1)
                {
                    // We are at range 1 of a target?
                    Priority += 10;
                }
            }
            // Reduce our priority by the number of enemies that can still target us after the action.
            Priority -= CurrentPosition.enemiesTargetingThisShip * 40;
            if (CurrentPosition.enemiesTargetingThisShip == 0)
            {
                // Any position that leads to no-one attacking us is a pretty good position.
                Priority += 10;
                if (CurrentPosition.enemiesInShotRange > 0)
                {
                    // We have no-one targeting us and at least one target in range.  A really good action!
                    Priority += 30;
                }
            }

            if (CurrentPosition.obstaclesHit > 0)
            {
                Priority -= CurrentPosition.obstaclesHit * 2000;
            }
            Priority -= CurrentPosition.minesHit * 2000;

            if (CurrentPosition.isBumped)
            {
                // Leave space for testing Arvyl and Zeb.  Otherwise, we want to avoid this.
                Priority -= 1000;
            }

            // Set our priority between 0 and 90.
            if (Priority < 0)
            {
                Priority = 0;
            }

            return Priority;
        }

        // Set navigation information for the current ship's position.
        //private static NavigationResult GetCurrentPositionNavigationInfo(GenericShip thisShip, MovementPrediction firstMovement = null, MovementPrediction secondMovement = null)
        //{
        //    NavigationResult currentNavigationResult = new NavigationResult()
        //    {
        //        movement = thisShip.AssignedManeuver,
        //        distanceToNearestEnemy = 0,
        //        distanceToNearestEnemyInShotRange = 0,
        //        enemiesInShotRange = 0,
        //        enemiesTargetingThisShip = 0,
        //        isBumped = thisShip.IsBumped,
        //        isLandedOnObstacle = thisShip.IsLandedOnObstacle,
        //        obstaclesHit = 0,
        //        isOffTheBoard = false,
        //        minesHit = 0,
        //        isOffTheBoardNextTurn = false,
        //        isHitAsteroidNextTurn = false,
        //        FinalPositionInfo = thisShip.GetPositionInfo()
        //    };

        //    // Duplicate all information we can from our movement prediction(s).
        //    if (firstMovement != null)
        //    {
        //        if (secondMovement != null)
        //        {
        //            currentNavigationResult.movement = secondMovement.CurrentMovement;
        //            currentNavigationResult.isBumped = (secondMovement.IsBumped || firstMovement.IsBumped);
        //            currentNavigationResult.isLandedOnObstacle = (secondMovement.IsLandedOnAsteroid || firstMovement.IsLandedOnAsteroid);
        //            currentNavigationResult.obstaclesHit = (secondMovement.AsteroidsHit.Count + firstMovement.AsteroidsHit.Count);
        //            currentNavigationResult.isOffTheBoard = (secondMovement.IsOffTheBoard || firstMovement.IsOffTheBoard);
        //            currentNavigationResult.minesHit = (secondMovement.MinesHit.Count + firstMovement.MinesHit.Count);
        //        }
        //        else
        //        {
        //            currentNavigationResult.movement = firstMovement.CurrentMovement;
        //            currentNavigationResult.isBumped = firstMovement.IsBumped;
        //            currentNavigationResult.isLandedOnObstacle = firstMovement.IsLandedOnAsteroid;
        //            currentNavigationResult.obstaclesHit = firstMovement.AsteroidsHit.Count;
        //            currentNavigationResult.isOffTheBoard = firstMovement.IsOffTheBoard;
        //            currentNavigationResult.minesHit = firstMovement.MinesHit.Count;
        //        }
        //    }

        //    int enemiesInShotRange = 0;

        //    float minDistanceToNearestEnemyInShotRange = float.MaxValue;

        //    foreach (GenericShip enemyShip in thisShip.Owner.EnemyShips.Values)
        //    {
        //        if (IsActivationBeforeCurrentShip(enemyShip))
        //        {
        //            // This enemy acts before us.  They won't be able to leave our targeting arc.
        //            // Get our weapon shot info for each arc that has a weapon pointing in it.
        //            foreach (IShipWeapon currentWeapon in thisShip.GetAllWeapons())
        //            {
        //                ShotInfo shotInfo = new ShotInfo(thisShip, enemyShip, currentWeapon);
        //                if (shotInfo.IsShotAvailable)
        //                {
        //                    // We are looking for our closest enemy target.
        //                    enemiesInShotRange++;
        //                    if (minDistanceToNearestEnemyInShotRange > shotInfo.DistanceReal) minDistanceToNearestEnemyInShotRange = shotInfo.DistanceReal;
        //                }
        //            }
        //        }
        //    }

        //    currentNavigationResult.enemiesInShotRange = enemiesInShotRange;
        //    currentNavigationResult.distanceToNearestEnemyInShotRange = minDistanceToNearestEnemyInShotRange;

        //    // Find the nearest distance to an enemy ship.
        //    float minDistanceToEnemyShip = float.MaxValue;
        //    ShotInfo enemyCanShootThisShip = null;
        //    foreach (GenericShip enemyShip in thisShip.Owner.EnemyShips.Values)
        //    {
        //        DistanceInfo distInfo = new DistanceInfo(thisShip, enemyShip);
        //        if (distInfo.MinDistance.DistanceReal < minDistanceToEnemyShip)
        //        {
        //            if (distInfo.MinDistance.DistanceReal != 0 || thisShip.IsBumped == true)
        //            {
        //                // A value of 0 is false unless we've bumped. 
        //                minDistanceToEnemyShip = distInfo.MinDistance.DistanceReal;
        //            }

        //        }

        //        // See if this enemy can shoot us with any of its weapons.
        //        foreach (IShipWeapon currentWeapon in thisShip.GetAllWeapons())
        //        {
        //            enemyCanShootThisShip = new ShotInfo(enemyShip, thisShip, currentWeapon);
        //            if (enemyCanShootThisShip.IsShotAvailable == true)
        //            {
        //                currentNavigationResult.enemiesTargetingThisShip++;
        //                break;
        //            }
        //        }
        //    }

        //    currentNavigationResult.distanceToNearestEnemy = minDistanceToEnemyShip;

        //    return currentNavigationResult;
        //}

    }
}