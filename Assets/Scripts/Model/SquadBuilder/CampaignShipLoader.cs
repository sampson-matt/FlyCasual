using Editions;
using ExtraOptions.ExtraOptionsList;
using Players;
using SubPhases;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BoardTools;
using Ship;
using GameCommands;
using GameModes;
using System;
using Newtonsoft.Json;
using Mods;


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
        
        private int squadSize { get; set; }
        private int averageInitiative { get; set; }

        public void LoadCampaign()
        {
            List<DeploymentConfig> deploymentConfigs = new List<DeploymentConfig>();

            Phases.Events.OnSetupStart += InitialSetup;
            Phases.Events.OnPlanningPhaseStart += MidGameShipAddition;
        }

        private void InitialSetup()
        {
            squadSize = Roster.GetPlayer(PlayerNo.Player1).Ships.Count;
            determineAverageInitiative();
            LoadShipsForRound(0);
        }

        private void determineAverageInitiative()
        {
            List<GenericShip> ships =  Roster.GetPlayer(PlayerNo.Player1).Ships.Values.ToList();
            int initiativeTotal = 0;
            foreach (GenericShip ship in ships)
            {
                initiativeTotal += ship.PilotInfo.Initiative;
            }
            averageInitiative = initiativeTotal / ships.Count;
        }

        private void LoadShipsForRound(int roundNumber)
        {
            List<DeploymentConfig> deploymentConfigs = new List<DeploymentConfig>();

            if (CampaignLoader.campaignMission.HasField("deploymentConfigs"))
            {
                JSONObject deploymentConfigsJson = CampaignLoader.campaignMission["deploymentConfigs"];
                foreach (JSONObject deploymentConfigJson in deploymentConfigsJson.list)
                {
                    List<GenericShip> deploymentGroup = new List<GenericShip>();
                    GameObject startingZone = new GameObject();
                    if (deploymentConfigJson.HasField("deploymentRound") && deploymentConfigJson["deploymentRound"].str.Equals(roundNumber.ToString()))
                    {
                        string factionNameXws = deploymentConfigJson["faction"].str;
                        Faction faction = Edition.Current.XwsToFaction(factionNameXws);

                        if (deploymentConfigJson.HasField("pilots"))
                        {
                            JSONObject pilotJsons = deploymentConfigJson["pilots"];
                            foreach (JSONObject pilotJson in pilotJsons.list)
                            {
                                string squadSizeString = pilotJson["squadSize"].str;

                                int squadSizeValue = Int16.Parse(pilotJson["squadSize"].str);

                                if (squadSizeValue <= squadSize)
                                {
                                    string shipNameXws = pilotJson["ship"].str;

                                    string shipNameGeneral = "";
                                    ShipRecord shipRecord = SquadBuilder.Instance.Database.AllShips.FirstOrDefault(n => n.ShipNameCanonical == shipNameXws);
                                    if (shipRecord == null)
                                    {
                                        Messages.ShowError("Cannot find ship: " + shipNameXws);
                                        continue;
                                    }

                                    shipNameGeneral = shipRecord.ShipName;

                                    string pilotNameXws = pilotJson["id"].str;
                                    PilotRecord pilotRecord = SquadBuilder.Instance.Database.AllPilots.FirstOrDefault(n => n.PilotNameCanonical == pilotNameXws && n.Ship.ShipName == shipNameGeneral && n.PilotFaction == faction);
                                    if (pilotRecord == null)
                                    {
                                        Messages.ShowError("Cannot find pilot: " + pilotNameXws);
                                        continue;
                                    }

                                    GenericShip newShipInstance = (GenericShip)Activator.CreateInstance(Type.GetType(pilotRecord.PilotTypeName));
                                    Edition.Current.AdaptShipToRules(newShipInstance);
                                    SquadListShip newShip = Global.SquadBuilder.SquadLists[PlayerNo.Player2].AddShip(newShipInstance);
                                    
                                    Dictionary<string, string> upgradesThatCannotBeInstalled = new Dictionary<string, string>();

                                    if (pilotJson.HasField("upgrades"))
                                    {
                                        JSONObject upgradeJsons = pilotJson["upgrades"];
                                        InstallCampaignUpgrade(newShip, upgradesThatCannotBeInstalled, upgradeJsons);
                                    }

                                    if(pilotJson.HasField("randomUpgrades"))
                                    {
                                        string upgradeClass = pilotJson["randomUpgrades"].str;
                                        InstallElitePilotUpgrades(upgradeClass, newShip, newShipInstance, upgradesThatCannotBeInstalled);
                                    }

                                    while (true)
                                    {
                                        Dictionary<string, string> upgradesThatCannotBeInstalledCopy = new Dictionary<string, string>(upgradesThatCannotBeInstalled);

                                        bool wasSuccess = false;
                                        foreach (var upgrade in upgradesThatCannotBeInstalledCopy)
                                        {
                                            bool upgradeInstalledSucessfully = newShip.InstallUpgrade(upgrade.Key, Edition.Current.XwsToUpgradeType(upgrade.Value));
                                            if (upgradeInstalledSucessfully)
                                            {
                                                wasSuccess = true;
                                                upgradesThatCannotBeInstalled.Remove(upgrade.Key);
                                            }
                                        }

                                        if (!wasSuccess) break;
                                    }

                                    if (pilotJson.HasField("vendor"))
                                    {
                                        JSONObject vendorData = pilotJson["vendor"];
                                        if (vendorData.HasField("Sandrem.FlyCasual"))
                                        {
                                            JSONObject myVendorData = vendorData["Sandrem.FlyCasual"];
                                            if (myVendorData.HasField("skin"))
                                            {
                                                newShip.Instance.ModelInfo.SkinName = myVendorData["skin"].str;
                                            }
                                        }
                                    }
                                    ShipFactory.SpawnShip(newShip);
                                    deploymentGroup.Add(newShipInstance);
                                    Roster.AddShipToLists(newShipInstance);
                                }
                            }
                        }
                        if (deploymentConfigJson.HasField("startingZone"))
                        {
                            if (deploymentConfigJson["startingZone"].str == "random")
                            {
                                startingZone = RandomStartZone(1, 6);
                            }
                            else
                            {
                                startingZone = Board.GetStartingZoneCampaign(deploymentConfigJson["startingZone"].str);
                            }
                        }
                        if (deploymentGroup.Count > 0)
                        {
                            DeploymentConfig deploymentConfig = new DeploymentConfig(deploymentGroup, startingZone);
                            deploymentConfigs.Add(deploymentConfig);
                        }
                    }                    
                }

                if(deploymentConfigs.Count>0)
                {
                    var subphase = Phases.StartTemporarySubPhaseNew<SetupCampaignShipSubPhase>(
                    "Setup",
                    delegate
                    {
                    }
                );

                    subphase.DeploymentConfigs = deploymentConfigs;

                    subphase.Start();
                }
            }
        }

        private void InstallElitePilotUpgrades(string upgradeClass, SquadListShip newShip, GenericShip newShipInstance, Dictionary<string, string> upgradesThatCannotBeInstalled)
        {
            JSONObject elitePilotUpgrades = LoadElitePilotUpgrades();

            if(elitePilotUpgrades.HasField("ships"))
            {
                JSONObject shipsJson = elitePilotUpgrades["ships"];
                foreach (JSONObject shipJson in shipsJson.list)
                {
                    if (shipJson.HasField("type") && shipJson["type"].str.Equals(newShip.Instance.ShipTypeCanonical))
                    {
                        if (shipJson.HasField("upgrades"))
                        {
                            JSONObject upgrades = shipJson["upgrades"];
                            int randomUpgrade = UnityEngine.Random.Range(0, upgrades.Count-1);
                            JSONObject upgrade = upgrades[randomUpgrade];
                            if (upgradeClass.Equals("basic"))
                            {
                                if (upgrade.HasField("basicUpgrades"))
                                {
                                    JSONObject basicUpgrade = upgrade["basicUpgrades"];
                                    InstallCampaignUpgrade(newShip, upgradesThatCannotBeInstalled, basicUpgrade);
                                }
                                newShipInstance.PilotInfo.Initiative = 1;
                            }
                            
                        }
                    }
                }
            }
        }

        private JSONObject LoadElitePilotUpgrades()
        {
            string directoryPath = Application.persistentDataPath + "/" + Edition.Current.Name + "/" + Edition.Current.PathToElitePilotUpgrades;
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + "/elitePilotUpgrades.json";
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "{\"name\":\"elitePilotUpgrades\",\"ships\":[{\"type\":\"tiesabomber\",\"upgrades\":[{\"basicUpgrades\":{\"missile\":[\"homingmissiles\"]},\"elite3Upgrades\":{\"torpedo\":[\"extraMunitions\"]}},{\"basicUpgrades\":{\"torpedo\":[\"advprotontorpedoes\"]},\"elite3Upgrades\":{\"torpedo\":[\"extraMunitions\"]}},{\"basicUpgrades\":{\"missile\":[\"clustermissiles\"]},\"elite3Upgrades\":{\"torpedo\":[\"extraMunitions\"]}},{\"basicUpgrades\":{\"missile\":[\"clustermissiles\"]},\"elite3Upgrades\":{\"torpedo\":[\"extraMunitions\"]}},{\"basicUpgrades\":{\"missile\":[\"ionmissiles\"]},\"elite3Upgrades\":{\"torpedo\":[\"extraMunitions\"]}},{\"basicUpgrades\":{\"torpedo\":[\"protontorpedoes\"]},\"elite3Upgrades\":{\"torpedo\":[\"extraMunitions\"]}}]}]}");
            }
            string content = File.ReadAllText(filePath);
            JSONObject elitePilotJson = new JSONObject(content);
            return elitePilotJson;
        }

        private static void InstallCampaignUpgrade(SquadListShip newShip, Dictionary<string, string> upgradesThatCannotBeInstalled, JSONObject upgradeJsons)
        {
            if (upgradeJsons.keys != null)
            {
                foreach (string upgradeKey in upgradeJsons.keys)
                {
                    JSONObject upgradeNames = upgradeJsons[upgradeKey];
                    foreach (JSONObject upgradeRecord in upgradeNames.list)
                    {
                        string upgradeName = upgradeRecord.str;
                        string upgradeType = upgradeKey;

                        UpgradeRecord newUpgradeRecord = SquadBuilder.Instance.Database.AllUpgrades.FirstOrDefault(n => n.UpgradeNameCanonical == upgradeName);
                        if (newUpgradeRecord == null)
                        {
                            Messages.ShowError("Cannot find upgrade: " + upgradeName);
                        }

                        bool upgradeInstalledSucessfully = newShip.InstallUpgrade(upgradeName, Edition.Current.XwsToUpgradeType(upgradeType));
                        if (!upgradeInstalledSucessfully && !upgradesThatCannotBeInstalled.ContainsKey(upgradeName)) upgradesThatCannotBeInstalled.Add(upgradeName, upgradeType);
                    }
                }
            }


            
        }

        private void setRandomBasicUpgrade(ref string upgradeName, ref string upgradeType)
        {
            switch (upgradeName)
            {
                case "tiesabomber":
                    int randomUpgrade = UnityEngine.Random.Range(0, 4);
                    switch (randomUpgrade)
                    {
                        case 0:
                            upgradeName = "homingmissiles";
                            upgradeType = "missile";
                            break;
                        case 1:
                            upgradeName = "advprotontorpedoes";
                            upgradeType = "torpedo";
                            break;
                        case 2:
                            upgradeName = "clustermissiles";
                            upgradeType = "missile";
                            break;
                        case 3:
                            upgradeName = "ionmissiles";
                            upgradeType = "missile";
                            break;
                        case 4:
                            upgradeName = "protontorpedoes";
                            upgradeType = "torpedo";
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void MidGameShipAddition()
        {
            LoadShipsForRound(Phases.RoundCounter);
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
        public static JSONObject campaignMission { get; set; }
        public static void LoadCampaign()
        {
            CampaignShipLoader currentCampaignShipLoader = new CampaignShipLoader();
            currentCampaignShipLoader.LoadCampaign();
        }
    }
}
