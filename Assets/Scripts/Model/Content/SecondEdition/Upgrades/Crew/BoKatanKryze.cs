using Ship;
using Upgrade;
using System;
using SubPhases;
using Actions;
using ActionsList;

namespace UpgradesList.SecondEdition
{
    public class BoKatanKryze : GenericUpgrade
    {
        public BoKatanKryze() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Bo-Katan Kryze",
                UpgradeType.Crew,
                cost: 5,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Republic, Faction.Separatists),
                abilityType: typeof(Abilities.SecondEdition.BoKatanKryzeAbility)
            );
        }
    }
}


namespace Abilities.SecondEdition
{
    public class BoKatanKryzeAbility : GenericAbility
    {
        //While you perform an attack, if you are at range 0-1 of the defender, you may reroll 1 attack die. 
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostName,
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Reroll,
                1
            );
        }

        private bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Attack && Combat.ShotInfo.Range <= 1;
        }

        private int GetAiPriority()
        {
            return 90;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

    }
}