using System.Collections.Generic;
using Upgrade;
using BoardTools;
using Movement;
using Ship;
using System;
using SubPhases;
using Obstacles;
using System.Linq;
using UnityEngine;
using Bombs;

namespace UpgradesList.SecondEdition
{
    public class ElectroChaffMissiles : GenericUpgrade
    {
        public ElectroChaffMissiles() : base()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "Electro-Chaff Missiles",
                types: new List<UpgradeType>()
                {
                    UpgradeType.Missile,
                    UpgradeType.Device
                },
                cost: 4,
                limited: 2,
                charges: 1,
                cannotBeRecharged: true,
                subType: UpgradeSubType.Bomb,
                abilityType: typeof(Abilities.SecondEdition.ElectroChaffMissilesAbility)
            );
        }

        public override List<ManeuverTemplate> GetDefaultLaunchTemplates()
        {
           return new List<ManeuverTemplate>()
            {
                new ManeuverTemplate(ManeuverBearing.Straight, ManeuverDirection.Forward, ManeuverSpeed.Speed4),
                new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Left, ManeuverSpeed.Speed3),
                new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Right, ManeuverSpeed.Speed3)
            };
        }
    }
}

namespace Abilities.SecondEdition
{   
    public class ElectroChaffMissilesAbility : GenericAbility
    {
        protected DeviceObjectInfoPanel infoPanel;
        private ElectroChaffCloud chaffCloud;

        public override void ActivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation += CheckAbility;
            HostShip.OnSystemsAbilityActivation += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation -= CheckAbility;
            HostShip.OnSystemsAbilityActivation -= RegisterAbility;
        }

        private void CheckAbility(GenericShip ship, ref bool flag)
        {
            if (HostUpgrade.State.Charges > 0) 
            {
                flag = true;
            }
              
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToLaunch);
            }
        }

        private void AskToLaunch(object sender, EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);

            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                NeverUseByDefault,
                StartSelectTemplateDecision,
                descriptionLong: "Do you want to launch 1 Electro-Chaff Cloud?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void StartSelectTemplateDecision(object sender, EventArgs e)
        {
            SelectBombLaunchTemplateDecisionSubPhase selectBoostTemplateDecisionSubPhase = (SelectBombLaunchTemplateDecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                "Select template to launch the Electro-Chaff Cloud",
                typeof(SelectBombLaunchTemplateDecisionSubPhase),
                Triggers.FinishTrigger
            );

            selectBoostTemplateDecisionSubPhase.ShowSkipButton = false;

            List<ManeuverTemplate> AvailableBombLaunchTemplates = HostShip.GetAvailableDeviceLaunchTemplates(HostUpgrade);

            foreach (var dropTemplate in AvailableBombLaunchTemplates)
            {
                selectBoostTemplateDecisionSubPhase.AddDecision(
                    dropTemplate.Name,
                    delegate { LaunchCloud(dropTemplate); },
                    isCentered: (dropTemplate.Direction == Movement.ManeuverDirection.Forward)
                );
            }

            selectBoostTemplateDecisionSubPhase.DescriptionShort = "Select template to launch the bomb";

            selectBoostTemplateDecisionSubPhase.DefaultDecisionName = selectBoostTemplateDecisionSubPhase.GetDecisions().First().Name;

            selectBoostTemplateDecisionSubPhase.RequiredPlayer = Selection.ThisShip.Owner.PlayerNo;

            selectBoostTemplateDecisionSubPhase.Start();
        }

        private class SelectBombLaunchTemplateDecisionSubPhase : DecisionSubPhase { }

        private void LaunchCloud(ManeuverTemplate dropTemplate)
        {
            Action Callback = Phases.CurrentSubPhase.CallBack;            

            HostUpgrade.State.SpendCharge();

            dropTemplate.ApplyTemplate(HostShip, HostShip.GetPosition(), Direction.Top);

            chaffCloud = new ElectroChaffCloud("Electro-Chaff Cloud", "electro-chaffcloud");
            chaffCloud.Spawn("Electro-Chaff Cloud " + HostShip.ShipId, Board.GetBoard());
            ObstaclesManager.AddObstacle(chaffCloud);

            chaffCloud.ObstacleGO.transform.position = dropTemplate.GetFinalPosition();
            chaffCloud.ObstacleGO.transform.eulerAngles = dropTemplate.GetFinalAngles();
            chaffCloud.IsPlaced = true;

            chaffCloud.Fuses = 1;

            var infoPanelPrefab = Resources.Load<DeviceObjectInfoPanel>("Prefabs/Bombs/Helpers/DeviceInfoPanel");
            infoPanel = UnityEngine.Object.Instantiate(infoPanelPrefab, chaffCloud.ObstacleGO.transform);

            Phases.Events.OnActivationPhaseEnd_Triggers += PlanTimedDetonation;

            infoPanel.setParentObstacle(chaffCloud);

            GameManagerScript.Wait(
                1,
                delegate
                {
                    dropTemplate.DestroyTemplate();
                    DecisionSubPhase.ConfirmDecisionNoCallback();
                    Callback();
                }
            );

        }

        private void PlanTimedDetonation()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = "Removal of Electro-Chaff Cloud",
                TriggerType = TriggerTypes.OnActivationPhaseEnd,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = TryDetonate
            });
            
            
        }

        private void TryDetonate(object sender, EventArgs e)
        {
            Triggers.FinishTrigger();
            if (chaffCloud.IsFused)
            {
                chaffCloud.Fuses--;
            }
            else
            {
                Phases.Events.OnActivationPhaseEnd_Triggers -= PlanTimedDetonation;
                ObstaclesManager.DestroyObstacle(chaffCloud);
            }
        }
    }
}