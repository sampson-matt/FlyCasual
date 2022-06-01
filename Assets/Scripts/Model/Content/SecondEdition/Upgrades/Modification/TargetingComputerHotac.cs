using Actions;
using ActionsList;
using Upgrade;
using Ship;
using System;
using SubPhases;
using BoardTools;

namespace UpgradesList.SecondEdition
{
    public class TargetingComputerHotac : GenericUpgrade
    {
        public TargetingComputerHotac() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Targeting Computer Hotac",
                UpgradeType.Modification,
                cost: 3,
                addAction: new ActionInfo(typeof(TargetLockAction)),
                abilityType: typeof(Abilities.SecondEdition.TargetingComputerHotacAbility)
            );

            ImageUrl = "https://images-cdn.fantasyflightgames.com/filer_public/0b/d7/0bd7d42f-4401-4f58-9f9e-a5856e6c94f1/swz47_upgrade-targeting-computer.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TargetingComputerHotacAbility : GenericAbility
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
                    description: "You may acquire a lock on an enemy ship in Range",
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
            return HostShip.GetRangeToShip(ship) <= 3 && ship.Owner!=HostShip.Owner;
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