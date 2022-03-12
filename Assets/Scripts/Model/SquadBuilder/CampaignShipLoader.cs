using Editions;
using ExtraOptions.ExtraOptionsList;
using Players;
using SubPhases;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using BoardTools;
using Ship;
using GameCommands;
using GameModes;
using System;


namespace SquadBuilderNS
{
    public class DeploymentConfig
    {
        public List<GenericShip> Ships { get; set; }
        public GameObject StartingZone { get; set; }

        public DeploymentConfig(List<GenericShip> Ships, GameObject StartingZone)
        {
            this.Ships = Ships;
            this.StartingZone = StartingZone;
        }
    }
    public class CampaignShipLoader
    {
        private float FormationWidth { get; set; }

        private Dictionary<GenericShip, Vector3> PlannedShipPositions = new Dictionary<GenericShip, Vector3>();

        private int ShipDirection { get; set; }

        public void LoadCampaign()
        {
            Phases.Events.OnSetupStart += InitialSetup;
            Phases.Events.OnPlanningPhaseStart += MidGameShipAddition;
        }

        private void InitialSetup()
        {
            List<DeploymentConfig> deploymentConfigs = new List<DeploymentConfig>();
            List<GenericShip> deploymentGroup = new List<GenericShip>();
            for (int i = 1; i < 3; i++)
            {
                GenericShip testShip = new Ship.SecondEdition.TIELnFighter.AcademyPilot();
                SquadListShip newship = Global.SquadBuilder.SquadLists[PlayerNo.Player2].AddShip(testShip);
                ShipFactory.SpawnShip(newship);
                deploymentGroup.Add(testShip);
                Roster.AddShipToLists(testShip);
            }
            DeploymentConfig deploymentConfig = new DeploymentConfig(deploymentGroup, Board.StartingZoneCampaign3);
            deploymentConfigs.Add(deploymentConfig);

            List<GenericShip> deploymentGroup2 = new List<GenericShip>();
            for (int i = 1; i < 3; i++)
            {
                GenericShip testShip = new Ship.SecondEdition.TIELnFighter.AcademyPilot();
                SquadListShip newship = Global.SquadBuilder.SquadLists[PlayerNo.Player2].AddShip(testShip);
                ShipFactory.SpawnShip(newship);
                deploymentGroup2.Add(testShip);
                Roster.AddShipToLists(testShip);
            }
            DeploymentConfig deploymentConfig2 = new DeploymentConfig(deploymentGroup2, Board.StartingZoneCampaign4);
            deploymentConfigs.Add(deploymentConfig2);

            var subphase = Phases.StartTemporarySubPhaseNew<SetupCampaignShipSubPhase>(
                "Setup",
                delegate {
                }
            );

            subphase.DeploymentConfigs = deploymentConfigs;

            subphase.Start();

            //for (int i = 1; i < 3; i++)
            //{
            //    GenericShip testShip = new Ship.SecondEdition.TIELnFighter.AcademyPilot();
            //    SquadListShip newship = Global.SquadBuilder.SquadLists[PlayerNo.Player2].AddShip(testShip);
            //    ShipFactory.SpawnShip(newship);
            //    deploymentGroup.Add(testShip);
            //    Roster.AddShipToLists(testShip);
            //}

            //var subphase2 = Phases.StartTemporarySubPhaseNew<SetupCampaignShipSubPhase>(
            //    "Setup",
            //    delegate {
            //    }
            //);

            //subphase2.DeploymentGroup = deploymentGroup;
            //subphase2.StartingZone = Board.StartingZoneCampaign4;

            //subphase2.Start();
        }

        private void MidGameShipAddition()
        {
            if (Phases.RoundCounter == 4)
            {
                List<GenericShip> deploymentGroup = new List<GenericShip>();
                for (int i = 1; i < 2; i++)
                {
                    GenericShip testShip = new Ship.SecondEdition.TIEInterceptor.AlphaSquadronPilot();
                    SquadListShip newship = Global.SquadBuilder.SquadLists[PlayerNo.Player2].AddShip(testShip);
                    ShipFactory.SpawnShip(newship);
                    deploymentGroup.Add(testShip);
                    Roster.AddShipToLists(testShip);
                }
                var subphase = Phases.StartTemporarySubPhaseNew<SetupCampaignShipSubPhase>(
                    "Setup",
                    delegate { }
                );
                subphase.DeploymentConfigs = new[] { new DeploymentConfig(deploymentGroup, RandomStartZone(1, 6)) }.ToList();

                subphase.Start();
            }
            if (Phases.RoundCounter == 7)
            {
                List<GenericShip> deploymentGroup = new List<GenericShip>();
                for (int i = 1; i < 3; i++)
                {
                    GenericShip testShip = new Ship.SecondEdition.TIELnFighter.AcademyPilot();
                    SquadListShip newship = Global.SquadBuilder.SquadLists[PlayerNo.Player2].AddShip(testShip);
                    ShipFactory.SpawnShip(newship);
                    deploymentGroup.Add(testShip);
                    Roster.AddShipToLists(testShip);
                }
                var subphase = Phases.StartTemporarySubPhaseNew<SetupCampaignShipSubPhase>(
                    "Setup",
                    delegate { }
                );
                subphase.DeploymentConfigs = new[] { new DeploymentConfig(deploymentGroup, RandomStartZone(1, 6)) }.ToList();

                subphase.Start();
            }
        }


        private GameObject RandomStartZone(int min, int max)
        {
            int randomVector = UnityEngine.Random.Range(min, max);
            switch (randomVector)
            {
                case 1:
                    return Board.StartingZoneCampaign1;
                case 2:
                    return Board.StartingZoneCampaign2;
                case 3:
                    return Board.StartingZoneCampaign3;
                case 4:
                    return Board.StartingZoneCampaign4;
                case 5:
                    return Board.StartingZoneCampaign5;
                case 6:
                    return Board.StartingZoneCampaign6;
                default:
                    return Board.StartingZoneCampaign1;
            }
        }

    }
    public static class CampaignLoader
    {
        public static void LoadCampaign()
        {
            CampaignShipLoader currentCampaignShipLoader = new CampaignShipLoader();
            currentCampaignShipLoader.LoadCampaign();
        }
    }
}
