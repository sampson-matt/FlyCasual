using BoardTools;
using Conditions;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;


namespace Ship
{
    namespace SecondEdition.AuzituckGunship
    {
        public class Tarfful : AuzituckGunship
        {
            public Tarfful() : base()
            {
                IsHidden = true;
                PilotInfo = new PilotCardInfo(
                    "Tarfful",
                    5,
                    59,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TarffulAbility),
                    tags: new List<Tags>
                    {
                        Tags.Wookie,
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/Tarfful.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TarffulAbility : GenericAbility
    {
        protected virtual string Prompt
        {
            get
            {
                return "Assign the Liberated condition to 1 friendly ship other than Tarfful.";
            }
        }
        public override void ActivateAbility()
        {
            Phases.Events.OnSetupEnd += RegisterTarffulAbility;
            GenericShip.OnAttackFinishGlobal += CheckTarffulAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnSetupEnd -= RegisterTarffulAbility;
            GenericShip.OnAttackFinishGlobal -= CheckTarffulAbility;
        }

        private void CheckTarffulAbility(GenericShip ship)
        {

            if (Tools.IsFriendly(Combat.Defender, HostShip) && Combat.Defender.Tokens.HasToken<Liberated>())
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskToSelectTarget);
            }
        }

        private void AskToSelectTarget(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                ShouldAbilityBeUsed,
                AcquireTargetLock,
                descriptionLong: "Do you want to acquire a Lock on " + Combat.Attacker.PilotInfo.PilotName + "?",
                imageHolder: HostShip
            );
        }

        private bool ShouldAbilityBeUsed()
        {
            return (!HostShip.Tokens.HasToken<BlueTargetLockToken>(letter: '*'));
        }

        private void AcquireTargetLock(object sender, System.EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            IsAbilityUsed = true;
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " acquired a Lock on " + Combat.Attacker.PilotInfo.PilotName);
            ActionsHolder.AcquireTargetLock(HostShip, Combat.Attacker, Triggers.FinishTrigger, Triggers.FinishTrigger, ignoreRange: true);
        }

        private void RegisterTarffulAbility()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = HostShip.ShipId + ": Assign \"Liberated\" condition",
                TriggerType = TriggerTypes.OnSetupEnd,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = SelectTarffulTarget,
            });
        }

        private void SelectTarffulTarget(object Sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                  AssignLiberated,
                  CheckRequirements,
                  GetAiGuardedPriority,
                  HostShip.Owner.PlayerNo,
                  "Liberated",
                  Prompt,
                  HostUpgrade
            );
        }

        protected virtual void AssignLiberated()
        {
            // Remove Liberated from all friendly ships
            foreach (var kvp in Roster.AllShips)
            {
                GenericShip ship = kvp.Value;
                ship.Tokens.RemoveCondition(typeof(Liberated));
            }
            TargetShip.Tokens.AssignCondition(new Liberated(TargetShip) { SourceUpgrade = HostUpgrade });
            SelectShipSubPhase.FinishSelection();
        }

        protected virtual bool CheckRequirements(GenericShip ship)
        {
            var match = Tools.IsFriendly(ship, HostShip)
                && (!ship.PilotInfo.IsLimited || ship.PilotInfo.Tags.Contains(Content.Tags.Wookie))
                && ship.PilotInfo.PilotName != "Tarfful";
            return match;
        }

        private int GetAiGuardedPriority(GenericShip ship)
        {
            int result = 0;

            result += (ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost));

            return result;
        }
    }
}

namespace Conditions
{
    public class Liberated : GenericToken
    {
        public GenericUpgrade SourceUpgrade;
        private GenericShip curToDamage;
        private DamageSourceEventArgs curDamageInfo;

        public Liberated(GenericShip host) : base(host)
        {
            Name = ImageName = "Liberated Condition";
            Temporary = false;
            Tooltip = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/Liberated.png";
        }

