using BoardTools;
using Ship;
using System.Collections.Generic;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class ItsATrap : GenericUpgrade
    {
        public ItsATrap() : base()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "It's A Trap!",
                UpgradeType.Talent,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.ItsATrapAbility)
            );

            IsHidden = true;

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/ItsATrap.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    //While defending if there are more other friendly ships than enemy ships at range 0-1, you may reroll 1 of your blank results
    public class ItsATrapAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                "It's A Trap!",
                IsDiceModificationAvailable,
                GetAiPriority,
                DiceModificationType.Reroll,
                1,
                new List<DieSide>() { DieSide.Blank },
                timing: DiceModificationTimingType.AfterRolled
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsDiceModificationAvailable()
        {
            return Combat.AttackStep == CombatStep.Defence 
                && Combat.Defender == HostShip 
                && Combat.CurrentDiceRoll.Blanks > 0
                && HasMoreFriendlyShipsInRange(Combat.Defender);
        }

        private bool HasMoreFriendlyShipsInRange(GenericShip ship)
        {
            int friendlyShipsInRange = -1;
            int enemyShipsInRange = 0;

            foreach (GenericShip anotherShip in Roster.AllShips.Values)
            {
                DistanceInfo distInfo = new DistanceInfo(ship, anotherShip);
                if (distInfo.Range <= 1)
                {
                    if (Tools.IsFriendly(ship, anotherShip))
                    {
                        friendlyShipsInRange++;
                    }
                    else
                    {
                        enemyShipsInRange++;
                    }
                }
            }

            return  friendlyShipsInRange > enemyShipsInRange;
        }

        private int GetAiPriority()
        {
            return 95;
        }


    }
}