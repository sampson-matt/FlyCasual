using Abilities.SecondEdition;
using Ship;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class Howlrunner : TIELnFighter
        {
            public Howlrunner() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Howlrunner\"",
                    5,
                    39,
                    isLimited: true,
                    abilityType: typeof(HowlrunnerAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //When a friendly ship at range 0-1 performs a primary attack, that ship may reroll one attack die.

    public class HowlrunnerAbility : Abilities.FirstEdition.HowlrunnerAbility
    {
        protected override bool IsAvailable()
        {
            if (Combat.AttackStep != CombatStep.Attack) return false;

            if (!Tools.IsFriendly(Combat.Attacker, HostShip)) return false;

            if (Combat.ChosenWeapon.WeaponType != WeaponTypes.PrimaryWeapon) return false;

            BoardTools.DistanceInfo positionInfo = new BoardTools.DistanceInfo(HostShip, Combat.Attacker);
            if (positionInfo.Range > 1) return false;

            return true;
        }
    }
}