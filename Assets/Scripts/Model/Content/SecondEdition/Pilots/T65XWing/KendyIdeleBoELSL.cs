using Abilities.SecondEdition;
using System.Collections.Generic;
using Ship;
using SubPhases;
using BoardTools;
using Content;
using Actions;
using ActionsList;
using Tokens;
using System;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class KendyIdeleBoELSL : T65XWing
        {
            public KendyIdeleBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Kendy Idele",
                    4,
                    48,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    abilityType: typeof(Abilities.SecondEdition.KendyIdeleAbility)
                );
                ShipAbilities.Add(new LockedSFoils());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(BoostAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction)));
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                PilotNameCanonical = "kendyidele-battleoverendor-lsl";
                ModelInfo.SkinName = "Luke Skywalker";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class KendyIdeleAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnTokenIsSpent += RegisterKendyIdeleAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsSpent -= RegisterKendyIdeleAbility;
        }

        private void RegisterKendyIdeleAbility(GenericShip ship, GenericToken token)
        {
            if (token.TokenColor == TokenColors.Green)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsSpent, SelectTargetForAbility);
            }
        }

        private void SelectTargetForAbility(object sender, EventArgs e)
        {
            if (HasTargetsForAbility())
            {
                SelectTargetForAbility(
                    GrantAction,
                    FilterTargets,
                    GetAiPriority,
                    HostShip.Owner.PlayerNo,
                    HostShip.PilotInfo.PilotName,
                    "You may choose a  friendly ship at range 1-3 and gain a strain token. If you do, that ship may perform a red Focus or red Evade action. ",
                    HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private int GetAiPriority(GenericShip ship)
        {
            int result = 0;

            result += NeedTokenPriority(ship);
            result += ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost);

            return result;
        }

        private int NeedTokenPriority(GenericShip ship)
        {
            if (!ship.Tokens.HasToken(typeof(FocusToken))) return 100;
            if (ship.ActionBar.HasAction(typeof(EvadeAction)) && !ship.Tokens.HasToken(typeof(EvadeToken))) return 50;
            return 0;
        }

        private void GrantAction()
        {
            TargetShip.BeforeActionIsPerformed += PayStrainCost;

            SelectShipSubPhase.FinishSelectionNoCallback();
            Selection.ThisShip = TargetShip;
            List<GenericAction> actions = new List<GenericAction>() { new FocusAction() { Color = ActionColor.Red }, new EvadeAction()  { Color = ActionColor.Red }};

            TargetShip.AskPerformFreeAction(
                actions,
                delegate {
                    Selection.ThisShip = HostShip;
                    TargetShip.BeforeActionIsPerformed -= PayStrainCost;
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "You may perform an action, even if you is stressed.",
                HostShip
            );
        }

        private void PayStrainCost(GenericAction action, ref bool isFreeAction)
        {
            TargetShip.BeforeActionIsPerformed -= PayStrainCost;

            RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, GainStrain);
        }

        private void GainStrain(object sender, EventArgs e)
        {
            HostShip.Tokens.AssignToken(typeof(Tokens.StrainToken), Triggers.FinishTrigger);
        }

        private bool HasTargetsForAbility()
        {
            foreach (GenericShip ship in HostShip.Owner.Ships.Values)
            {
                if (FilterTargets(ship)) return true;
            }

            return false;
        }

        private bool FilterTargets(GenericShip ship)
        {
            return BoardTools.Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 3), Team.Type.Friendly).Contains(ship);
        }
    }

    public class LockedSFoils : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckConditions;
        }
        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckConditions;
        }
        private void CheckConditions(GenericAction action)
        {
            if (action is BoostAction)
            {
                HostShip.OnActionDecisionSubphaseEnd += RegisterTrigger;
            }
        }
        private void RegisterTrigger(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= RegisterTrigger;

            Triggers.RegisterTrigger(new Trigger()
            {
                Name = HostName + "'s ability",
                TriggerType = TriggerTypes.OnActionDecisionSubPhaseEnd,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = DoLockedSFoilsAbility
            });
        }
        private void DoLockedSFoilsAbility(object sender, System.EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " gains one Deplete token after performing a Boost action");
            HostShip.Tokens.AssignToken(typeof(DepleteToken), Triggers.FinishTrigger);
        }
    }
}
