using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Arcs;
using Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class ArvelCrynydBoELSL : RZ1AWing
        {
            public ArvelCrynydBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Arvel Crynyd",
                    3,
                    38,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ArvelCrynydBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );
                PilotNameCanonical = "arvelcrynyd-battleoverendor-lsl";
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(SlamAction)));
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
    public class ArvelCrynydBattleOverEndorAbility : GenericAbility
    {
        //While defending, you may gain 1 strain token to change 1 focus result to a evade result.
        public override void ActivateAbility()
        {
            AddDiceModification(
                "Arvel Crynyd",
                IsAvailable,
                AiPriority,
                DiceModificationType.Change,
                1,
                sidesCanBeSelected: new List<DieSide>() { DieSide.Focus },
                sideCanBeChangedTo: DieSide.Success,
                payAbilityCost: PayAbilityCost
            );
        }
        private void PayAbilityCost(Action<bool> callback)
        {
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), () => callback(true));
        }

        public bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Defence && Combat.CurrentDiceRoll.HasResult(DieSide.Focus);
        }

        private int AiPriority()
        {
            int result = 0;

            if (Combat.DiceRollAttack.Successes > Combat.DiceRollDefence.Successes
                && Combat.DiceRollDefence.Focuses > 0 && !HostShip.Tokens.HasGreenTokens)
            {
                result = 15;
            }
            return result;
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }
    }
}