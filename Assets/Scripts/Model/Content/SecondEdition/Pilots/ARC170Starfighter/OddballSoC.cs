using System;
using System.Collections.Generic;
using Upgrade;
using Ship;
using Tokens;
using BoardTools;
using SubPhases;
using UnityEngine;
using ActionsList;
using Content;
using Movement;
using Editions;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class OddballSoC : ARC170Starfighter
        {
            public OddballSoC() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Odd Ball\"",
                    5,
                    48,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.OddBallSoCAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "oddball-soc";

                ModelInfo.SkinName = "Red";

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/oddball-soc.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BornForThisAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostName + ": Born for This: Focus",
                IsFocusAvailable,
                () => 20,
                DiceModificationType.Change,
                int.MaxValue,
                new List<DieSide> { DieSide.Focus },
                DieSide.Success,
                isGlobal: true,
                payAbilityCost: PayFocusCost
            );

            AddDiceModification(
                HostName + ": Born for This: Evade",
                IsEvadeAvailable,
                GetDiceModificationPriority,
                DiceModificationType.Change,
                1,
                new List<DieSide> { DieSide.Blank, DieSide.Focus },
                DieSide.Success,
                isGlobal: true,
                payAbilityCost: PayEvadeCost
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private int GetDiceModificationPriority()
        {
            int result = 0;

            if (Combat.AttackStep == CombatStep.Defence)
            {
                int attackSuccessesCancelable = Combat.DiceRollAttack.SuccessesCancelable;
                int defenceSuccesses = Combat.CurrentDiceRoll.Successes;
                if (attackSuccessesCancelable > defenceSuccesses)
                {
                    int defenceFocuses = Combat.DiceRollDefence.Focuses;
                    int numFocusTokens = Selection.ActiveShip.Tokens.CountTokensByType(typeof(FocusToken));
                    if (numFocusTokens > 0 && defenceFocuses == Combat.CurrentDiceRoll.Count)
                    {
                        // Multiple focus results on our defense roll and we have a Focus token.  Use it instead of the Evade.
                        result = 0;
                    }
                    else
                    {
                        // Either we don't have a focus token or we have at least one blank.  Better use the Evade.
                        result = 70;
                    }
                }
            }

            if (Edition.Current is Editions.SecondEdition && Combat.CurrentDiceRoll.Failures == 0) return 0;

            return result;
        }

        private bool IsEvadeAvailable()
        {
            GenericShip activeShip = Combat.Defender;
            return Combat.AttackStep == CombatStep.Defence
                && !HostShip.IsStrained
                && Tools.IsFriendly(activeShip, HostShip)
                && activeShip != HostShip
                && Board.IsShipBetweenRange(HostShip, activeShip, 0, 2)
                && HostShip.Tokens.HasToken(typeof(EvadeToken));
        }

        private void PayEvadeCost(Action<bool> callback)
        {
            if (HostShip.Tokens.HasToken(typeof(FocusToken)))
            {
                HostShip.Tokens.AssignToken(typeof(StrainToken), delegate { });
                HostShip.Tokens.RemoveToken(typeof(EvadeToken), () => callback(true));
            }
            else callback(false);
        }

        private bool IsFocusAvailable()
        {
            GenericShip activeShip = Combat.Defender;
            return Combat.AttackStep == CombatStep.Defence
                && Combat.CurrentDiceRoll.Focuses > 0
                && !HostShip.IsStrained
                && Tools.IsFriendly(activeShip, HostShip)
                && activeShip != HostShip
                && Board.IsShipBetweenRange(HostShip, activeShip, 0, 2)
                && HostShip.Tokens.HasToken(typeof(FocusToken));
        }

        private void PayFocusCost(Action<bool> callback)
        {
            if (HostShip.Tokens.HasToken(typeof(FocusToken)))
            {
                HostShip.Tokens.AssignToken(typeof(StrainToken), delegate { });
                HostShip.Tokens.RemoveToken(typeof(FocusToken), () => callback(true));
            }
            else callback(false);
        }
    }
}

