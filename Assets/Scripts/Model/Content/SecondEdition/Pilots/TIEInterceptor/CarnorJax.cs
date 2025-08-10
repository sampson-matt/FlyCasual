using Abilities.SecondEdition;
using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship.SecondEdition.TIEInterceptor
{
    public class CarnorJax : TIEInterceptor
    {
        public CarnorJax() : base()
        {
            PilotInfo = new PilotCardInfo(
                "Carnor Jax",
                5,
                49,
                isLimited: true,
                abilityType: typeof(CarnorJaxAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );
            ImageUrl= "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/CarnorJax.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CarnorJaxAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterAbility;
        }

        private void RegisterAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, Ability);
        }

        private void Ability(object sender, EventArgs e)
        {
            if (TargetsForAbilityExist(FilterAbilityTarget))
            {
                Selection.ChangeActiveShip(HostShip);
                Messages.ShowInfoToHuman(HostShip.PilotInfo.PilotName + ": You may chose 1 enemy ship at range 0-1 in your front arc to gain 1 jam token");

                SelectTargetForAbility(
                    SelectAbilityTarget,
                    FilterAbilityTarget,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostShip.PilotInfo.PilotName,
                    "You may chose 1 enemy ship at range 0-1 in your front arc to gain 1 jam token",
                    HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            return ship.PilotInfo.Cost;
        }

        private bool FilterAbilityTarget(GenericShip ship)
        {
            return
                FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.Enemy }) 
                && FilterTargetsByRangeInArc(ship, 0, 1);
        }

        private void SelectAbilityTarget()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();
            GenericShip thisship = TargetShip;
            thisship.Tokens.AssignToken(new JamToken(thisship, HostShip.Owner), Triggers.FinishTrigger);
        }
    }
}