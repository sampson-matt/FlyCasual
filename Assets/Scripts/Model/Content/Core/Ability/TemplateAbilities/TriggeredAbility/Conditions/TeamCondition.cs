using UnityEngine;

namespace Abilities
{
    public class TeamCondition : Condition
    {
        private ShipTypes ShipType { get; }

        public TeamCondition(ShipTypes shipType)
        {
            ShipType = shipType;
        }

        public override bool Passed(ConditionArgs args)
        {
            if (args.ShipToCheck == null || args.ShipAbilityHost == null)
            {
                Messages.ShowError("Ability Condition Error: ship is not set");
                return false;
            }

            switch (ShipType)
            {
                case ShipTypes.This:
                    return args.ShipToCheck.ShipId == args.ShipAbilityHost.ShipId;
                case ShipTypes.Friendly:
                    return Tools.IsFriendly(args.ShipToCheck, args.ShipAbilityHost);
                case ShipTypes.OtherFriendly:
                    return args.ShipToCheck.Owner.PlayerNo == args.ShipAbilityHost.Owner.PlayerNo
                        && args.ShipToCheck.ShipId != args.ShipAbilityHost.ShipId;
                case ShipTypes.Enemy:
                    return !Tools.IsSameTeam(args.ShipToCheck, args.ShipAbilityHost);
                case ShipTypes.Any:
                    return true;
                default:
                    Debug.Log("Unknown shiptype");
                    return false;
            }
        }
    }
}
