using BoardTools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.FiresprayClassPatrolCraft
    {
        public class BobaFettSep : FiresprayClassPatrolCraft
        {
            public BobaFettSep() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Boba Fett",
                    3,
                    68,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BobaFettSeparatistAbility),
                    extraUpgradeIcon: UpgradeType.Crew,
                    factionOverride: Faction.Separatists
                );

                PilotNameCanonical = "bobafett-separatistalliance";

                ModelInfo.SkinName = "Jango Fett";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BobaFettSeparatistAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification
            (
                "Boba Fett",
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Change,
                count: 1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                sideCanBeChangedTo: DieSide.Focus                
            );
        }

        private bool IsAvailable()
        {
            int friendlyShipsInRange = Board.GetShipsAtRange(HostShip, new Vector2(0, 2), Team.Type.Friendly).Count();

            return Combat.AttackStep == CombatStep.Defence
                && Combat.DiceRollDefence.HasResult(DieSide.Blank)
                && friendlyShipsInRange == 1;
        }

        private int GetAiPriority()
        {
            return 100;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}