        public override void WhenAssigned()
        {
            Host.OnShipIsDestroyed += RegisterTrigger;
            GenericShip.OnTryDamagePreventionGlobal += CheckDrawTheirFireAbility;
        }

        public override void WhenRemoved()
        {
            Host.OnShipIsDestroyed -= RegisterTrigger;
            GenericShip.OnTryDamagePreventionGlobal -= CheckDrawTheirFireAbility;
        }

        private void CheckDrawTheirFireAbility(GenericShip ship, DamageSourceEventArgs e)
        {
            curToDamage = ship;
            curDamageInfo = e;

            if (AbilityCanBeUsed())
            {
                Triggers.RegisterTrigger
                (
                    new Trigger()
                    {
                        Name = "Remove Broken Trust",
                        TriggerType = TriggerTypes.OnTryDamagePrevention,
                        TriggerOwner = Host.Owner.PlayerNo,
                        EventHandler = StartQuestionSubphase
                    }
                );
            }
        }

        protected virtual bool AbilityCanBeUsed()
        {
            // Is the damage type a ship attack?
            if (curDamageInfo.DamageType != DamageTypes.ShipAttack)
                return false;

            // Is the defender on our team and not us? If not return.
            if (!Tools.IsFriendly(curToDamage, Host) || curToDamage.ShipId == Host.ShipId)
                return false;

            // Is the defender at range 1 and is there a hit/crit result?
            if (!Board.IsShipAtRange(Host, curToDamage, 1) || curToDamage.AssignedDamageDiceroll.Successes < 1)
                return false;

            return true;
        }

        protected class BiggsDarklighterDecisionSubPhase : DecisionSubPhase { }

        protected virtual void StartQuestionSubphase(object sender, System.EventArgs e)
        {
            BiggsDarklighterDecisionSubPhase selectBiggsDarklighterSubPhase = (BiggsDarklighterDecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                Name,
                typeof(BiggsDarklighterDecisionSubPhase),
                Triggers.FinishTrigger
            );

            selectBiggsDarklighterSubPhase.DescriptionShort = Name;
            selectBiggsDarklighterSubPhase.DescriptionLong = "You may suffer 1 Hit or Crit damage to cancel 1 matching result";
            selectBiggsDarklighterSubPhase.ImageSource = SourceUpgrade;

            if (curToDamage.AssignedDamageDiceroll.RegularSuccesses > 0)
            {
                selectBiggsDarklighterSubPhase.AddDecision("Redirect Hit damage", delegate { PreventDamage(DieSide.Success); });
                selectBiggsDarklighterSubPhase.AddTooltip("Redirect Hit damage", Tooltip);
            }

            if (curToDamage.AssignedDamageDiceroll.CriticalSuccesses > 0)
            {
                selectBiggsDarklighterSubPhase.AddDecision("Redirect Crit damage", delegate { PreventDamage(DieSide.Crit); });
                selectBiggsDarklighterSubPhase.AddTooltip("Redirect Crit damage", Tooltip);
            }

            selectBiggsDarklighterSubPhase.AddDecision("No", delegate { DecisionSubPhase.ConfirmDecision(); });
            selectBiggsDarklighterSubPhase.DefaultDecisionName = GetDefaultDecision();
            selectBiggsDarklighterSubPhase.ShowSkipButton = true;
            selectBiggsDarklighterSubPhase.DecisionOwner = Host.Owner;
            selectBiggsDarklighterSubPhase.Start();
        }

        private void PreventDamage(DieSide type)
        {
            // Find a crit and remove it from the ship we're protecting's assigned damage.
            Die dieToRemove = curToDamage.AssignedDamageDiceroll.DiceList.Find(n => n.Side == type);
            curToDamage.AssignedDamageDiceroll.DiceList.Remove(dieToRemove);

            DamageSourceEventArgs drawtheirfireDamage = new DamageSourceEventArgs()
            {
                Source = Name,
                DamageType = DamageTypes.CardAbility
            };

            int hits = type == DieSide.Success ? 1 : 0;
            int crits = type == DieSide.Crit ? 1 : 0;
            Host.Damage.TryResolveDamage(hits, crits, drawtheirfireDamage, DecisionSubPhase.ConfirmDecision);
        }

