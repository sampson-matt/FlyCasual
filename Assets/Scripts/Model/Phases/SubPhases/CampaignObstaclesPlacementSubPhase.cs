using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BoardTools;
using UnityEngine.EventSystems;
using Obstacles;
using Players;
using UnityEngine.UI;
using System;
using GameModes;
using GameCommands;
using System.Globalization;

namespace SubPhases
{

    public class CampaignObstaclesPlacementSubPhase : GenericSubPhase
    {
        public override List<GameCommandTypes> AllowedGameCommandTypes { get { return new List<GameCommandTypes>() { GameCommandTypes.ObstaclePlacement, GameCommandTypes.PressSkip, GameCommandTypes.PressNext }; } }

        private bool IsRangeRulerNeeded { get {return Roster.GetPlayer(Phases.CurrentSubPhase.RequiredPlayer).GetType() == typeof(HumanPlayer); } }

        public static GenericObstacle ChosenObstacle;
        private float MinBoardEdgeDistance;
        private float MinObstaclesDistance;
        private static bool IsPlacementBlocked;
        private static bool IsEnteredPlaymat;
        private static bool IsEnteredPlacementZone;
        public static bool IsLocked;
        private List<GenericObstacle> ChosenObstacles { get; } = new List<GenericObstacle>();

        public override void Start()
        {
            Name = "Obstacle Setup";
            UpdateHelpInfo();
        }

        public override void Initialize()
        {
            Console.Write($"\nSetup Phase", isBold: true, color: "orange");

            ShowObstaclesHolder();

            MinBoardEdgeDistance = Board.BoardIntoWorld(2 * Board.RANGE_1);
            MinObstaclesDistance = Board.BoardIntoWorld(Board.RANGE_1);

            ChosenObstacle = null;

            ObstaclesManager.SetObstaclesCollisionDetectionQuality(CollisionDetectionQuality.Low);

            if (ChosenObstacles.Count > 0)
            {
                Next();
            }
            else
            {
                FinishSubPhase();
            }
        }

        private void ShowObstaclesHolder()
        {
            Board.ToggleObstaclesHolder(true);

            ObstaclesManager.Instance.ChosenObstacles.Clear();
            ChosenObstacles.Clear();
            for(int i = 0; i<6; i++)
            {
                ChosenObstacles.Add(ObstaclesManager.GetRandomAsteroid());
                GameObject obstacleHolder = Board.GetObstacleHolder().Find("Obstacle" + 1).gameObject;
                ChosenObstacles[i].Spawn(ChosenObstacles[i].Name + " " + i, obstacleHolder.transform);
            }
        }

        public override void Next()
        {
            HideSubphaseDescription();
            IsLocked = false;
            IsReadyForCommands = true;
            PlaceRandom();
        }

        private static void FinishSubPhase()
        {
            HideSubphaseDescription();

            Board.ToggleObstaclesHolder(false);
            Board.ToggleOffTheBoardHolder(true);
            ObstaclesManager.SetObstaclesCollisionDetectionQuality(CollisionDetectionQuality.High);

            GenericSubPhase subphase = Phases.StartTemporarySubPhaseNew("Notification", typeof(NotificationSubPhase), StartSetupPhase);
            (subphase as NotificationSubPhase).TextToShow = "Setup";
            subphase.Start();
        }

        private static void StartSetupPhase()
        {
            Phases.CurrentSubPhase = new SetupStartSubPhase();
            Phases.CurrentSubPhase.Start();
            Phases.CurrentSubPhase.Prepare();
            Phases.CurrentSubPhase.Initialize();
        }

        public override bool ThisShipCanBeSelected(Ship.GenericShip ship, int mouseKeyIsPressed)
        {
            return false;
        }

        public override bool AnotherShipCanBeSelected(Ship.GenericShip targetShip, int mouseKeyIsPressed)
        {
            return false;
        }

