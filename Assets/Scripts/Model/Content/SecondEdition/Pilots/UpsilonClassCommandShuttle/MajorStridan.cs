﻿using BoardTools;
using Ship;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.UpsilonClassCommandShuttle
    {
        public class MajorStridan : UpsilonClassCommandShuttle
        {
            public MajorStridan() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Major Stridan",
                    4,
                    61,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.MajorStridanAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MajorStridanAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckRange += CheckRangeModification;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckRange -= CheckRangeModification;
        }

        private void CheckRangeModification(GenericShip anotherShip, int minRange, int maxRange, RangeCheckReason reason, ref bool isInRange)
        {
            if ((Tools.IsFriendly(anotherShip, HostShip))
                && (reason == RangeCheckReason.CoordinateAction || reason == RangeCheckReason.UpgradeCard)
                && (minRange >= 0 || maxRange <= 1)
            )
            {
                DistanceInfo distInfo = new DistanceInfo(HostShip, anotherShip);
                if (distInfo.Range >= 2 && distInfo.Range <= 3)
                {
                    isInRange = true;
                }
            }
        }
    }
}