        protected string GetDefaultDecision()
        {
            string result = "No";

            if (Host.State.HullCurrent > 1)
            {
                if (curToDamage.AssignedDamageDiceroll.RegularSuccesses > 0)
                {
                    result = "Redirect Hit damage";
                }

                if (curToDamage.AssignedDamageDiceroll.CriticalSuccesses > 0)
                {
                    result = "Redirect Crit damage";
                }

            }

            return result;
        }

        private void RegisterTrigger(GenericShip ship, bool flag)
        {
            var otherFriendliesCount = Roster.GetPlayer(Host.Owner.PlayerNo).Ships.Values
                .Where(s => s != null && !s.IsDestroyed && s.ShipId != Host.ShipId && s.PilotInfo.PilotName != "Tarfful")
                .Count();

            // Do nothing if there aren't any friendlies left
            if (otherFriendliesCount == 0)
            {
                return;
            }

            Triggers.RegisterTrigger(new Trigger()
            {
                Name = "Liberated",
                TriggerType = TriggerTypes.OnShipIsDestroyed,
                TriggerOwner = Host.Owner.PlayerNo,
                EventHandler = AssignConditionToAnotherFriendly
            });
        }

        private void AssignConditionToAnotherFriendly(object sender, EventArgs e)
        {
            var otherFriendlies = Roster.GetPlayer(Host.Owner.PlayerNo).Ships.Values
                .Where(s => s != null && !s.IsDestroyed && s.ShipId != Host.ShipId && s.PilotInfo.PilotName != "Tarfful")
                .ToArray();

            // Do nothing if there aren't any friendlies left
            if (otherFriendlies.Length == 0)
            {
                return;
            }

            LiberatedDecisionSubPhase selectAllyDecisionSubPhase = Phases.StartTemporarySubPhaseNew<LiberatedDecisionSubPhase>(Name, Triggers.FinishTrigger);

            selectAllyDecisionSubPhase.DescriptionShort = "Tarfful";
            selectAllyDecisionSubPhase.DescriptionLong = "Assign the Liberated condition to 1 friendly ship other than Tarfful";
            selectAllyDecisionSubPhase.ImageSource = SourceUpgrade;

            foreach (var friendlyShip in otherFriendlies)
            {
                var friendly = friendlyShip;
                selectAllyDecisionSubPhase.AddDecision(
                    friendlyShip.ShipId + ": " + friendlyShip.PilotInfo.PilotName,
                    delegate
                    {
                        SelectTarget(friendly);
                    }
                );
            }

            selectAllyDecisionSubPhase.DescriptionShort = "Liberated: Select another friendly ship";

            GenericShip mostWorthAlly = otherFriendlies
                .OrderBy(ally => ally.State.Initiative)
                .Reverse()
                .FirstOrDefault();
            selectAllyDecisionSubPhase.DefaultDecisionName = mostWorthAlly.ShipId + ": " + mostWorthAlly.PilotInfo.PilotName;
            selectAllyDecisionSubPhase.RequiredPlayer = Host.Owner.PlayerNo;
            selectAllyDecisionSubPhase.Start();
        }

        private class LiberatedDecisionSubPhase : SubPhases.DecisionSubPhase { }

        private void SelectTarget(GenericShip targetShip)
        {
            Messages.ShowInfo("Liberated: " + targetShip.PilotInfo.PilotName + " (" + targetShip.ShipId + ") is selected");

            targetShip.Tokens.AssignCondition(typeof(Liberated));

            SubPhases.DecisionSubPhase.ConfirmDecision();
        }
    }
}
