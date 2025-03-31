using Abilities.SecondEdition;
using Arcs;
using Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class GemmerSojanBoELSL : RZ1AWing
        {
            public GemmerSojanBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Gemmer Sojan",
                    2,
                    33,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.GemmerSojanBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );
                PilotNameCanonical = "gemmersojan-battleoverendor-lsl";
                ShipInfo.ArcInfo.Arcs.Add(new ShipArcInfo(ArcType.SingleTurret, 2));
                ShipInfo.ArcInfo.Arcs.RemoveAll(n => n.ArcType == ArcType.Front);
                VectoredThrustersAbility oldAbility = (VectoredThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(VectoredThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new VectoredCannonsAbility());
            }            
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GemmerSojanBattleOverEndorAbility : GenericAbility
    {
        //While defending, you may gain 1 strain token to change up to 2 of your blank results to focus results.
        public override void ActivateAbility()
        {
            AddDiceModification(
                "Gemmer Sojan",
                IsAvailable,
                AiPriority,
                DiceModificationType.Change,
                2,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Blank },
                sideCanBeChangedTo: DieSide.Focus,
                payAbilityCost: PayAbilityCost
            );
        }
        private void PayAbilityCost(Action<bool> callback)
        {
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), () => callback(true));
        }

        public bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Defence && Combat.CurrentDiceRoll.HasResult(DieSide.Blank);
        }

        private int AiPriority()
        {
            int result = 0;

            if (Combat.DiceRollAttack.Successes > Combat.DiceRollDefence.Successes
                && Combat.DiceRollDefence.Blanks > 0 
                && HostShip.Tokens.HasToken(typeof(FocusToken))
                && Combat.DiceRollAttack.Successes > Combat.DiceRollDefence.Focuses + Combat.DiceRollDefence.Successes)
            {
                result = 55;
            }
            return result;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}