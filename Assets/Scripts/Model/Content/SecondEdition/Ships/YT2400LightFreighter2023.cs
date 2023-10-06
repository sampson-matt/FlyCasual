using System.Collections;
using System.Collections.Generic;
using Actions;
using ActionsList;
using Arcs;
using Upgrade;

namespace Ship.SecondEdition.YT2400LightFreighter2023
{
    public class YT2400LightFreighter2023 : FirstEdition.YT2400.YT2400
    {
        public YT2400LightFreighter2023() : base()
        {
            ShipInfo.ShipName = "YT-2400 Light Freighter 2023";

            ShipInfo.ArcInfo = new ShipArcsInfo(ArcType.DoubleTurret, 3);

            ShipInfo.Hull = 6;
            ShipInfo.Shields = 4;

            ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Cannon);
            ShipInfo.UpgradeIcons.Upgrades.Add(UpgradeType.Illicit);
            ShipInfo.UpgradeIcons.Upgrades.Add(UpgradeType.Illicit);

            ShipInfo.ActionIcons.RemoveActions(typeof(BarrelRollAction));
            ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BarrelRollAction), ActionColor.Red));
            ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(RotateArcAction)));

            ShipAbilities.Add(new Abilities.SecondEdition.SensorBlindspot2023());

            IconicPilots[Faction.Rebel] = typeof(DashRendar2023);

            ManeuversImageUrl = "https://vignette.wikia.nocookie.net/xwing-miniatures-second-edition/images/1/11/Maneuver_yt-2400.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class SensorBlindspot2023 : GenericAbility
    {
        public override string Name { get { return "Sensor Blindspot"; } }

        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += CheckSensorBlindspot;
            HostShip.AfterGotNumberOfDefenceDice += CheckSensorBlindspotDefense;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice -= CheckSensorBlindspot;
        }

        private void CheckSensorBlindspot(ref int count)
        {
            if (Combat.ChosenWeapon.WeaponType == Ship.WeaponTypes.PrimaryWeapon && Combat.ShotInfo.Range < 2) count -= 2;
        }
        private void CheckSensorBlindspotDefense(ref int count)
        {
            if (Combat.ShotInfo.Range < 2) count -= 1;
        }
    }
}
