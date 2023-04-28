using Ship;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.VCX100LightFreighter
    {
        public class AlexandrKallus : VCX100LightFreighter
        {
            public AlexandrKallus() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Alexsandr Kallus",
                    4,
                    68,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.AlexandrKallusAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //While you defend, if the attacker modified any attack dice, you may roll an additional defense die.
    public class AlexandrKallusAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfDefenceDice += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfDefenceDice -= CheckAbility;
        }

        private void CheckAbility(ref int value)
        {
            if (Combat.AttackStep == CombatStep.Defence && Combat.DiceRollAttack.ModifiedByPlayers.Contains(Combat.Attacker.Owner.PlayerNo))
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " rolls an additional defense die");
                value++;
            }
        }
    }
}