using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class CommanderAlozenPilotAbility : GenericUpgrade
    {
        public CommanderAlozenPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Commander Alozen Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.CommanderAlozenAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/commanderalozen.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CommanderAlozenAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCombatActivation += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCombatActivation -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatActivation, StarExecutionOfAbility);
        }

        //private bool AbilityConditionsAreMet()
        //{
        //    bool result = true;

        //    foreach (GenericShip enemyShip in HostShip.Owner.EnemyShips.Values)
        //    {
        //        if (enemyShip.SectorsInfo.IsShipInSector(HostShip, Arcs.ArcType.Front)) return false;
        //    }

        //    return result;
        //}

        private void StarExecutionOfAbility(object sender, EventArgs e)
        {
            if (HasTargersForAbility())
            {
                SelectTargetForAbility
                (
                    TryAcquireLock,
                    FilterTargets,
                    GetAiPriority,
                    HostShip.Owner.PlayerNo,
                    name: HostShip.PilotInfo.PilotName,
                    description: "You may acquire a lock on an enemy ship at Range 1",
                    imageSource: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private bool HasTargersForAbility()
        {
            foreach (GenericShip enemyShip in HostShip.Owner.EnemyShips.Values)
            {
                if (FilterTargets(enemyShip)) return true;
            }

            return false;
        }

        private void TryAcquireLock()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();

            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: Acquires lock on {TargetShip.PilotInfo.PilotName}");

            ActionsHolder.AcquireTargetLock(HostShip, TargetShip, Triggers.FinishTrigger, Triggers.FinishTrigger);
        }

        private bool FilterTargets(GenericShip ship)
        {
            return HostShip.GetRangeToShip(ship) == 1;
        }

        private int GetAiPriority(GenericShip ship)
        {
            int priority = ship.PilotInfo.Cost;

            bool canAttackShip = false;
            bool isInRangeButRequiresLock = false;

            foreach (IShipWeapon weapon in HostShip.GetAllWeapons())
            {
                ShotInfo shotInfo = new ShotInfo(HostShip, ship, HostShip.PrimaryWeapons);

                if (shotInfo.IsShotAvailable) canAttackShip = true;

                if (shotInfo.Range >= weapon.WeaponInfo.MinRange
                    && shotInfo.Range <= weapon.WeaponInfo.MaxRange
                    && weapon.WeaponInfo.RequiresTokens.Contains(typeof(Tokens.BlueTargetLockToken))
                )
                {
                    isInRangeButRequiresLock = true;
                }
            }

            if (canAttackShip) priority += 100;
            if (isInRangeButRequiresLock) priority += 200;

            return priority;
        }
    }
}