namespace Abilities.SecondEdition
{
    public class OddBallSoCAbility : GenericAbility
    {
        private GenericShip LockedShip;
        private GenericShip PreviousCurrentShip;

        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckConditions;
            HostShip.OnMovementFinishSuccessfully += RegisterMovementTrigger;
        }
        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckConditions;
            HostShip.OnMovementFinishSuccessfully -= RegisterMovementTrigger;
        }

        protected void CheckConditions(GenericAction action)
        {
            if (action.IsRed)
            {
                HostShip.OnActionDecisionSubphaseEnd += RegisterActionTrigger;
            }
        }

        protected void RegisterMovementTrigger(GenericShip ship)
        {
            if (HostShip.GetLastManeuverColor() == MovementComplexity.Complex && Board.GetShipsAtRange(HostShip, new Vector2(0, 1), Team.Type.Enemy).Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskToChooseLockedTarget);
            }
        }

        // SELECT A LOCKED TARGET

        private void RegisterActionTrigger(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= RegisterActionTrigger;

            if (Board.GetShipsAtRange(HostShip, new Vector2(0, 1), Team.Type.Enemy).Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToChooseLockedTarget);
            }
        }

        private void AskToChooseLockedTarget(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                AskToSelectAnotherFriendlyShip,
                FilterTargets,
                GetLockedTargetAiPriority,
                HostShip.Owner.PlayerNo,
                name: HostShip.PilotInfo.PilotName,
                description: "You may choose an enemy ship at range 0-1...",
                imageSource: HostShip
            );
        }

        private bool FilterTargets(GenericShip ship)
        {
            return Board.GetShipsAtRange(HostShip, new Vector2(0, 1), Team.Type.Enemy).Contains(ship);
        }

        private int GetLockedTargetAiPriority(GenericShip ship)
        {
            return ship.PilotInfo.Cost;
        }

        // SELECT ANOTHER FRIENDLY SHIP

        private void AskToSelectAnotherFriendlyShip()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();

            LockedShip = TargetShip;

            SelectTargetForAbility(
                AcquireTargetLock,
                FilterFriendlyTargets,
                GetFriednlyShipAiPriority,
                HostShip.Owner.PlayerNo,
                name: HostShip.PilotInfo.PilotName,
                description: "Choose a friendly ship at range 0-3, it may acquire a lock on that enemy ship",
                imageSource: HostShip
            );
        }

        private bool FilterFriendlyTargets(GenericShip ship)
        {
            return Board.GetShipsAtRange(HostShip, new Vector2(0, 3), Team.Type.Friendly).Contains(ship);
        }

        private int GetFriednlyShipAiPriority(GenericShip ship)
        {
            int priority = ship.PilotInfo.Cost;

            DistanceInfo distInfo = new DistanceInfo(ship, LockedShip);
            if (distInfo.Range < 4) priority += 100;

            ShotInfo shotInfo = new ShotInfo(ship, LockedShip, ship.PrimaryWeapons);
            if (shotInfo.IsShotAvailable) priority += 50;

            if (!ship.Tokens.HasToken<BlueTargetLockToken>('*')) priority += 100;

            if (!ship.ActionBar.HasAction(typeof(TargetLockAction))) priority = 0;

            return priority;
        }

        private void AcquireTargetLock()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();

            IsAbilityUsed = true;
            Messages.ShowInfo(TargetShip.PilotInfo.PilotName + " acquired a Lock on " + LockedShip.PilotInfo.PilotName);
            ActionsHolder.AcquireTargetLock(TargetShip, LockedShip, FinishAbility, FinishAbility);
        }

        private void FinishAbility()
        {
            
            Selection.ChangeActiveShip(HostShip);
            Triggers.FinishTrigger();
        }
    }
}
