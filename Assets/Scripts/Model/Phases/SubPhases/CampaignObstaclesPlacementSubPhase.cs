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
using SquadBuilderNS;
using Ship;
using Remote;

namespace SubPhases
{

    public class CampaignObstaclesPlacementSubPhase : GenericSubPhase
    {
        public override List<GameCommandTypes> AllowedGameCommandTypes { get { return new List<GameCommandTypes>() { GameCommandTypes.ObstaclePlacement, GameCommandTypes.PressSkip, GameCommandTypes.PressNext }; } }

        private bool IsRangeRulerNeeded { get {return Roster.GetPlayer(Phases.CurrentSubPhase.RequiredPlayer).GetType() == typeof(HumanPlayer); } }

        public static GenericObstacle ChosenObstacle;
        private int MinXint;
        private int MinYint;
        private float MinBoardEdgeDistanceX;
        private float MinBoardEdgeDistanceY;
        private float MinDistanceFromCenterX;
        private float MinDistanceFromCenterY;
        private float MinObstaclesDistance;
        private float MaxObstaclesDistance;
        private int numberObstacles;
        private static bool IsPlacementBlocked;
        public static bool IsLocked;
        private string obstacleType;
        private List<Vector3> minePlacements { get; } = new List<Vector3>();
        private List<GenericObstacle> ChosenObstacles { get; } = new List<GenericObstacle>();

        public override void Start()
        {
            Name = "Obstacle Setup";
            UpdateHelpInfo();
        }

        public override void Initialize()
        {
            Console.Write($"\nSetup Phase", isBold: true, color: "orange");

            MinXint = 2;
            MinYint = 2;
            MaxObstaclesDistance = float.MaxValue;
            numberObstacles = 0;

            LoadCampaingObstacles();

            MinBoardEdgeDistanceX = Board.BoardIntoWorld(MinXint * Board.RANGE_1);
            MinDistanceFromCenterX = 5 - MinBoardEdgeDistanceX;

            MinBoardEdgeDistanceY = Board.BoardIntoWorld(MinYint * Board.RANGE_1);
            MinDistanceFromCenterY = 5 - MinBoardEdgeDistanceY;

            //MinObstaclesDistance = Board.BoardIntoWorld(Board.RANGE_1);

            ShowObstaclesHolder();
            
            ChosenObstacle = null;

            ObstaclesManager.SetObstaclesCollisionDetectionQuality(CollisionDetectionQuality.Low);

            if(obstacleType=="mineField")
            {
                PlaceMines();
            }

            if (ChosenObstacles.Count > 0)
            {
                Next();
            }
            else
            {
                FinishSubPhase();
            }
        }

        private void PlaceMines()
        {
            for(int i =0; i<numberObstacles; i++)
            {
                Vector3 minePlacement = generateRandomMinePlacement();                
                minePlacements.Add(minePlacement);
                Quaternion bombRotation = new Quaternion(0, 0, 0, 0);
                GameObject obstacleHolder = Board.GetObstacleHolder().Find("Obstacle" + 1).gameObject;
                Type remoteType = typeof(MineField);
                GenericShip mineField = ShipFactory.SpawnRemote(
                    (GenericRemote)Activator.CreateInstance(remoteType, Roster.Player2),
                    minePlacement,
                    bombRotation
                );
            }
        }

        private Vector3 adjustMineMinimumDistance(Vector3 minePlacement, Vector3 previousMinePlacement)
        {
            float distanceBetween = Vector3.Distance(minePlacement, previousMinePlacement);
            if (distanceBetween < MinObstaclesDistance)
            {
                adjustMineMinimumDistance(generateRandomMinePlacement(), previousMinePlacement);
            }

            return minePlacement;
        }

        private bool withinMaxOfAtLeastOne(Vector3 minePlacement)
        {
            foreach(Vector3 previousMinePlacement in minePlacements)
            {
                float distanceBetween = Vector3.Distance(minePlacement, previousMinePlacement);
                if(distanceBetween<=MaxObstaclesDistance+.4)
                {
                    return true;
                }
            }
            return false;
        }

        private Vector3 generateRandomMinePlacement()
        {
            float randomX = UnityEngine.Random.Range(-MinDistanceFromCenterX+.5f, MinDistanceFromCenterX-.5f);
            float randomZ = UnityEngine.Random.Range(-MinDistanceFromCenterY+.5f, MinDistanceFromCenterY-.5f);
            Vector3 minePlacement = new Vector3(randomX, 0, randomZ);
            foreach(Vector3 previousMinePlacement in minePlacements)
            {
                float distanceBetween = Vector3.Distance(minePlacement, previousMinePlacement);
                if (distanceBetween < MinObstaclesDistance+.3 || !withinMaxOfAtLeastOne(minePlacement))
                {
                    minePlacement = generateRandomMinePlacement();
                }
            }
            return minePlacement;
        }

