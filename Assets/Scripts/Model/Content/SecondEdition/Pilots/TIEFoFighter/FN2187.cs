using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Tokens;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class FN2187 : TIEFoFighter
        {
            public FN2187() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "FN-2187",
                    1,
                    28,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.FN2187Ability)
                );

                PilotNameCanonical = "fn2187-wat1";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class FN2187Ability : GenericAbility
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
                Messages.ShowInfoToHuman(HostShip.PilotInfo.PilotName + ": You may gain a strain token to chose 1 enemy ship in your front arc to gain 1 deplete token");

                SelectTargetForAbility(
                    SelectAbilityTarget,
                    FilterAbilityTarget,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostShip.PilotInfo.PilotName,
                    "You may gain a strain token to chose 1 enemy ship in your front arc to gain 1 deplete token",
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
                FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.Enemy }) &&
                Board.IsShipInArc(HostShip, ship);
        }

        private void SelectAbilityTarget()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();
            GenericShip thisship = TargetShip;
            thisship.Tokens.AssignToken(typeof(DepleteToken), delegate { });
            HostShip.Tokens.AssignToken(typeof(StrainToken), Triggers.FinishTrigger);
        }
    }
}

