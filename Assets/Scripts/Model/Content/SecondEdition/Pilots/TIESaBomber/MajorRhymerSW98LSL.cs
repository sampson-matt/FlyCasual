using ActionsList;
using Arcs;
using Bombs;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class MajorRhymerSW98LSL : TIESaBomber
        {
            public MajorRhymerSW98LSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Major Rhymer",
                    4,
                    33,
                     tags: new List<Tags>
                    {
                        Tags.LsL
                    },
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.MajorRhymerLSLAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "majorrhymer-swz98-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //While you perform a Torpedo attack, if the defender is in your bullseye, change 1 Focus result to a Crit result.
    public class MajorRhymerLSLAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostShip.PilotInfo.PilotName,
                IsDiceModificationAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Change,
                1,
                new List<DieSide>() { DieSide.Focus },
                DieSide.Crit
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsDiceModificationAvailable()
        {
            return (Combat.AttackStep == CombatStep.Attack
                && Combat.Attacker == HostShip
                && Combat.ChosenWeapon.WeaponType == WeaponTypes.Torpedo
                && Combat.DiceRollAttack.Focuses > 0
                && Combat.Attacker.SectorsInfo.IsShipInSector(Combat.Defender, ArcType.Bullseye)
            );
        }

        private int GetDiceModificationAiPriority()
        {
            return 70;
        }


    }
}