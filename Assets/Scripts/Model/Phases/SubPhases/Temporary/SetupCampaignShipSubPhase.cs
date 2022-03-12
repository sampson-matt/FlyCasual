using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Ship;
using BoardTools;
using GameModes;
using GameCommands;
using System;
using System.Globalization;
using SquadBuilderNS;

namespace SubPhases
{

    public class SetupCampaignShipSubPhase : GenericSubPhase
    {
        public override List<GameCommandTypes> AllowedGameCommandTypes { get { return new List<GameCommandTypes>() { GameCommandTypes.ShipPlacement }; } }

        public List<DeploymentConfig> DeploymentConfigs;

        private float FormationWidth { get; set; }

        private Dictionary<GenericShip, Vector3> PlannedShipPositions = new Dictionary<GenericShip, Vector3>();

        private int ShipDirection { get; set; }

        public override void Start()
        {
            IsTemporary = true;

            Prepare();
            Initialize();

            UpdateHelpInfo();
            foreach (DeploymentConfig deploymentConfig in DeploymentConfigs)
            {
                DeployGroup(deploymentConfig.StartingZone, deploymentConfig.Ships);
            }

            base.Start();

        }

        public override void Prepare()
        {

        }

        public override void Initialize()
        {
            IsReadyForCommands = true;
        }

        public override void Next()
        {
            if (GameController.CommandsReceived.Any())
            {
                Initialize();
            }
            else
            {
                FinishSubPhase();
            }
        }

        private void FinishSubPhase()
        {
            HideSubphaseDescription();

            Phases.CurrentSubPhase = Phases.CurrentSubPhase.PreviousSubPhase;
            Phases.CurrentSubPhase.Next();

        }

        private void DeployGroup(GameObject StartingZone, List<GenericShip> DeploymentGroup)
        {
            if (StartingZone.Equals(Board.StartingZoneCampaign1) || StartingZone.Equals(Board.StartingZoneCampaign2))
            {
                GenerateShipFormation(DeploymentGroup, PlotShipLeft);
            }
            if (StartingZone.Equals(Board.StartingZoneCampaign3) || StartingZone.Equals(Board.StartingZoneCampaign4))
            {
                GenerateShipFormation(DeploymentGroup, PlotShipTop);
            }
            if (StartingZone.Equals(Board.StartingZoneCampaign5) || StartingZone.Equals(Board.StartingZoneCampaign6))
            {
                GenerateShipFormation(DeploymentGroup, PlotShipRight);
            }
            AdjustShipFormationToDeploymentZoneCenter(StartingZone, DeploymentGroup);
            SetupShip(DeploymentGroup);
            PlannedShipPositions.Clear();
            FormationWidth = 0;
        }

        private void SetupShip(List<GenericShip> DeploymentGroup)
        {
            foreach (GenericShip shipToSetup in DeploymentGroup)
            {
                Selection.ChangeActiveShip(shipToSetup);
                GenerateSetupShip(shipToSetup);
            }
        }

        private void GenerateSetupShip(GenericShip shipToSetup)
        {
            GameCommand command = GeneratePlaceShipCommand(
            shipToSetup.ShipId,
            PlannedShipPositions[shipToSetup],
            shipToSetup.GetAngles());
            GameMode.CurrentGameMode.ExecuteCommand(command);
        }

        private void AdjustShipFormationToDeploymentZoneCenter(GameObject StartingZone, List<GenericShip> DeploymentGroup)
        {
            float xCenter = StartingZone.transform.position.x;

            float xShift = xCenter - Board.BoardIntoWorld(FormationWidth / 2f);

            float zCenter = StartingZone.transform.position.z;

            float zShift = zCenter - Board.BoardIntoWorld(FormationWidth / 2f);

            foreach (GenericShip ship in DeploymentGroup)
            {
                if (StartingZone.Equals(Board.StartingZoneCampaign3) || StartingZone.Equals(Board.StartingZoneCampaign4))
                {
                    PlannedShipPositions[ship] += new Vector3(xShift, 0, Board.BoardIntoWorld(91.44f / 2f) + -1 * Board.BoardIntoWorld(Board.RANGE_1));
                }
                else if (StartingZone.Equals(Board.StartingZoneCampaign1) || StartingZone.Equals(Board.StartingZoneCampaign2))
                {
                    PlannedShipPositions[ship] += new Vector3(-1 * Board.BoardIntoWorld(91.44f / 2f) + Board.BoardIntoWorld(Board.RANGE_1), 0, zShift);
                    ship.SetAngles(new Vector3(0, 90f, 0));
                }
                else if (StartingZone.Equals(Board.StartingZoneCampaign5) || StartingZone.Equals(Board.StartingZoneCampaign6))
                {
                    PlannedShipPositions[ship] += new Vector3(Board.BoardIntoWorld(91.44f / 2f) - Board.BoardIntoWorld(Board.RANGE_1), 0, zShift);
                    ship.SetAngles(new Vector3(0, -90f, 0));
                }
            }
        }

