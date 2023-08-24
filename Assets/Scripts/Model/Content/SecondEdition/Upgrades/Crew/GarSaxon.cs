using Upgrade;
using Ship;
using BoardTools;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class GarSaxon : GenericUpgrade
    {
        public GarSaxon() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Gar Saxon",
                UpgradeType.Crew,
                cost: 6,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Imperial),
                abilityType: typeof(Abilities.SecondEdition.GarSaxonCrewAbility)
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class GarSaxonCrewAbility : GenericAbility
    {
        //When a friendly ship at range 1-3 with an initiative of 4 or lower performs an attack on a defender you have locked,
        //the attacker may change 1 Focus to a Hit. 
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostName,
                IsAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Change,
                1,
                new List<DieSide> { DieSide.Focus },
                DieSide.Success, 
                isGlobal: true
            );
        }

        private bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Attack
                && Tools.IsFriendly(Combat.Attacker, HostShip)
                && Combat.Attacker.State.Initiative <= 4
                && Combat.CurrentDiceRoll.Focuses > 0
                && IsInRangeFromOneToThree(HostShip, Combat.Defender)
                && ActionsHolder.HasTargetLockOn(HostShip, Combat.Defender);
        }

        public int GetDiceModificationAiPriority()
        {
            int result = 0;

            if (Combat.AttackStep == CombatStep.Attack)
            {
                int attackFocuses = Combat.DiceRollAttack.FocusesNotRerolled;
                int attackBlanks = Combat.DiceRollAttack.BlanksNotRerolled;

                if (attackFocuses > 0) result = 100;
            }

            return result;
        }

        private bool IsInRangeFromOneToThree(GenericShip ship1, GenericShip ship2)
        {
            DistanceInfo distInfo = new DistanceInfo(ship1, ship2);
            return (distInfo.Range == 1 || distInfo.Range == 2);
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

    }
}