        private void CheckEntered()
        {
            Vector3 position = ChosenObstacle.ObstacleGO.transform.position;

            if (!IsEnteredPlacementZone)
            {
                if (Mathf.Abs(position.x) < 2.7f && Mathf.Abs(position.z) < 2.7f)
                {
                    IsEnteredPlacementZone = true;
                }
            }

            if (!IsEnteredPlaymat)
            {
                if (Mathf.Abs(position.x) < 5f && Mathf.Abs(position.z) < 5f)
                {
                    IsEnteredPlaymat = true;
                }
            }

            if (IsEnteredPlaymat)
            {
                if (position.x < -4.5f) ChosenObstacle.ObstacleGO.transform.position = new Vector3(-4.5f, position.y, position.z);
                if (position.x > 4.5f) ChosenObstacle.ObstacleGO.transform.position = new Vector3(4.5f, position.y, position.z);

                position = ChosenObstacle.ObstacleGO.transform.position;
                if (position.z < -4.5f) ChosenObstacle.ObstacleGO.transform.position = new Vector3(position.x, position.y, -4.5f);
                if (position.z > 4.5f) ChosenObstacle.ObstacleGO.transform.position = new Vector3(position.x, position.y, 4.5f);
            }
        }

        private void CheckLimits()
        {
            IsPlacementBlocked = false;
            ApplyEdgeLimits();
            ApplyObstacleLimits();
        }

        private void ApplyEdgeLimits()
        {
            MovementTemplates.ReturnRangeRulerR2();

            Vector3 fromEdge = Vector3.zero;
            Vector3 toObstacle = Vector3.zero;
            float minDistance = float.MaxValue;

            bool IsShiftRequired = false;

            foreach (BoxCollider collider in Board.BoardTransform.Find("OffTheBoardHolder").GetComponentsInChildren<BoxCollider>())
            {
                Vector3 closestPoint = collider.ClosestPoint(ChosenObstacle.ObstacleGO.transform.position);

                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(closestPoint + new Vector3(0, 0.003f, 0), ChosenObstacle.ObstacleGO.transform.position - closestPoint, out hitInfo))
                {
                    float distanceFromEdge = Vector3.Distance(closestPoint, hitInfo.point);
                    if (distanceFromEdge < MinBoardEdgeDistance)
                    {
                        IsShiftRequired = true;

                        if (distanceFromEdge < minDistance)
                        {
                            fromEdge = closestPoint;
                            toObstacle = hitInfo.point;
                            minDistance = distanceFromEdge;
                        }

                        MoveObstacleToKeepInPlacementZone(closestPoint, hitInfo.point);
                    }
                }
            }

            if (IsShiftRequired && IsRangeRulerNeeded)
            {
                MovementTemplates.ShowRangeRulerR2(fromEdge, toObstacle);
            }
        }

        private void MoveObstacleToKeepInPlacementZone(Vector3 pointOnEdge, Vector3 nearestPoint)
        {
            Vector3 disallowedVector = nearestPoint - pointOnEdge;
            Vector3 allowedVector = disallowedVector / Vector3.Distance(pointOnEdge, nearestPoint) * MinBoardEdgeDistance;

            Vector3 shift = (allowedVector - disallowedVector);
            ChosenObstacle.ObstacleGO.transform.position += shift;
        }

        private void ApplyObstacleLimits()
        {
            MovementTemplates.ReturnRangeRulerR1();

            Vector3 fromObstacle = Vector3.zero;
            Vector3 toObstacle = Vector3.zero;
            float minDistance = float.MaxValue;

            bool isBlockedByAnotherObstacle = false;

            foreach (MeshCollider collider in ObstaclesManager.GetPlacedObstacles().Select(n => n.ObstacleGO.GetComponentInChildren<MeshCollider>()))
            {
                Vector3 closestPoint = collider.ClosestPoint(ChosenObstacle.ObstacleGO.transform.position);

                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(closestPoint + new Vector3(0, 0.003f, 0), ChosenObstacle.ObstacleGO.transform.position - closestPoint, out hitInfo))
                {
                    float distanceBetween = Vector3.Distance(closestPoint, hitInfo.point);
                    if (distanceBetween < MinObstaclesDistance)
                    {
                        IsPlacementBlocked = true;
                        isBlockedByAnotherObstacle = true;

                        if (distanceBetween < minDistance)
                        {
                            fromObstacle = closestPoint + new Vector3(0, 0.003f, 0);
                            toObstacle = hitInfo.point;
                            minDistance = distanceBetween;
                        }
                    }
                }

                // In case if one asteroid is inside of another
                float distanceToCenter = Vector3.Distance(closestPoint, ChosenObstacle.ObstacleGO.transform.position);
                if (distanceToCenter < MinObstaclesDistance)
                {
                    IsPlacementBlocked = true;
                    isBlockedByAnotherObstacle = true;

                    if (distanceToCenter < minDistance)
                    {
                        fromObstacle = closestPoint;
                        toObstacle = ChosenObstacle.ObstacleGO.transform.position;
                        minDistance = distanceToCenter;
                    }
                }
            }

