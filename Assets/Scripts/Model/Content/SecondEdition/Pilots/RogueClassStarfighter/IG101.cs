﻿using System.Collections.Generic;
using Upgrade;
using System;
using System.Linq;
using SubPhases;
using Abilities.SecondEdition;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class IG101 : RogueClassStarfighter
        {
            public IG101() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "IG-101",
                    4,
                    39,
                    pilotTitle: "Tenacious Bodyguard",
                    isLimited: true,
                    abilityType: typeof(IG101Ability),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                DeadToRights oldAbility = (DeadToRights)ShipAbilities.First(n => n.GetType() == typeof(DeadToRights));
                oldAbility.DeactivateAbility();
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new NetworkedCalculationsAbility());
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class IG101Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnSystemsPhaseStart += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnSystemsPhaseStart -= RegisterAbility;
        }

        private void RegisterAbility(Ship.GenericShip ship)
        {
            if (HostShip.Damage.HasFaceupCards)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsPhaseStart, AskToUseOwnAbility);
            }
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);
            AskToUseAbility(
                HostShip.PilotName,
                AlwaysUseByDefault,
                UseOwnAbility,
                descriptionLong: "Do you want to repair 1 Faceup Damage Card?",
                imageHolder: HostShip
            );
        }

        private void UseOwnAbility(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            if (HostShip.Damage.GetFaceupCrits().Count == 1)
            {
                DoAutoRepair();
            }
            else
            {
                AskToSelectCrit();
            }
        }

        private void DoAutoRepair()
        {
            HostShip.Damage.FlipFaceupCritFacedown(HostShip.Damage.GetFaceupCrits().First(), Triggers.FinishTrigger);
        }

        private void AskToSelectCrit()
        {
            IG101DecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<IG101DecisionSubPhase>(
                "IG-101: Select faceup damage card",
                Triggers.FinishTrigger
            );

            subphase.DescriptionShort = "IG-101";
            subphase.DescriptionLong = "Select Faceup Damage Card to repair";
            subphase.ImageSource = HostShip;

            subphase.Start();
        }
    }
}

namespace SubPhases
{

    public class IG101DecisionSubPhase : DecisionSubPhase
    {

        public override void PrepareDecision(System.Action callBack)
        {
            DecisionViewType = DecisionViewTypes.ImagesDamageCard;

            foreach (var faceupCrit in Selection.ThisShip.Damage.GetFaceupCrits().ToList())
            {
                AddDecision(faceupCrit.Name, delegate { Repair(faceupCrit); }, faceupCrit.ImageUrl);
            }

            DefaultDecisionName = GetDecisions().First().Name;

            callBack();
        }

        private void Repair(GenericDamageCard critCard)
        {
            Selection.ThisShip.Damage.FlipFaceupCritFacedown(critCard, ConfirmDecision);
        }

    }

}