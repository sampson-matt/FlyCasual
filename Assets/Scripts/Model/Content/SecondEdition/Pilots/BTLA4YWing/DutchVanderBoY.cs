using Abilities.SecondEdition;
using ActionsList;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class DutchVanderBoY : BTLA4YWing
        {
            public DutchVanderBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Dutch\" Vander",
                    4,
                    44,
                    isLimited: true,
                    abilityType: typeof(DutchVanderBoYAbility),
                    extraUpgradeIcon: UpgradeType.Modification
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/dutchvander-boy.png";
                PilotNameCanonical = "dutchvander-boy";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DutchVanderBoYAbility : GenericAbility
    {
        private ITargetLockable LockedShip;

        public override void ActivateAbility()
        {
            HostShip.OnTokenIsSpent += CheckDutchVanderBoYAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsSpent -= CheckDutchVanderBoYAbility;
        }

        private void CheckDutchVanderBoYAbility(GenericShip ship, GenericToken token)
        {
            if (token is BlueTargetLockToken && HasPossibleTargets())
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsSpent, StartAbility);
            }
        }

        private bool HasPossibleTargets()
        {
            return BoardTools.Board.GetShipsAtRange(HostShip, new Vector2(1, 3), Team.Type.Friendly).Count > 0;
        }

        private void StartAbility(object sender, EventArgs e)
        {
            LockedShip = Combat.Defender;
            if (LockedShip == null)
            {
                Messages.ShowError(HostShip.PilotInfo.PilotName + " doesn't have any targets!");
                Triggers.FinishTrigger();
                return;
            }

            SelectTargetForAbility(
                GetTargetLockOnSameTarget,
                AnotherFriendlyShipInRange,
                AiPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                "Choose a ship, that ship will acquire a lock on the object you locked",
                HostShip
            );
        }

        private void GetTargetLockOnSameTarget()
        {
            if (LockedShip is GenericShip)
            {
                Messages.ShowInfo(TargetShip.PilotInfo.PilotName + " acquired a Target Lock on " + (LockedShip as GenericShip).PilotInfo.PilotName);
            }
            else
            {
                Messages.ShowInfo(TargetShip.PilotInfo.PilotName + " acquired a Target Lock on obstacle");
            }

            ActionsHolder.AcquireTargetLock(TargetShip, LockedShip, SelectShipSubPhase.FinishSelection, SelectShipSubPhase.FinishSelection, ignoreRange: true);
        }

        private bool AnotherFriendlyShipInRange(GenericShip ship)
        {
            return FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.OtherFriendly }) && FilterTargetsByRange(ship, 1, 3);
        }

        private int AiPriority(GenericShip ship)
        {
            int priority = 0;

            if (!ship.Tokens.HasToken(typeof(BlueTargetLockToken))) priority += 50;

            if (LockedShip is GenericShip)
            {
                BoardTools.ShotInfo shotInfo = new BoardTools.ShotInfo(ship, LockedShip as GenericShip, ship.PrimaryWeapons);
                if (shotInfo.IsShotAvailable) priority += 40;
            }

            priority += ship.State.Firepower * 5;

            return priority;
        }
    }
}