            if (IsPlacementBlocked && isBlockedByAnotherObstacle && IsRangeRulerNeeded)
            {
                MovementTemplates.ShowRangeRulerR1(fromObstacle, toObstacle);
            }
        }

        private void SelectObstacle(GenericObstacle obstacle)
        {
            Board.HighlightOfStartingZoneOff();
            Board.ToggleOffTheBoardHolder(true);

            ChosenObstacle = obstacle;

            UI.HideSkipButton();
        }

        private bool TryToPlaceObstacle()
        {
            if (IsEnteredPlacementZone && !IsPlacementBlocked && !IsLocked)
            {
                IsLocked = true;

                GameCommand command = GeneratePlaceObstacleCommand(
                    ChosenObstacle.Name,
                    ChosenObstacle.ObstacleGO.transform.position,
                    ChosenObstacle.ObstacleGO.transform.eulerAngles
                );
                GameMode.CurrentGameMode.ExecuteCommand(command);
                return true;
            }
            else
            {
                Messages.ShowError("The obstacle cannot be placed");
                IsLocked = false;
                return false;
            }
        }

        private GameCommand GeneratePlaceObstacleCommand(string obstacleName, Vector3 position, Vector3 angles)
        {
            JSONObject parameters = new JSONObject();

            parameters.AddField("name", obstacleName);
            
            parameters.AddField("positionX", position.x.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("positionY", "0");
            parameters.AddField("positionZ", position.z.ToString(CultureInfo.InvariantCulture));

            parameters.AddField("rotationX", angles.x.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("rotationY", angles.y.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("rotationZ", angles.z.ToString(CultureInfo.InvariantCulture));

            return GameController.GenerateGameCommand(
                GameCommandTypes.ObstaclePlacement,
                typeof(CampaignObstaclesPlacementSubPhase),
                Phases.CurrentSubPhase.ID,
                parameters.ToString()
            );
        }

        public static void PlaceObstacle(string obstacleName,  Vector3 position, Vector3 angles)
        {
            ChosenObstacle = ObstaclesManager.GetChosenObstacle(obstacleName);
            ChosenObstacle.ObstacleGO.transform.position = position;
            ChosenObstacle.ObstacleGO.transform.eulerAngles = angles;

            Board.ToggleOffTheBoardHolder(false);

            ChosenObstacle.ObstacleGO.transform.parent = Board.BoardTransform;

            ChosenObstacle.IsPlaced = true;
            ChosenObstacle = null;
            IsEnteredPlacementZone = false;
            IsEnteredPlaymat = false;

            MovementTemplates.ReturnRangeRulerR1();
            MovementTemplates.ReturnRangeRulerR2();

            if (ObstaclesManager.GetPlacedObstaclesCount() < ObstaclesManager.Instance.ChosenObstacles.Count)
            {
                Phases.CurrentSubPhase.Next();
            }
            else
            {
                FinishSubPhase();
            }
        }

        public void PlaceRandom()
        {
            GetRandomObstacle();
            SetRandomPosition();
        }

        private void GetRandomObstacle()
        {
            SelectObstacle(ObstaclesManager.GetRandomFreeObstacle());
        }

        private void SetRandomPosition()
        {
            float randomX = UnityEngine.Random.Range(-2.7f, 2.7f);
            float randomZ = UnityEngine.Random.Range(-2.7f, 2.7f);
            float randomRot = UnityEngine.Random.Range(-180, 180);

            ChosenObstacle.ObstacleGO.transform.position = new Vector3(randomX, 0, randomZ);
            ChosenObstacle.ObstacleGO.transform.localEulerAngles += new Vector3(0, randomRot, 0);
            CheckEntered();
            CheckLimits();

            if (!IsPlacementBlocked)
            {
                TryToPlaceObstacle();
            }
            else
            {
                SetRandomPosition();
            }
        }
    }

}
