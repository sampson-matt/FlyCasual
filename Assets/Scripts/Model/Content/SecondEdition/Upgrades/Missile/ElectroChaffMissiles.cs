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
    public class ElectroChaffMissiles : GenericTimedBombSE
    {
        private ElectroChaffCloud chaffCloud;
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
                subType: UpgradeSubType.Bomb
            );
            detonationRange = 2;
            bombPrefabPath = "Prefabs/Bombs/ElectroChaffCloud";
        }

        public override List<ManeuverTemplate> GetDefaultDropTemplates()
        {
            return new List<ManeuverTemplate>();
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

        public override void ActivateBombs(List<GenericDeviceGameObject> bombObjects, Action callBack)
        {
            //base.ActivateBombs(bombObjects, callBack);
            CurrentBombObjects.AddRange(bombObjects);
            HostShip.IsBombAlreadyDropped = true;
            BombsManager.RegisterBombs(bombObjects, this);
            PayDropCost(callBack);

            Phases.Events.OnEndPhaseStart_Triggers += base.PlanTimedDetonation;

            foreach (var bombObject in bombObjects)
            {
                chaffCloud = new ElectroChaffCloud("Electro-Chaff Cloud", "electro-chaffcloud");
                chaffCloud.Spawn("Electro-Chaff Cloud " + HostShip.ShipId, Board.GetBoard());
                ObstaclesManager.AddObstacle(chaffCloud);

                chaffCloud.ObstacleGO.transform.position = bombObject.transform.position;
                chaffCloud.ObstacleGO.transform.eulerAngles = bombObject.transform.eulerAngles;
                chaffCloud.IsPlaced = true;
                bombObject.Fuses++;
            }
        }

        protected override void Detonate()
        {
            ObstaclesManager.DestroyObstacle(chaffCloud);
            Phases.Events.OnEndPhaseStart_Triggers -= base.PlanTimedDetonation;
            base.Detonate();
        }
    }
}