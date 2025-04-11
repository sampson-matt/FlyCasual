using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ModifiedYT1300LightFreighter
    {
        public class LandoCalrissianBoELSL : ModifiedYT1300LightFreighter
        {
            public LandoCalrissianBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lando Calrissian",
                    5,
                    75,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    abilityType: typeof(LandoCalrissianBattleOverEndorAbility),
                    charges: 2,
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipAbilities.Add(new HighStakesAbility());
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(EvadeAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(CoordinateAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(CoordinateAction), ActionColor.Red));                
                PilotNameCanonical = "landocalrissian-battleoverendor-lsl";
            }
            
        }
        
    }
}

namespace Abilities.SecondEdition
{
    //At the start of the Activation Phase, you may spend 1 Charge. If you do, choose an initiative from 1 to 6. You activate at that initiative this phase.
    public class LandoCalrissianBattleOverEndorAbility : GenericAbility, IModifyPilotSkill
    {
        int initiative = 0;
        public override void ActivateAbility()
        {
            HostShip.OnActivationPhaseStart += RegisterTrigger;
        }
        public override void DeactivateAbility()
        {
            HostShip.OnActivationPhaseStart -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            if (HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnActivationPhaseStart, AskToUseAbility);
            }
        }

        private void AskToUseAbility(object sender, System.EventArgs e)
        {
            if (HostShip.State.Charges > 0)
            {
                AskToUseAbility(
                    HostShip.PilotInfo.PilotName,
                    NeverUseByDefault,
                    UseAbility,
                    descriptionLong: "Do you want to spend a charge to choose an initiative to activate at?",
                    imageHolder: HostShip
                );
            }
        }

        private void UseAbility(object sender, System.EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            HostShip.State.Charges--;
            var selectionSubPhase = (InitiativeSelectionsSubPhase)Phases.StartTemporarySubPhaseNew(
                "Choose an initiative from 1 to 6. You activate at that initiative this phase.",
                typeof(InitiativeSelectionsSubPhase),
                Triggers.FinishTrigger
            );

            selectionSubPhase.DescriptionShort = "Lando";
            selectionSubPhase.DescriptionLong = String.Format("Choose an initiative from 1 to 6. You activate at that initiative this phase.");
            selectionSubPhase.ImageSource = HostUpgrade;

            for (var i = 1; i <= 6; i++)
            {
                int option = i;
                selectionSubPhase.AddDecision(option.ToString(),
                    delegate
                    {
                        this.initiative = option;
                        updateInitiative();
                    }
                );
            }

            selectionSubPhase.DefaultDecisionName = "1";
            selectionSubPhase.RequiredPlayer = HostShip.Owner.PlayerNo;
            selectionSubPhase.Start();
        }

        private void updateInitiative()
        {            
            HostShip.State.AddPilotSkillModifier(this);
            Phases.Events.OnActivationPhaseEnd_NoTriggers += RemovePilotSkillModifieer;
            SubPhases.DecisionSubPhase.ConfirmDecision();
        }

        private void RemovePilotSkillModifieer()
        {
            Phases.Events.OnActivationPhaseEnd_NoTriggers -= RemovePilotSkillModifieer;
            HostShip.State.RemovePilotSkillModifier(this);
        }

        public void ModifyPilotSkill(ref int pilotSkill)
        {
            pilotSkill = initiative;
        }

        private class InitiativeSelectionsSubPhase : DecisionSubPhase { }
    }

    //After you perform a red action, you may roll an attack die. On a hit/crit result, remove 1 stress.
    public class HighStakesAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckActionAbility;
        }
        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckActionAbility;
        }

        private void CheckActionAbility(GenericAction action)
        {
            if (action.IsRed && action.HostShip.Tokens.CountTokensByType<Tokens.StressToken>() >= 1
            )
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskToRoll);
            }
        }

        private void AskToRoll(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                AlwaysUseByDefault,
                UseAbility,
                descriptionLong: "Do you want to roll 1 attack die? (On a \"hit\" or \"crit\" result, remove 1 stress token)",
                imageHolder: HostShip
            );
        }

        private void UseAbility(object sender, System.EventArgs e)
        {
            Phases.StartTemporarySubPhaseOld(
                HostShip.PilotInfo.PilotName + ": Try to remove stress",
                typeof(SubPhases.BraylenStrammCheckSubPhase),
                delegate {
                    //We have a BraylenStrammCheckSubPhase open, so finish it
                    Phases.FinishSubPhase(typeof(SubPhases.BraylenStrammCheckSubPhase));

                    //We have a Decision SubPhase open, so finish it
                    SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

                    //The trigger is still active, so finish it.  Must be explicitly finished since ConfirmDecisionNoCallback was used
                    Triggers.FinishTrigger();
                }
            );
        }
    }
}

