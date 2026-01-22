using GameModes;
using Movement;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class SlaveISep : GenericUpgrade
    {
        public SlaveISep() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Slave I",
                UpgradeType.Title,
                cost: 3,
                isLimited: true,
                addSlot: new UpgradeSlot(UpgradeType.Gunner),
                restrictions: new UpgradeCardRestrictions(
                    new ShipRestriction(typeof(Ship.SecondEdition.FiresprayClassPatrolCraft.FiresprayClassPatrolCraft)),
                    new FactionRestriction(Faction.Scum, Faction.Separatists)
                ),
                abilityType: typeof(Abilities.SecondEdition.SlaveISeparatistsAbility)
            );

            NameCanonical = "slavei-swz82";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class SlaveISeparatistsAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                "Slave I",
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Change,
                count: 1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Success },
                sideCanBeChangedTo: DieSide.Crit
            );
        }

        private bool IsAvailable()
        {
            return Combat.ArcForShot.ArcType == Arcs.ArcType.Front
                && Combat.Defender.SectorsInfo.IsShipInSector(HostShip, Arcs.ArcType.FullRear)
                && Combat.DiceRollAttack.HasResult(DieSide.Success);
        }

        private int GetAiPriority()
        {
            return 20;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}