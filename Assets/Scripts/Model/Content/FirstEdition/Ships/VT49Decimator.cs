﻿using System.Collections;
using System.Collections.Generic;
using Movement;
using Actions;
using ActionsList;
using Arcs;
using Upgrade;
using UnityEngine;

namespace Ship.FirstEdition.VT49Decimator
{
    public class VT49Decimator : GenericShip
    {
        public VT49Decimator() : base()
        {
            ShipInfo = new ShipCardInfo
            (
                "VT-49 Decimator",
                BaseSize.Large,
                Faction.Imperial,
                new ShipArcsInfo(
                    new ShipArcInfo(ArcType.Front, 3),
                    new ShipArcInfo(ArcType.TurretPrimaryWeapon, 3)
                ),
                0, 12, 4,
                new ShipActionsInfo(
                    new ActionInfo(typeof(FocusAction)),
                    new ActionInfo(typeof(TargetLockAction))
                ),
                new ShipUpgradesInfo(
                    UpgradeType.Torpedo,
                    UpgradeType.Crew,
                    UpgradeType.Crew,
                    UpgradeType.Crew,
                    UpgradeType.Device,
                    UpgradeType.Title,
                    UpgradeType.Modification
                )
            );

            IconicPilots = new Dictionary<Faction, System.Type> {
                { Faction.Imperial, typeof(PatrolLeader) }
            };

            ModelInfo = new ShipModelInfo(
                "VT-49 Decimator",
                "Gray",
                new Vector3(-4f, 8.15f, 5.55f),
                3.5f
            );

            DialInfo = new ShipDialInfo(
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),

                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Normal),

                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Normal),

                new ManeuverInfo(ManeuverSpeed.Speed4, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal)
            );

            SoundInfo = new ShipSoundInfo(
                new List<string>()
                {
                    "Slave1-Fly1",
                    "Slave1-Fly2"
                },
                "Slave1-Fire", 3
            );

            ShipIconLetter = 'D';

            HotacManeuverTable = new AI.VT49DecimatorTable();
        }
    }
}
