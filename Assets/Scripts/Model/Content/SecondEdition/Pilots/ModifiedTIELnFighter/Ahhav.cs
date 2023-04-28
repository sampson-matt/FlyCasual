using Upgrade;

namespace Ship
{
    namespace SecondEdition.ModifiedTIELnFighter
    {
        public class Ahhav : ModifiedTIELnFighter
        {
            public Ahhav() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Ahhav",
                    3,
                    27,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.AhhavAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AhhavAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += RegisterAhhavAttackAbility;
            HostShip.AfterGotNumberOfDefenceDice += RegisterAhhavDefenceAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice -= RegisterAhhavAttackAbility;
            HostShip.AfterGotNumberOfDefenceDice -= RegisterAhhavDefenceAbility;
        }

        private void RegisterAhhavAttackAbility(ref int result)
        {
            if (Combat.Attacker == HostShip && Combat.Defender.ShipInfo.BaseSize > Combat.Attacker.ShipInfo.BaseSize)
            {
                Messages.ShowInfo("Ahhav is attacking a larger ship and gains +1 attack die");
                result++;
            }
        }

        private void RegisterAhhavDefenceAbility(ref int result)
        {
            if (Combat.Defender == HostShip && Combat.Attacker.ShipInfo.BaseSize > Combat.Defender.ShipInfo.BaseSize)
            {
                Messages.ShowInfo("Ahhav is defending against a larger ship and gains +1 defence die");
                result++;
            }
        }
    }
}