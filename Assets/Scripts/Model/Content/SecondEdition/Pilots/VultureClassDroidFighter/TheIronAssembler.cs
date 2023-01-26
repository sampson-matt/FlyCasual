using BoardTools;
using System;
using Ship;
using SubPhases;
using System.Linq;
using System.Collections.Generic;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class TheIronAssembler : VultureClassDroidFighter
    {
        public TheIronAssembler()
        {
            PilotInfo = new PilotCardInfo(
                "The Iron Assembler",
                1,
                22,
                true,
                charges: 3,
                abilityType: typeof(Abilities.SecondEdition.TheIronAssemblerAbility),
                pilotTitle: "Scintilla Scavenger"
            );

            ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/theironassembler.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    //After a friendly ship at range 0-1 skips its execute maneuver step, you may spend 1 Charge. If you do, if there is an asteroid or debris cloud at range 0 of it, that ship may repair 1 damage. 
    public class TheIronAssemblerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnManeuverIsSkippedGlobal += CheckAbility;

        }


        public override void DeactivateAbility()
        {
            GenericShip.OnManeuverIsSkippedGlobal -= CheckAbility;
        }


        private void CheckAbility(GenericShip ship)
        {
            bool obstacleCheck = ship.ObstaclesLanded.Any(n => n.GetTypeName == "Asteroid" || n.GetTypeName == "Debris");

            if (ship.IsManeuverSkipped && obstacleCheck && Tools.IsFriendly(ship, HostShip)
                && new DistanceInfo(ship, HostShip).Range < 2 && ship.Damage.IsDamaged)
            {
                TargetShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnManeuverIsSkipped, AskToUseOwnAbility);
            }
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {
            TheIronAssemblerDecisionSubphase subphase = Phases.StartTemporarySubPhaseNew<TheIronAssemblerDecisionSubphase>("The Iron Assembler Decision", Triggers.FinishTrigger);

            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "You may spend 1 charge to:";
            subphase.ImageSource = HostShip;

            if (TargetShip.Damage.HasFacedownCards)
            {
                subphase.AddDecision("Repair 1 facedown damage card", RepairFacedownDamageCard);
            }

            if (TargetShip.Damage.HasFaceupCards)
            {
                subphase.AddDecision("Repair 1 faceup damage card", RepairFaceupDamageCard);
            }

            subphase.DecisionOwner = HostShip.Owner;
            subphase.DefaultDecisionName = subphase.GetDecisions().First().Name;
            subphase.ShowSkipButton = true;

            subphase.Start();
        }

        private void RepairFacedownDamageCard(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.SpendCharge();
            
            if (TargetShip.Damage.DiscardRandomFacedownCard())
            {
                Messages.ShowInfoToHuman("Facedown Damage card is discarded");
            }

            Triggers.FinishTrigger();
        }

        private void RepairFaceupDamageCard(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.SpendCharge();
            List<GenericDamageCard> shipCritsList = TargetShip.Damage.GetFaceupCrits();

            if (shipCritsList.Count == 1)
            {
                TargetShip.Damage.FlipFaceupCritFacedown(shipCritsList.First());
                Triggers.FinishTrigger();
            }
            else if (shipCritsList.Count > 1)
            {
                Phases.StartTemporarySubPhaseOld(
                    HostShip.PilotInfo.PilotName + ": Select faceup ship Crit",
                    typeof(SubPhases.R5AstromechDecisionSubPhase),
                    Triggers.FinishTrigger
                );
            }
        }
        private class TheIronAssemblerDecisionSubphase : DecisionSubPhase { }
    }
}
