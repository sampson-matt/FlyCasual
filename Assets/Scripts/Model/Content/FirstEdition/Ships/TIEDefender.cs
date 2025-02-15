﻿using Content;
using System.Collections.Generic;
using Movement;
using ActionsList;
using Actions;
using Arcs;
using Upgrade;
using UnityEngine;

namespace Ship
{
    namespace FirstEdition.TIEDefender
    {
        public class TIEDefender : GenericShip
        {
            public TIEDefender() : base()
            {
                ShipInfo = new ShipCardInfo
                (
                    "TIE Defender",
                    BaseSize.Small,
                    Faction.Imperial,
                    new ShipArcsInfo(ArcType.Front, 3), 3, 3, 3,
                    new ShipActionsInfo(
                        new ActionInfo(typeof(FocusAction)),
                        new ActionInfo(typeof(BarrelRollAction)),
                        new ActionInfo(typeof(TargetLockAction))
                    ),
                    new ShipUpgradesInfo(
                        UpgradeType.Modification,
                        UpgradeType.Cannon,
                        UpgradeType.Missile
                    ),
                    tags: new List<Tags>
                    {
                        Tags.Tie
                    }
                );

                IconicPilots = new Dictionary<Faction, System.Type> {
                    { Faction.Imperial, typeof(ColonelVessery) }
                };

                ModelInfo = new ShipModelInfo(
                    "TIE Defender",
                    "Gray",
                    new Vector3(-3.7f, 7.9f, 5.55f),
                    2f
                );

                DialInfo = new ShipDialInfo(
                    new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Complex),
                    new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Easy),
                    new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Easy),
                    new ManeuverInfo(ManeuverSpeed.Speed1, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Complex),

                    new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Complex),
                    new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                    new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                    new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),
                    new ManeuverInfo(ManeuverSpeed.Speed2, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Complex),

                    new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Turn, MovementComplexity.Normal),
                    new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Left, ManeuverBearing.Bank, MovementComplexity.Normal),
                    new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                    new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Bank, MovementComplexity.Normal),
                    new ManeuverInfo(ManeuverSpeed.Speed3, ManeuverDirection.Right, ManeuverBearing.Turn, MovementComplexity.Normal),

                    new ManeuverInfo(ManeuverSpeed.Speed4, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy),
                    new ManeuverInfo(ManeuverSpeed.Speed4, ManeuverDirection.Forward, ManeuverBearing.KoiogranTurn, MovementComplexity.Normal),

                    new ManeuverInfo(ManeuverSpeed.Speed5, ManeuverDirection.Forward, ManeuverBearing.Straight, MovementComplexity.Easy)
                );

                SoundInfo = new ShipSoundInfo(
                    new List<string>()
                    {
                        "TIE-Fly1",
                        "TIE-Fly2",
                        "TIE-Fly3",
                        "TIE-Fly4",
                        "TIE-Fly5",
                        "TIE-Fly6",
                        "TIE-Fly7"
                    },
                    "TIE-Fire", 3
                );

                ShipIconLetter = 'D';

                HotacManeuverTable = new AI.TIEDefenderTable();
            }
        }
    }
}
