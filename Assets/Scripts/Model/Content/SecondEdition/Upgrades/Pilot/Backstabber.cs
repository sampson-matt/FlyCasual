using Upgrade;
using BoardTools;

namespace UpgradesList.SecondEdition
{
    public class BackstabberPilotAbility : GenericUpgrade
    {
        public BackstabberPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Backstabber Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        4
                    ),
                abilityType: typeof(Abilities.SecondEdition.BackstabberPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/backstabber.png";
        }


    }
}

namespace Abilities.SecondEdition
{
    public class BackstabberPilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice -= CheckAbility;
        }

        private void CheckAbility(ref int diceNumber)
        {
            ShotInfo shotInformation = new ShotInfo(Combat.Defender, Combat.Attacker, Combat.ChosenWeapon);
            if (!shotInformation.InPrimaryArc)
            {
                Messages.ShowInfo("Backstabber is attacking from outside the defender's primary firing arc and rolls an additional attack die");
                diceNumber++;
            }
        }
    }
}