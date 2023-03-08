using Abilities.SecondEdition;
using System;
using Ship;
using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEAdvancedX1
    {
        public class DarthVaderBOY : TIEAdvancedX1
        {
            public DarthVaderBOY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Darth Vader",
                    6,
                    73,
                    isLimited: true,
                    abilityType: typeof(DarthVaderBoYAbility),
                    force: 3,
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.DarkSide,
                        Tags.Sith
                    },
                    extraUpgradeIcon: UpgradeType.ForcePower
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/darthvader-boy.png";
                PilotNameCanonical = "darthvader-battleofyavin";
                ModelInfo.SkinName = "Blue";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DarthVaderBoYAbility : GenericAbility
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
    }
}