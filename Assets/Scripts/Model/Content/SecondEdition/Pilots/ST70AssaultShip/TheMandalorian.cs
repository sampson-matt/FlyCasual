using Arcs;
using BoardTools;
using Ship;
using Content;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ST70AssaultShip
    {
        public class TheMandalorian : ST70AssaultShip
        {
            public TheMandalorian() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "The Mandalorian",
                    5,
                    49,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TheMandalorianAbility),
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian,
                        Tags.BountyHunter
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TheMandalorianAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostShip.PilotInfo.PilotName,
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Change,
                1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                sideCanBeChangedTo: DieSide.Focus
            );
        }

        private bool IsAvailable()
        {
            return IsInFrontSectorOf2Ships()
                && DiceRoll.CurrentDiceRoll.Blanks > 0;
        }

        private bool IsInFrontSectorOf2Ships()
        {
            int count = 0;

            foreach (GenericShip enemyShip in HostShip.Owner.EnemyShips.Values)
            {
                int rangeInFrontSector = enemyShip.SectorsInfo.RangeToShipBySector(HostShip, ArcType.Front);
                if (rangeInFrontSector >= 1 && rangeInFrontSector <= 2)
                {
                    count++;
                    if (count == 2) return true;
                }
            }

            return false;
        }

        private int GetAiPriority()
        {
            return 100; // Free change limited by side if 1
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}