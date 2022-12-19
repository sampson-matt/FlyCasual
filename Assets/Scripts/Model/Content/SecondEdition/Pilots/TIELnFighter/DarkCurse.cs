using Upgrade;
using Ship;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class DarkCurse: TIELnFighter
        {
            public DarkCurse() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Dark Curse\"",
                    6,
                    35,
                    isLimited: true,
                    extraUpgradeIcon: UpgradeType.Talent,
                    abilityType: typeof(Abilities.SecondEdition.DarkCurseAbility)
                );
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/darkcurse-boy.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DarkCurseAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsDefender += AddDarkCursePilotAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsDefender -= AddDarkCursePilotAbility;
        }

        private void AddDarkCursePilotAbility()
        {
            Combat.Attacker.OnTryAddAvailableDiceModification += UseDarkCurseRestriction;
            Combat.Attacker.OnTryAddDiceModificationOpposite += UseDarkCurseRestriction;
        }

        private void UseDarkCurseRestriction(GenericShip ship, ActionsList.GenericAction diceModification, ref bool canBeUsed)
        {
            if (!diceModification.IsNotRealDiceModification)
            {
                Messages.ShowErrorToHuman(HostShip.PilotInfo.PilotName + ": "+ Combat.Attacker.PilotInfo.PilotName+ " is unable to modify dice");
                canBeUsed = false;
            }
        }
    }
}
