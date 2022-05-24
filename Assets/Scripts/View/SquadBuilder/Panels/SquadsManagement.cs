using Editions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SquadBuilderNS
{
    public class SquadsManagement
    {
        private SquadBuilderView View;

        public SquadsManagement(SquadBuilderView squadBuilderView)
        {
            View = squadBuilderView;
        }

        public void PrepareSaveSquadronPanel()
        {
            GameObject.Find("UI/Panels/SaveSquadronPanel/Panel/Name/InputField").GetComponent<InputField>().text = Global.SquadBuilder.CurrentSquad.Name;
        }

        public void OpenImportExportPanel(bool isImport)
        {
            MainMenu.CurrentMainMenu.ChangePanel("ImportExportPanel");

            GameObject.Find("UI/Panels/ImportExportPanel/Content/InputField").GetComponent<InputField>().text = (isImport) ? "" : Global.SquadBuilder.CurrentSquad.GetSquadInJson().ToString();
            GameObject.Find("UI/Panels/ImportExportPanel/BottomPanel/ImportButton").SetActive(isImport);
        }

        private List<JSONObject> GetSavedSquadsJsons()
        {
            List<JSONObject> savedSquadsJsons = new List<JSONObject>();

            string directoryPath = Application.persistentDataPath + "/" + Edition.Current.Name + "/" + Edition.Current.PathToSavedSquadrons;
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                string content = File.ReadAllText(filePath);
                JSONObject squadJson = new JSONObject(content);

                if (Global.SquadBuilder.CurrentSquad.SquadFaction == Faction.None || Edition.Current.XwsToFaction(squadJson["faction"].str) == Global.SquadBuilder.CurrentSquad.SquadFaction)
                {
                    squadJson.AddField("filename", new FileInfo(filePath).Name);
                    savedSquadsJsons.Add(squadJson);
                }
            }

            return savedSquadsJsons;
        }

        public void ShowListOfSavedSquadrons(List<JSONObject> squadsJsonsList)
        {
            SetNoSavedSquadronsMessage(squadsJsonsList.Count == 0);

            float FREE_SPACE = 25;

            Transform contentTransform = GameObject.Find("UI/Panels/BrowseSavedSquadsPanel/Scroll View/Viewport/Content").transform;

            SquadBuilderView.DestroyChildren(contentTransform);

            GameObject prefab = (GameObject)Resources.Load("Prefabs/SquadBuilder/SavedSquadronPanel", typeof(GameObject));

            //RectTransform contentRectTransform = contentTransform.GetComponent<RectTransform>();
            //Vector3 currentPosition = new Vector3(0, -FREE_SPACE, contentTransform.localPosition.z);

            foreach (var squadList in squadsJsonsList)
            {
                GameObject SquadListRecord;

                SquadListRecord = MonoBehaviour.Instantiate(prefab, contentTransform);

                SquadListRecord.transform.Find("Name").GetComponent<Text>().text = squadList["name"].str;

                Text descriptionText = SquadListRecord.transform.Find("Description").GetComponent<Text>();
                RectTransform descriptionRectTransform = SquadListRecord.transform.Find("Description").GetComponent<RectTransform>();
                if (squadList.HasField("description"))
                {
                    descriptionText.text = squadList["description"].str.Replace("\\\"", "\"");
                }
                else
                {
                    descriptionText.text = "No description";
                }

                float descriptionPreferredHeight = descriptionText.preferredHeight;
                descriptionRectTransform.sizeDelta = new Vector2(descriptionRectTransform.sizeDelta.x, descriptionPreferredHeight);

                SquadListRecord.transform.Find("PointsValue").GetComponent<Text>().text = squadList["points"].i.ToString();

                SquadListRecord.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    SquadListRecord.GetComponent<RectTransform>().sizeDelta.x,
                    15 + 70 + 10 + descriptionPreferredHeight + 10 + 55 + 10
                );

                SquadListRecord.name = squadList["filename"].str;

                SquadListRecord.transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(delegate { DeleteSavedSquadAndRefresh(SquadListRecord.name); });
                SquadListRecord.transform.Find("LoadButton").GetComponent<Button>().onClick.AddListener(delegate { LoadSavedSquadAndReturn(GetSavedSquadJson(SquadListRecord.name)); });
            }

            SquadBuilderView.OrganizePanels(contentTransform, FREE_SPACE);
        }

        private JSONObject GetSavedSquadJson(string fileName)
        {
            JSONObject squadJson = null;

            string directoryPath = Application.persistentDataPath + "/" + Edition.Current.Name + "/" + Edition.Current.PathToSavedSquadrons;
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + "/" + fileName;

            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                squadJson = new JSONObject(content);
            }

            return squadJson;
        }

        private JSONObject GetCampaignMissionJson(string fileName)
        {
            JSONObject squadJson = null;

            string directoryPath = Application.persistentDataPath + "/" + Edition.Current.Name + "/" + Edition.Current.PathToCampaignSetup;
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + "/" + fileName;

            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                squadJson = new JSONObject(content);
            }

            return squadJson;
        }

        private void SetNoSavedSquadronsMessage(bool isActive)
        {
            GameObject.Find("UI/Panels/BrowseSavedSquadsPanel/NoSavedSquads").SetActive(isActive);
        }

        private void SetNoCampaignMissionMessage(bool isActive)
        {
            GameObject.Find("UI/Panels/BrowseCampaignMissionsPanel/NoSavedSquads").SetActive(isActive);
        }

        private void DeleteSavedSquadFile(string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + Edition.Current.Name + "/SavedSquadrons/" + fileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void DeleteCampaignMissionFile(string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + Edition.Current.Name + "/CampaignSetup/" + fileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        // IMPORT / EXPORT

        public void BrowseSavedSquads()
        {
            List<JSONObject> sortedSavedSquadsJsons = GetSavedSquadsJsons();
            foreach (string autosaveName in new List<string>() { "Autosave", "Autosave (Player 2)", "Autosave (Player 1)" })
            {
                SetAutosavesOnTop(sortedSavedSquadsJsons, autosaveName);
            }
            ShowListOfSavedSquadrons(sortedSavedSquadsJsons);
        }

        public void BrowseCampaignMissions()
        {
            List<JSONObject> campaignMissionJsons = GetCampaignMissionJsons();
            ShowCampaignMissions(campaignMissionJsons);
        }

        public void ShowCampaignMissions(List<JSONObject> campaignMissionJsonsList)
        {
            SetNoCampaignMissionMessage(campaignMissionJsonsList.Count == 0);

            float FREE_SPACE = 25;

            Transform contentTransform = GameObject.Find("UI/Panels/BrowseCampaignMissionsPanel/Scroll View/Viewport/Content").transform;

            SquadBuilderView.DestroyChildren(contentTransform);

            GameObject prefab = (GameObject)Resources.Load("Prefabs/SquadBuilder/CampaignMissionPanel", typeof(GameObject));

            //RectTransform contentRectTransform = contentTransform.GetComponent<RectTransform>();
            //Vector3 currentPosition = new Vector3(0, -FREE_SPACE, contentTransform.localPosition.z);

            foreach (var campaignMission in campaignMissionJsonsList)
            {
                GameObject CampaignMissionRecord;

                CampaignMissionRecord = MonoBehaviour.Instantiate(prefab, contentTransform);

                CampaignMissionRecord.transform.Find("Name").GetComponent<Text>().text = campaignMission["name"].str;

                Text descriptionText = CampaignMissionRecord.transform.Find("Description").GetComponent<Text>();
                RectTransform descriptionRectTransform = CampaignMissionRecord.transform.Find("Description").GetComponent<RectTransform>();
                if (campaignMission.HasField("description"))
                {
                    descriptionText.text = campaignMission["description"].str.Replace("\\\"", "\"");
                }
                else
                {
                    descriptionText.text = "No description";
                }

                float descriptionPreferredHeight = descriptionText.preferredHeight;
                descriptionRectTransform.sizeDelta = new Vector2(descriptionRectTransform.sizeDelta.x, descriptionPreferredHeight);

                //SquadListRecord.transform.Find("PointsValue").GetComponent<Text>().text = squadList["points"].i.ToString();

                CampaignMissionRecord.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    CampaignMissionRecord.GetComponent<RectTransform>().sizeDelta.x,
                    15 + 70 + 10 + descriptionPreferredHeight + 10 + 55 + 10
                );

                CampaignMissionRecord.name = campaignMission["filename"].str;

                CampaignMissionRecord.transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(delegate { DeleteCampaignMissionAndRefresh(CampaignMissionRecord.name); });
                CampaignMissionRecord.transform.Find("LoadButton").GetComponent<Button>().onClick.AddListener(delegate { LoadCampaignMissionAndReturn(GetCampaignMissionJson(CampaignMissionRecord.name)); });
            }

            SquadBuilderView.OrganizePanels(contentTransform, FREE_SPACE);
        }

        private List<JSONObject> GetCampaignMissionJsons()
        {
            List<JSONObject> campaignMissionJsons = new List<JSONObject>();

            string directoryPath = Application.persistentDataPath + "/" + Edition.Current.Name + "/" + Edition.Current.PathToCampaignSetup;
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            
            File.WriteAllText(directoryPath + "/Local Trouble.json", "{\"description\":\"At least one Rebel ship must survive and remain in play at the end of Turn 10.\",\"name\":\"Local Trouble\",\"obstacles\":[{\"type\":\"asteroids\",\"minBoardEdgeDistanceX\":\"2\",\"minBoardEdgeDistanceY\":\"2\",\"minSeparationDistance\":\"1\",\"count\":\"6\"}],\"deploymentConfigs\":[{\"faction\":\"galacticempire\",\"startingZone\":\"StartingZoneCampaign3\",\"deploymentRound\":\"0\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"2\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"4\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"6\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]},{\"faction\":\"galacticempire\",\"startingZone\":\"StartingZoneCampaign4\",\"deploymentRound\":\"0\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"3\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"5\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]},{\"faction\":\"galacticempire\",\"startingZone\":\"random\",\"deploymentRound\":\"4\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"alphasquadronpilot\",\"ship\":\"tieininterceptor\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"4\",\"id\":\"alphasquadronpilot\",\"ship\":\"tieininterceptor\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]},{\"faction\":\"galacticempire\",\"startingZone\":\"random\",\"deploymentRound\":\"7\",\"pilots\":[{\"squadSize\":\"2\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"3\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"5\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"6\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]}],\"points\":76,\"version\":\"0.3.0\"}");
            File.WriteAllText(directoryPath + "/Tread Softly.json", "{\"description\":\"Destroy minefield tokens. The mission is a success if there are fewer minefields remaining than the total number of players.\",\"name\":\"Tread Softly\",\"obstacles\":[{\"type\":\"mineField\",\"minBoardEdgeDistanceX\":\"1\",\"minBoardEdgeDistanceY\":\"1\",\"minSeparationDistance\":\"1\",\"maxSeparationDistance\":\"1\",\"squadCount\":\"3\"}],\"deploymentConfigs\":[{\"faction\":\"galacticempire\",\"startingZone\":\"StartingZoneCampaign3\",\"deploymentRound\":\"0\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"hotacbomberpilot\",\"ship\":\"tiesabomber\",\"upgrades\":{\"device\":[\"minefield\"]},\"randomUpgrades\":\"basic\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"2\",\"averageInitiave\":\"5\",\"id\":\"hotacbomberpilot\",\"ship\":\"tiesabomber\",\"upgrades\":{\"device\":[\"minefield\"]},\"randomUpgrades\":\"basic\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"3\",\"id\":\"hotacbomberpilot\",\"ship\":\"tiesabomber\",\"upgrades\":{\"device\":[\"minefield\"]},\"randomUpgrades\":\"basic\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"5\",\"id\":\"hotacbomberpilot\",\"ship\":\"tiesabomber\",\"upgrades\":{\"device\":[\"minefield\"]},\"randomUpgrades\":\"basic\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]},{\"faction\":\"galacticempire\",\"startingZone\":\"StartingZoneCampaign5\",\"deploymentRound\":\"0\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"2\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"3\",\"averageInitiave\":\"4\",\"replace\":\"tielnfighter\",\"id\":\"hotacinterceptorpilot\",\"ship\":\"tieininterceptor\",\"upgrades\":{},\"randomUpgrades\":\"basic\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"4\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"5\",\"averageInitiave\":\"3\",\"replace\":\"tielnfighter\",\"id\":\"hotacinterceptorpilot\",\"ship\":\"tieininterceptor\",\"upgrades\":{},\"randomUpgrades\":\"basic\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"4\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]},{\"faction\":\"galacticempire\",\"startingZone\":\"random\",\"deploymentRound\":\"5\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"hotacbomberpilot\",\"ship\":\"tiesabomber\",\"upgrades\":{},\"randomUpgrades\":\"elite\",\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]},{\"faction\":\"galacticempire\",\"startingZone\":\"random\",\"deploymentRound\":\"5\",\"pilots\":[{\"squadSize\":\"1\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"2\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"3\",\"averageInitiave\":\"5\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"4\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"3\",\"averageInitiave\":\"4\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"squadSize\":\"4\",\"id\":\"academypilot\",\"ship\":\"tielnfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}]}],\"points\":76,\"version\":\"0.3.0\"}");

            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                string content = File.ReadAllText(filePath);
                JSONObject missionJson = new JSONObject(content);
                missionJson.AddField("filename", new FileInfo(filePath).Name);
                campaignMissionJsons.Add(missionJson);
            }

            return campaignMissionJsons;
        }

        private void SetAutosavesOnTop(List<JSONObject> jsonList, string autosaveName)
        {
            try
            {
                JSONObject autosaveJson = jsonList.Find(n => n["name"].str == autosaveName);
                if (autosaveJson != null)
                {
                    jsonList.Remove(autosaveJson);
                    jsonList.Insert(0, autosaveJson);
                }
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        private void DeleteSavedSquadAndRefresh(string fileName)
        {
            if (Edition.Current.IsSquadBuilderLocked)
            {
                Messages.ShowError("This part of squad builder is disabled");
                return;
            }

            DeleteSavedSquadFile(fileName);
            BrowseSavedSquads();
        }

        private void DeleteCampaignMissionAndRefresh(string fileName)
        {
            if (Edition.Current.IsSquadBuilderLocked)
            {
                Messages.ShowError("This part of squad builder is disabled");
                return;
            }

            DeleteCampaignMissionFile(fileName);
            BrowseCampaignMissions();
        }

        public void LoadSavedSquadAndReturn(JSONObject squadJson)
        {
            Global.SquadBuilder.SquadLists[Global.SquadBuilder.CurrentPlayer].SetPlayerSquadFromImportedJson(squadJson);
            View.ReturnToSquadBuilder();
        }

        public void LoadCampaignMissionAndReturn(JSONObject campaignMissionJson)
        {
            CampaignLoader.campaignMission = campaignMissionJson;
            MainMenu.CurrentMainMenu.ChangePanel("SelectFactionPanel");
        }
    }
}
