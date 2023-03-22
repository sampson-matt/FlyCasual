using System.Collections.Generic;
using Upgrade;
using System.Linq;
using System;
using Content;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class DBS404SoC : HyenaClassDroidBomber
    {
        public DBS404SoC()
        {
            PilotInfo = new PilotCardInfo(
                "DBS-404",
                4,
                30,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.DBS404SoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Torpedo, UpgradeType.Missile, UpgradeType.Device },
                pilotTitle: "Siege of Coruscant"
            );

            PilotNameCanonical = "dbs404-siegeofcoruscant";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/dbs404-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DBS404SoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += CheckBonusDice;
            HostShip.OnAttackHitAsAttacker += CheckSelfDamage;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice -= CheckBonusDice;
            HostShip.OnAttackHitAsAttacker -= CheckSelfDamage;
        }

        private void CheckBonusDice(ref int count)
        {
            if (Combat.ShotInfo.Range < 2)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": Attacker rolls +1 attack die");
                count++;
            }
        }

        private void CheckSelfDamage()
        {
            if (Combat.ShotInfo.Range < 2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackHit, SufferSelfDamage);
            }
        }

        private void SufferSelfDamage(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": Suffers critical damage");

            HostShip.Damage.TryResolveDamage(
                0,
                1,
                new DamageSourceEventArgs()
                {
                    Source = HostShip,
                    DamageType = DamageTypes.CardAbility
                },
                Triggers.FinishTrigger
            );
        }
    }
}