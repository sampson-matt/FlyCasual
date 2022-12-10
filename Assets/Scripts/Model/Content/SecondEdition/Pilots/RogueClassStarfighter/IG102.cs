using System.Collections.Generic;
using Upgrade;
using System;
using Abilities.SecondEdition;
using System.Linq;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class IG102 : RogueClassStarfighter
        {
            public IG102() : base()
            {

                PilotInfo = new PilotCardInfo(
                    "IG-102",
                    4,
                    39,
                    pilotTitle: "Dueling Droid",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.IG102Ability),
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                DeadToRights oldAbility = (DeadToRights)ShipAbilities.First(n => n.GetType() == typeof(DeadToRights));
                oldAbility.DeactivateAbility();
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/ig102.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class IG102Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostName,
                IsDiceModificationAvailable,
                GetDiceModificationPriority,
                DiceModificationType.Change,
                1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                sideCanBeChangedTo: DieSide.Focus
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private int GetDiceModificationPriority()
        {
            int result = 0;

            if (Combat.CurrentDiceRoll.Blanks > 0) result = 100;

            return result;
        }

        private bool IsDiceModificationAvailable()
        {
            bool result = false;

            if (Combat.AttackStep == CombatStep.Defence)
            {
                if (Combat.Attacker.PilotInfo.Initiative>=HostShip.PilotInfo.Initiative)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}