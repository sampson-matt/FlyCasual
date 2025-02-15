﻿using System.Collections;
using System.Collections.Generic;
using Movement;
using Actions;
using ActionsList;
using Arcs;
using Upgrade;
using UnityEngine;

namespace Ship.FirstEdition.KWing
{
    public class KWing : GenericShip
    {
        public KWing() : base()
        {
            ShipInfo = new ShipCardInfo
            (
                "K-wing",
                BaseSize.Small,
                Faction.Rebel,
                new ShipArcsInfo(
                    new ShipArcInfo(ArcType.Front, 2),
                    new ShipArcInfo(ArcType.TurretPrimaryWeapon, 2)
                ),
                1, 5, 4,
                new ShipActionsInfo(
                    new ActionInfo(typeof(FocusAction)),
                    new ActionInfo(typeof(TargetLockAction)),
                    new ActionInfo(typeof(SlamAction))
                ),
                new ShipUpgradesInfo(
                    UpgradeType.Modification,
                    UpgradeType.Turret,
                    UpgradeType.Torpedo,
                    UpgradeType.Torpedo,
                    UpgradeType.Missile,
                    UpgradeType.Crew,
                    UpgradeType.Device,
                    UpgradeType.Device
                )
            );

            IconicPilots = new Dictionary<Faction, System.Type> {
                { Faction.Rebel, typeof(MirandaDoni) }
            };

            ModelInfo = new ShipModelInfo(
                "K-wing",
                "Red",
                new Vector3(-3.55f, 7.3f, 5.55f),
                3f
            );

            DialInfo = new ShipDialInfo(
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Easy),

                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Normal),

                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Normal),
                new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal)
            );

            SoundInfo = new ShipSoundInfo(
                new List<string>()
                {
                    "YWing-Fly1",
                    "YWing-Fly2"
                },
                "XWing-Laser", 2
            );

            ShipIconLetter = 'k';

            HotacManeuverTable = new AI.KWingTable();
        }
    }
}
