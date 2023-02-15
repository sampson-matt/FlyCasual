using Abilities.SecondEdition;
using Arcs;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Content;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class PopsKrailBoY : BTLA4YWing
        {
            public PopsKrailBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Pops\" Krail",
                    3,
                    36,
                    pilotTitle: "Battle of Yavin",
                    isLimited: true,
                    abilityType: typeof(PopsKrailBoYAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Modification }
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/popskrail-boy.png";
                PilotNameCanonical = "popskrail-battleofyavin";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PopsKrailBoYAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostShip.PilotInfo.PilotName,
                IsDiceModificationAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Reroll,
                2
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        public bool IsDiceModificationAvailable()
        {
            bool result = false;
            if (Combat.AttackStep == CombatStep.Attack
                && Combat.ShotInfo.InArcByType(ArcType.SingleTurret))
            {
                result = true;
            }
            return result;
        }

        public int GetDiceModificationAiPriority()
        {
            return 90;
        }


    }
}