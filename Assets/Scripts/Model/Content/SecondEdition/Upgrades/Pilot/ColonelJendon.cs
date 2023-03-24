using Upgrade;
using System.Collections.Generic;
using UnityEngine;
using BoardTools;
using System;
using System.Linq;
using Ship;
using SubPhases;

namespace UpgradesList.SecondEdition
{
    public class ColonelJendonPilotAbility : GenericUpgrade
    {
        public ColonelJendonPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Colonel Jendon Pilot Ability",
                UpgradeType.Pilot,

                cost: 6,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        3
                    ),
                abilityType: typeof(Abilities.SecondEdition.ColonelJendonHotacAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/coloneljendon.png";
        }
    }
}

namespace Abilities.SecondEdition
{    
    public class ColonelJendonHotacAbility : GenericAbility
    {
        public GenericShip friendlyShip;
        public GenericShip targetShip;

        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= CheckAbility;
        }

        private void CheckAbility()
        {
            if (hasEligilbleFriendlyShipInRange())
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, GrantTargetLockAbility);
            }
        }

        private void GrantTargetLockAbility(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                    SelectTargetForLock,
                    FilterFriendlyTargets,
                    GetFriednlyShipAiPriority,
                    HostShip.Owner.PlayerNo,
                    name: HostShip.PilotInfo.PilotName,
                    description: "You may choose a friendly ship at range 1",
                    imageSource: HostUpgrade
                );
        }

        private void SelectTargetForLock()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();

            friendlyShip = TargetShip;

            SelectTargetForAbility(
                GetLockIgnoringRange,
                FilterTargets,
                GetAiPriority,
                HostShip.Owner.PlayerNo,
                name: HostUpgrade.UpgradeInfo.Name,
                description: "You may acquire a lock on a ship, ignoring range restrictions",
                imageSource: HostUpgrade
            );
        }

        private void GetLockIgnoringRange()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();

            ActionsHolder.AcquireTargetLock(
                friendlyShip,
                TargetShip,
                Triggers.FinishTrigger,
                Triggers.FinishTrigger,
                ignoreRange: true
            );
        }

        private bool FilterTargets(GenericShip ship)
        {
            return FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.Enemy });
        }

        private int GetAiPriority(GenericShip ship)
        {
            DistanceInfo distInfo = new DistanceInfo(ship, TargetShip);
            return 100 - distInfo.Range;
        }

        private bool FilterFriendlyTargets(GenericShip ship)
        {
            return getEligibleFriendlies().Contains(ship);
        }

        private int GetFriednlyShipAiPriority(GenericShip ship)
        {
            int priority = ship.PilotInfo.Initiative;
            return priority;
        }

        private bool hasEligilbleFriendlyShipInRange()
        {
            return getEligibleFriendlies().Count() > 0;
        }

        private IEnumerable<GenericShip> getEligibleFriendlies()
        {
            return Board.GetShipsAtRange(HostShip, new Vector2(1, 1), Team.Type.Friendly).Where(s => !s.Tokens.HasToken(typeof(Tokens.BlueTargetLockToken)));
        }
    }
}