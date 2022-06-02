using Ship;
using Tokens;
using System.Collections.Generic;
using System.Linq;
using Editions;
using System;
using Players;

namespace RulesList
{
    public enum ProtectIsNotAllowedReasons
    {
        NotInRange,
        EnemyShip
    }

    public class ProtectRule
    {
        static bool RuleIsInitialized = false;

        public delegate void EventHandlerListProtectIsNotAllowedReasons2Ships(ref List<ProtectIsNotAllowedReasons> blockReasons, GenericShip protectSource, GenericShip protectTarget);
        public static event EventHandlerListProtectIsNotAllowedReasons2Ships OnCheckProtectIsAllowed;
        public static event EventHandlerListProtectIsNotAllowedReasons2Ships OnCheckProtectIsDisallowed;

        public ProtectRule()
        {
            if (!RuleIsInitialized)
            {
                RuleIsInitialized = true;
            }
        }

        public bool ProtectIsAllowed(GenericShip protectSource, GenericShip protectTarget)
        {
            List<ProtectIsNotAllowedReasons> blockReasons = new List<ProtectIsNotAllowedReasons>();

            if (!Tools.IsSameTeam(protectSource, protectTarget))
            {
                blockReasons.Add(ProtectIsNotAllowedReasons.EnemyShip);
            }

            int rangeBetween = protectTarget.GetRangeToShip(protectSource);

            if (rangeBetween == 1)
            {

            }
            else
            {
                blockReasons.Add(ProtectIsNotAllowedReasons.NotInRange);
            }

            if (blockReasons.Count > 0) OnCheckProtectIsAllowed?.Invoke(ref blockReasons, protectSource, protectTarget);
            if (blockReasons.Count == 0) OnCheckProtectIsDisallowed?.Invoke(ref blockReasons, protectSource, protectTarget);

            return blockReasons.Count == 0;
        }
    }
}
