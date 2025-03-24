using Abilities.SecondEdition;
using BoardTools;
using Ship;
using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.UT60DUWing
    {
        public class BodhiRook : UT60DUWing
        {
            public BodhiRook() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Bodhi Rook",
                    4,
                    48,
                    isLimited: true,
                    abilityType: typeof(BodhiRookAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BodhiRookAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            RulesList.TargetLocksRule.OnCheckTargetLockIsAllowed += CanPerformTargetLock;
        }

        public override void DeactivateAbility()
        {
            RulesList.TargetLocksRule.OnCheckTargetLockIsAllowed -= CanPerformTargetLock;
        }

        private void CanPerformTargetLock(ref bool result, GenericShip ship, ITargetLockable defender)
        {
            if (!Tools.IsFriendly(ship, HostShip)) return;

            foreach (GenericShip friendlyShip in HostShip.Owner.Ships.Values)
            {
                if (defender.GetRangeToShip(friendlyShip) < 4 && Tools.IsFriendly(friendlyShip, HostShip))
                {
                    result = true;
                    return;
                }
            }
        }
    }
}
