using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Content;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.FangFighter
    {
        public class TorPhun : FangFighter
        {
            public TorPhun() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Tor Phun",
                    3,
                    47,
                    pilotTitle: "Direct Pressure",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TorPhunAbility),
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Modification }
                );

                ModelInfo.SkinName = "Skull Squadron Pilot";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TorPhunAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnShipIsDestroyedGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnShipIsDestroyedGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, bool flag)
        {
            if (!(Phases.CurrentPhase is MainPhases.CombatPhase))
                return;

            if (Tools.IsSameShip(ship, Combat.Defender)
                && Tools.IsSameShip(HostShip, Combat.Attacker))
            {
                RegisterAbilityTrigger(TriggerTypes.OnShipIsDestroyed, PerformAction);
                if (!HostShip.IsCannotAttackSecondTime) HostShip.OnCombatCheckExtraAttack += RegisterTorPhunExtraAttackAbility;
            }
        }

        private void PerformAction(object sender, System.EventArgs e)
        {
            CameraScript.RestoreCamera();

            HostShip.OnCanPerformActionWhileStressed += TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed += TemporaryAllowActionsWhileStressed;
            HostShip.OnActionIsPerformed += DisallowActionsWhileStressed;
            HostShip.OnActionIsSkipped += DisallowActionsWhileStressedAlt;

            List<GenericAction> actions = Selection.ThisShip.GetAvailableActions();            

            HostShip.AskPerformFreeAction(
                actions,
                delegate {
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "You may perfrom an action, even while stressed",
                HostShip
            );
        }

        private void DisallowActionsWhileStressed(GenericAction action)
        {
            HostShip.OnCanPerformActionWhileStressed -= TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed -= TemporaryAllowActionsWhileStressed;
            HostShip.OnActionIsPerformed -= DisallowActionsWhileStressed;
        }

        private void DisallowActionsWhileStressedAlt(GenericShip ship)
        {
            HostShip.OnCanPerformActionWhileStressed -= TemporaryAllowAnyActionsWhileStressed;
            HostShip.OnCheckCanPerformActionsWhileStressed -= TemporaryAllowActionsWhileStressed;
            HostShip.OnActionIsPerformed -= DisallowActionsWhileStressed;
            HostShip.OnActionIsSkipped -= DisallowActionsWhileStressedAlt;
        }

        private void TemporaryAllowAnyActionsWhileStressed(GenericAction action, ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void TemporaryAllowActionsWhileStressed(ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void RegisterTorPhunExtraAttackAbility(GenericShip ship)
        {
            HostShip.OnCombatCheckExtraAttack -= RegisterTorPhunExtraAttackAbility;

            RegisterAbilityTrigger(TriggerTypes.OnCombatCheckExtraAttack, StartExtraAttack);
        }

        private void StartExtraAttack(object sender, EventArgs e)
        {
            HostShip.IsCannotAttackSecondTime = true;

            Combat.StartSelectAttackTarget(
                HostShip,
                FinishAbility,
                AnyTarget,
                HostShip.PilotInfo.PilotName,
                "You may gain 2 Strain tokens to perform a bonus attack",
                HostShip
            );
        }

        private bool AnyTarget(GenericShip ship, IShipWeapon weapon, bool isSilent)
        {
            return true;
        }

        private void FinishAbility()
        {
            if (Selection.ThisShip.IsAttackSkipped)
            {
                Selection.ThisShip.IsCannotAttackSecondTime = false;
                Triggers.FinishTrigger();
            }
            else
            {
                Selection.ThisShip.IsAttackPerformed = true;
                AssignTwoStrainTokens();
            }            
        }

        private void AssignTwoStrainTokens()
        {
            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: 2 Strain tokens are gained to perform a bonus attack");
            HostShip.Tokens.AssignTokens(CreateStrainToken, 2, Triggers.FinishTrigger);
        }

        private GenericToken CreateStrainToken()
        {
            return new StrainToken(HostShip);
        }
    }
}