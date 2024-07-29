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
            HostShip.SpendCharge();
            List<GenericDamageCard> shipCritsList = TargetShip.Damage.GetFaceupCrits();

            if (shipCritsList.Count == 1)
            {
                TargetShip.Damage.FlipFaceupCritFacedown(shipCritsList.First(), DecisionSubPhase.ConfirmDecision);
            }
            else if (shipCritsList.Count > 1)
            {
                IronAssemblerDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<IronAssemblerDecisionSubPhase>(
                    "The Iron Assembler: Select faceup ship Crit",
                    DecisionSubPhase.ConfirmDecision
                );
                subphase.DescriptionShort = "The Iron Assembler";
                subphase.DescriptionLong = "Select a faceup ship Crit damage card to flip it facedown";
                subphase.ImageSource = HostUpgrade;
                subphase.TheShip = TargetShip;
                subphase.Start();
            }
        }
        private class TheIronAssemblerDecisionSubphase : DecisionSubPhase { }
    }
}
namespace SubPhases
{
    public class IronAssemblerDecisionSubPhase : DecisionSubPhase
    {
        public override void PrepareDecision(System.Action callBack)
        {
            DecisionViewType = DecisionViewTypes.ImagesDamageCard;

            foreach (var shipCrit in TheShip.Damage.GetFaceupCrits().ToList())
            {
                AddDecision(shipCrit.Name, delegate { DiscardCrit(shipCrit); }, shipCrit.ImageUrl);
            }

            DefaultDecisionName = GetDecisions().First().Name;

            callBack();
        }

        private void DiscardCrit(GenericDamageCard critCard)
        {
            Selection.ActiveShip.Damage.FlipFaceupCritFacedown(critCard, Phases.CurrentSubPhase.CallBack);
            Sounds.PlayShipSound("R2D2-Proud");
        }
    }
}
