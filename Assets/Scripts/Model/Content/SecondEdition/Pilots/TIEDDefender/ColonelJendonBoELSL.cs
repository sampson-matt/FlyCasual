using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;
using Actions;
using Abilities.SecondEdition;
using System.Linq;
using SubPhases;

namespace Ship
{
    namespace SecondEdition.TIEDDefender
    {
        public class ColonelJendonBoELSL : TIEDDefender
        {
            public ColonelJendonBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Colonel Jendon",
                    6,
                    80,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ColonelJendonBattleOverEndorAbility),
                    extraUpgradeIcons: new List<UpgradeType>(){ UpgradeType.Talent, UpgradeType.Sensor}
                );
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(EvadeAction), typeof(BarrelRollAction)));
                FullThrottleAbility oldAbility = (FullThrottleAbility)ShipAbilities.First(n => n.GetType() == typeof(FullThrottleAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new ChissEngineeringAbility());
                PilotNameCanonical = "coloneljendon-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ColonelJendonBattleOverEndorAbility : GenericAbility
    {
        //While you defend, if you are not shielded, you may change 1 of your blank results to a Focus result.
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

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Defence
                && HostShip.State.ShieldsCurrent == 0
                && Combat.DiceRollDefence.Blanks > 0;
        }

        private int GetAiPriority()
        {
            return 100;
        }


    }

    public class ChissEngineeringAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully += CheckTargetLockAbility;
            HostShip.OnAttackStartAsAttacker += RegisterAttackAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinishSuccessfully -= CheckTargetLockAbility;
            HostShip.OnAttackStartAsAttacker -= RegisterAttackAbility;
        }

        private void RegisterAttackAbility()
        {
            if (HostShip.State.ShieldsCurrent < 1)
                return;

            if (HostShip.IsStressed)
                return;

            if (Combat.ShotInfo.Range == 1)
                return;

            RegisterAbilityTrigger(TriggerTypes.OnAttackStart, delegate
            {
                AskToUseAbility(
                    "Chiss Engineering",
                    AlwaysUseByDefault,
                    UseAttackAbility,
                    descriptionLong: "Do you want to spend 1 Shield to apply the range 1 bonus?",
                    imageHolder: HostShip
                );
            });
        }

        private void UseAttackAbility(object sender, EventArgs e)
        {
            Rules.DistanceBonus.OnCheckAllowRangeOneBonus += ApplyRangeOneBonus;
            HostShip.LoseShield();
            DecisionSubPhase.ConfirmDecision();
        }

        private void ApplyRangeOneBonus(ref bool isActive)
        {
            Rules.DistanceBonus.OnCheckAllowRangeOneBonus -= ApplyRangeOneBonus;

            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: Spent 1 Shield to apply the Range 1 bonus");
            isActive = true;
        }


        private void CheckTargetLockAbility(GenericShip ship)
        {
            if (ship.AssignedManeuver.Speed > 2)
            {
                Triggers.RegisterTrigger(new Trigger()
                {
                    Name = "Chiss Engineering",
                    TriggerType = TriggerTypes.OnMovementFinish,
                    TriggerOwner = HostShip.Owner.PlayerNo,
                    EventHandler = AskPerformLockAction,
                    Sender = HostReal,
                });
            }
        }

        private void AskPerformLockAction(object sender, System.EventArgs e)
        {
            HostShip.AskPerformFreeAction(
                new TargetLockAction(),
                Triggers.FinishTrigger,
                "Chiss Engineering",
                "After you fully execute a speed 3-5 maneuver, you may perform a Lock action",
                HostShip
            );
        }
    }
}