        private Vector3 PlotShipTop(float currentPosition, GenericShip smallShip, bool isSecondRow)
        {
            if (isSecondRow)
            {
                return new Vector3(currentPosition + Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f),
                                0,
                                -1 * (-Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM + 2f)));
            }
            return new Vector3(currentPosition + Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f), 0, 0);
        }

        private Vector3 PlotShipLeft(float currentPosition, GenericShip smallShip, bool isSecondRow)
        {
            if (isSecondRow)
            {
                return new Vector3(-Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM + 2f),
                                0,
                                currentPosition + Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f));

            }

            return new Vector3(0, 0, currentPosition + Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f));
        }

        private Vector3 PlotShipRight(float currentPosition, GenericShip smallShip, bool isSecondRow)
        {
            if (isSecondRow)
            {
                return new Vector3(-1 * (-Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM + 2f)),
                                0,
                                currentPosition + Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f));

            }

            return new Vector3(0, 0, currentPosition + Board.BoardIntoWorld(smallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f));
        }

        private void GenerateShipFormation(List<GenericShip> DeploymentGroup, Func<float, GenericShip, bool, Vector3> PlotFunction)
        {
            float currentPosition = 0;

            // Not small ships - in single row

            foreach (GenericShip notSmallShip in DeploymentGroup.Where(n => n.ShipInfo.BaseSize != BaseSize.Small))
            {
                PlannedShipPositions.Add(
                    notSmallShip,
                    new Vector3(currentPosition + Board.BoardIntoWorld(notSmallShip.ShipBase.SHIPSTAND_SIZE_CM / 2f), 0, 0)
                );

                currentPosition += Board.BoardIntoWorld(notSmallShip.ShipBase.SHIPSTAND_SIZE_CM + 2f);
                FormationWidth += notSmallShip.ShipBase.SHIPSTAND_SIZE_CM + 2f;
            }

            // Small ships - can be in 2 rows

            List<GenericShip> smallShips = DeploymentGroup.Where(n => n.ShipInfo.BaseSize == BaseSize.Small).ToList();
            float currentPositionSecondRow = currentPosition;
            float secondRowShift = (smallShips.Count % 2 == 0) ? 0 : Board.BoardIntoWorld(3f);

            for (int i = 0; i < smallShips.Count; i++)
            {
                if (!ShouldBeInSecondRow(DeploymentGroup, smallShips[i], i))
                {
                    PlannedShipPositions.Add(
                        smallShips[i], PlotFunction(currentPosition, smallShips[i], false)
                    );

                    currentPosition += Board.BoardIntoWorld(smallShips[i].ShipBase.SHIPSTAND_SIZE_CM + 2f);
                    FormationWidth += smallShips[i].ShipBase.SHIPSTAND_SIZE_CM + 2f;
                }
                else
                {
                    PlannedShipPositions.Add(smallShips[i], (PlotFunction(currentPositionSecondRow, smallShips[i], true)));

                    currentPositionSecondRow += Board.BoardIntoWorld(smallShips[i].ShipBase.SHIPSTAND_SIZE_CM + 2f);
                }
            }

            FormationWidth -= 2f;
        }

        private bool ShouldBeInSecondRow(List<GenericShip> DeploymentGroup, GenericShip ship, int count)
        {
            // only second part of small ships
            if (count + 1 > (DeploymentGroup.Count(n => n.ShipInfo.BaseSize == BaseSize.Small) + 1) / 2)
            {
                //if only 3 small ships without larger ships - still 1 row
                if (DeploymentGroup.Count(n => n.ShipInfo.BaseSize == BaseSize.Small) < 3 && DeploymentGroup.Count(n => n.ShipInfo.BaseSize != BaseSize.Small) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }



        public static GameCommand GeneratePlaceShipCommand(int shipId, Vector3 position, Vector3 angles)
        {
            JSONObject parameters = new JSONObject();

            parameters.AddField("id", shipId.ToString());

            parameters.AddField("positionX", position.x.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("positionY", position.y.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("positionZ", position.z.ToString(CultureInfo.InvariantCulture));

            parameters.AddField("rotationX", angles.x.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("rotationY", angles.y.ToString(CultureInfo.InvariantCulture));
            parameters.AddField("rotationZ", angles.z.ToString(CultureInfo.InvariantCulture));

            return GameController.GenerateGameCommand(
                GameCommandTypes.ShipPlacement,
                typeof(SetupCampaignShipSubPhase),
                Phases.CurrentSubPhase.ID,
                parameters.ToString()
            );
        }

        private void StopDrag()
        {


            GameCommand command = GeneratePlaceShipCommand(Selection.ThisShip.ShipId, Selection.ThisShip.GetPosition(), Selection.ThisShip.GetAngles());
            GameMode.CurrentGameMode.ExecuteCommand(command);
        }

    }

}
