using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;
using Arcs;
using BoardTools;

namespace UpgradesList.SecondEdition
{
    public class KirKanosPilotAbility : GenericUpgrade
    {
        public KirKanosPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Kir Kanos Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.KirKanosAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/kirkanos.png";
        }

    }
}

namespace Abilities.SecondEdition
{
    public class KirKanosAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnGenerateDiceModifications += AddKirKanosbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateDiceModifications -= AddKirKanosbility;
        }

        private void AddKirKanosbility(GenericShip ship)
        {
            ship.AddAvailableDiceModificationOwn(new KirKanosDiceModification()
            {
                ImageUrl = HostUpgrade.ImageUrl
            });
        }

        private class KirKanosDiceModification : GenericAction
        {
            public KirKanosDiceModification()
            {
                Name = DiceModificationName = "Kir Kanos";
            }

            public override void ActionEffect(System.Action callBack)
            {
                Combat.CurrentDiceRoll.AddDiceAndShow(DieSide.Success);
                callBack();
            }

            public override bool IsDiceModificationAvailable()
            {
                bool result = false;
                if (Combat.AttackStep == CombatStep.Attack)
                {
                    ShotInfo shotInfo = new ShotInfo(Combat.Defender, Combat.Attacker, Combat.Defender.PrimaryWeapons);
                    if (shotInfo.Range==2 || shotInfo.Range==3) result = true;
                }
                return result;
            }

            public override int GetDiceModificationPriority()
            {
                return 110;
            }
        }

    }
}