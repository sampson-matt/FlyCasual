using Arcs;
using BoardTools;
using Content;
using Ship;
using SubPhases;
using Upgrade;
using Tokens;
using System.Linq;
using System.Collections.Generic;
using Obstacles;

namespace UpgradesList.SecondEdition
{
    public class MandalorianOptics : GenericUpgrade
    {
        public MandalorianOptics()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "MandalorianOptics",
                UpgradeType.Modification,
                cost: 5,
                restriction: new TagRestriction(Tags.Mandalorian),
                abilityType: typeof(Abilities.SecondEdition.MandalorianOpticsAbility),
                charges: 2
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MandalorianOpticsAbility : GenericAbility
    {
        List<GenericObstacle> IgnoredObstacles = new List<GenericObstacle>();

        public override void ActivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation += CheckForAbility;
            HostShip.OnSystemsAbilityActivation += RegisterAbility;
            HostShip.OnDefenceStartAsAttacker += CheckObstacleObstructionAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation -= CheckForAbility;
            HostShip.OnSystemsAbilityActivation -= RegisterAbility;
            HostShip.OnDefenceStartAsAttacker -= CheckObstacleObstructionAbility;
        }

        private void CheckObstacleObstructionAbility()
        {
            if (Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon && ActionsHolder.HasTargetLockOn(Combat.Attacker, Combat.Defender))
            {
                Messages.ShowInfo("Mandalorian Optics: if you have a lock on the defender, ignore obstacles beyond range 0 obstructing the attack");
                IgnoredObstacles.AddRange(Combat.ShotInfo.ObstructedByObstacles.Where(o => !HostShip.ObstaclesLanded.Contains(o)));
                HostShip.IgnoreObstaclesList.AddRange(IgnoredObstacles);
                Phases.Events.OnRoundEnd += ClearAbility;
            }            
        }

        private void ClearAbility()
        {
            Phases.Events.OnRoundEnd -= ClearAbility;

            HostShip.IgnoreObstaclesList.RemoveAll(n => IgnoredObstacles.Contains(n));
            IgnoredObstacles.Clear();
        }

        private void CheckForAbility(GenericShip ship, ref bool flag)
        {
            if (HostUpgrade.State.Charges >= 1 && Board.GetShipsInArcAtRange(HostShip, ArcType.Front, new UnityEngine.Vector2(0,3), Team.Type.Any).Count >0) flag = true;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostUpgrade.State.Charges >= 1 && Board.GetShipsInArcAtRange(HostShip, ArcType.Front, new UnityEngine.Vector2(0, 3), Team.Type.Any).Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToUseMandalorianOpticsAbility);
            }
        }

        private void AskToUseMandalorianOpticsAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                    GrantFreeTargetLock,
                    FilterAbilityTargets,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostUpgrade.State.Name,
                    "You may spend 1 Charge to acquire a lock on an object in your front arc",
                    HostUpgrade
                );
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int priority = 0;

            if (!HostShip.Tokens.HasToken(typeof(BlueTargetLockToken))) priority += 50;
            ShotInfo shotInfo = new ShotInfo(HostShip, ship, ship.PrimaryWeapons);
            if (shotInfo.IsShotAvailable) priority += 40;

            priority += ship.PilotInfo.Cost;

            return priority;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            return HostShip.SectorsInfo.IsShipInSector(ship, ArcType.Front);
        }

        private void GrantFreeTargetLock()
        {
            if (TargetShip != null)
            {
                ActionsHolder.AcquireTargetLock(HostShip, TargetShip, SelectShipSubPhase.FinishSelection, SelectShipSubPhase.FinishSelection);
                HostUpgrade.State.SpendCharge();
            }
            else
            {
                SelectShipSubPhase.FinishSelection();
            }
        }
    }
}