        private void LoadCampaingObstacles()
        {
            if (CampaignLoader.CampaignMission.HasField("obstacles"))
            {
                JSONObject obstaclesJson = CampaignLoader.CampaignMission["obstacles"];
                foreach (JSONObject obstacle in obstaclesJson.list)
                {
                    if (obstacle.HasField("type"))
                    {
                        obstacleType = obstacle["type"].str;
                    }
                    if (obstacle.HasField("minBoardEdgeDistanceX"))
                    {
                        MinXint = Int16.Parse(obstacle["minBoardEdgeDistanceX"].str);
                    }
                    if (obstacle.HasField("minBoardEdgeDistanceY"))
                    {
                        MinYint = Int16.Parse(obstacle["minBoardEdgeDistanceY"].str);
                    }
                    if (obstacle.HasField("minSeparationDistance"))
                    {
                        MinObstaclesDistance = Int16.Parse(obstacle["minSeparationDistance"].str);
                    }
                    if (obstacle.HasField("maxSeparationDistance"))
                    {
                        MaxObstaclesDistance = Int16.Parse(obstacle["maxSeparationDistance"].str);
                    }
                    if (obstacle.HasField("count"))
                    {
                        numberObstacles = Int16.Parse(obstacle["count"].str);
                    }
                    if (obstacle.HasField("squadCount"))
                    {
                        numberObstacles = Roster.GetPlayer(PlayerNo.Player1).Ships.Count * Int16.Parse(obstacle["squadCount"].str);
                    }
                }
            }
        }

        private void ShowObstaclesHolder()
        {
            Board.ToggleObstaclesHolder(true);

            ObstaclesManager.Instance.ChosenObstacles.Clear();
            ChosenObstacles.Clear();
            if(obstacleType=="asteroids")
            {
                for (int i = 0; i < numberObstacles; i++)
                {
                    ChosenObstacles.Add(ObstaclesManager.GetRandomAsteroid());
                    GameObject obstacleHolder = Board.GetObstacleHolder().Find("Obstacle" + 1).gameObject;
                    ChosenObstacles[i].Spawn(ChosenObstacles[i].Name + " " + i, obstacleHolder.transform);
                }
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
                    float checkDistance = 0f;
                    if (collider.bounds.center.x == 0f)
                    {
                        checkDistance = MinBoardEdgeDistanceY;
                    }
                    else if (collider.bounds.center.z == 0f)
                    {
                        checkDistance = MinBoardEdgeDistanceX;
                    }
                    if (distanceFromEdge < checkDistance)
                    {
                        IsShiftRequired = true;

                        if (distanceFromEdge < minDistance)
                        {
                            fromEdge = closestPoint;
                            toObstacle = hitInfo.point;
                            minDistance = distanceFromEdge;
                        }

                        MoveObstacleToKeepInPlacementZone(closestPoint, hitInfo.point, checkDistance);
                    }
                }
            }

            if (IsShiftRequired && IsRangeRulerNeeded)
            {
                MovementTemplates.ShowRangeRulerR2(fromEdge, toObstacle);
            }
        }

        private void MoveObstacleToKeepInPlacementZone(Vector3 pointOnEdge, Vector3 nearestPoint, float checkDistance)
        {
            Vector3 disallowedVector = nearestPoint - pointOnEdge;
            Vector3 allowedVector = disallowedVector / Vector3.Distance(pointOnEdge, nearestPoint) * checkDistance;

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
            if (!IsPlacementBlocked && !IsLocked)
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

        public static void PlaceShip(int shipId, Vector3 position, Vector3 angles)
        {
            Phases.CurrentSubPhase.IsReadyForCommands = false;

            Roster.SetRaycastTargets(true);
            //inReposition = false;

            Selection.ChangeActiveShip("ShipId:" + shipId);
            Selection.ThisShip.CallOnSetupPlaced();
            Board.PlaceShip(Selection.ThisShip, position, angles, delegate { Selection.DeselectThisShip(); Phases.CurrentSubPhase.Next(); });
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
            float randomX = UnityEngine.Random.Range(-MinDistanceFromCenterX, MinDistanceFromCenterX);
            float randomZ = UnityEngine.Random.Range(-MinDistanceFromCenterY, MinDistanceFromCenterY);
            float randomRot = UnityEngine.Random.Range(-180, 180);

            ChosenObstacle.ObstacleGO.transform.position = new Vector3(randomX, 0, randomZ);
            ChosenObstacle.ObstacleGO.transform.localEulerAngles += new Vector3(0, randomRot, 0);
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
