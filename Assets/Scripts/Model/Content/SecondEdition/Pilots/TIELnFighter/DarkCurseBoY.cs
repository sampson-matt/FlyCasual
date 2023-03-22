using Upgrade;
using Ship;
using Content;
using System.Collections.Generic;
using System;

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
                    34,
                    isLimited: true,
                    extraUpgradeIcon: UpgradeType.Talent,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(Abilities.SecondEdition.DarkCurseAbility)
                );

                PilotNameCanonical = "darkcurse-battleofyavin";

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
            Combat.Attacker.OnAttackFinish += RemoveDarkCursePilotAbility;
        }

        private void RemoveDarkCursePilotAbility(GenericShip ship)
        {
            Combat.Attacker.OnTryAddAvailableDiceModification -= UseDarkCurseRestriction;
            Combat.Attacker.OnTryAddDiceModificationOpposite -= UseDarkCurseRestriction;
            Combat.Attacker.OnAttackFinish -= RemoveDarkCursePilotAbility;
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
