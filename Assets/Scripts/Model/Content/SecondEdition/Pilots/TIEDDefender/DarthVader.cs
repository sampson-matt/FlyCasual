﻿using System;
using System.Collections.Generic;
using Upgrade;
using Content;

namespace Ship.SecondEdition.TIEDDefender
{
    public class DarthVader : TIEDDefender
    {
        public DarthVader() : base()
        {
            PilotInfo = new PilotCardInfo(
                "Darth Vader",
                6,
                116,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.DarthVaderDefenderAbility),
                tags: new List<Tags>
                {
                    Tags.DarkSide,
                    Tags.Sith
                },
                extraUpgradeIcon: UpgradeType.ForcePower,
                abilityText: "You may not spend force charges except when attacking. While you perform an attack, you may spend 1 force charge to turn a blank result into a hit.",
                force: 3
            );

            PilotNameCanonical = "darthvader-tieddefender";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DarthVaderDefenderAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostShip.PilotInfo.PilotName,
                IsAvailable,
                GetAiPriority,
                DiceModificationType.Change,
                count: 1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                sideCanBeChangedTo: DieSide.Success,
                payAbilityCost: SpendForce
            );

            HostShip.OnCheckCanUseForceNow += AddRestriction;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Attack &&
                Combat.DiceRollAttack.Blanks > 0 &&
                HostShip.State.Force > 0;
        }

        private int GetAiPriority()
        {
            return 45;
        }

        private void SpendForce(Action<bool> callback)
        {
            HostShip.State.SpendForce(1, delegate { callback(true); });
        }

        private void AddRestriction(ref bool isAllowed)
        {
            if (Combat.Attacker == null || Combat.Attacker.ShipId != HostShip.ShipId) isAllowed = false;
        }
    }
}