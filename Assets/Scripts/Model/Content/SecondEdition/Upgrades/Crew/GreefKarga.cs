using Ship;
using Upgrade;
using ActionsList;
using SubPhases;
using Actions;
using Tokens;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class GreefKarga : GenericUpgrade
    {
        public GreefKarga() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Greef Karga",
                UpgradeType.Crew,
                cost: 8,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Scum),
                addAction: new ActionInfo(typeof(CoordinateAction), ActionColor.Red),
                abilityType: typeof(Abilities.SecondEdition.GreefKargaCrewAbility)
            );

            ImageUrl = "https://infinitearenas.com/xw2/images/upgrades/greefkarga.png";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class GreefKargaCrewAbility : GenericAbility
    {
        GenericShip coorinatedShip;
        private List<BlueTargetLockToken> GreefLocks;
        public override void ActivateAbility()
        {
            HostShip.BeforeActionIsPerformed += CheckAbility;
            HostShip.OnCoordinateTargetIsSelected += CoordinateShipSelected;
        }
                
        public override void DeactivateAbility()
        {
            HostShip.BeforeActionIsPerformed -= CheckAbility;
            HostShip.OnCoordinateTargetIsSelected -= CoordinateShipSelected;
        }

        private void CoordinateShipSelected(GenericShip ship)
        {
            coorinatedShip = ship;
            coorinatedShip.OnActionIsPerformed += RegisterAbility;
        }

        private void RegisterAbility(GenericAction action)
        {
            coorinatedShip.OnActionIsPerformed -= RegisterAbility;
            if(HostShip.Tokens.GetTokens<BlueTargetLockToken>('*').Count >0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskToUseOwnAbility);
            }
            
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {

            GreefLocks = HostShip.Tokens.GetAllTokens()
                .Where(n => n is BlueTargetLockToken)
                .Select(n => n as BlueTargetLockToken)
                .ToList();

            if (GreefLocks.Count == 0)
            {
                Triggers.FinishTrigger();
            }
            else
            {
                AskToUseAbility(
                    "Greef Karga",
                    AlwaysUseByDefault,
                    AcquireTargetLock,
                    descriptionLong: "Do you want to acquire a lock on a ship " + HostShip.PilotInfo.PilotName + " has locked?",
                    showSkipButton: true,
                    requiredPlayer: HostShip.Owner.PlayerNo
                );
            }
        }

        private void AcquireTargetLock(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            if (GreefLocks.Count == 1)
            {
                ActionsHolder.AcquireTargetLock(
                    coorinatedShip,
                    GreefLocks.First().OtherTargetLockTokenOwner,
                    Triggers.FinishTrigger,
                    Triggers.FinishTrigger
                );
            }
            else
            {
                SelectTargetForAbility(
                    AcquireTargetLockOnSelectedShip,
                    FilterTargets,
                    GetAiPriority,
                    HostShip.Owner.PlayerNo,
                    name: "Greef Karga",
                    description: "You may lock any of " + HostShip.PilotInfo.PilotName + "'s target(s)",
                    showSkipButton: true
                );
            }
        }

        private int GetAiPriority(GenericShip ship)
        {
            return ship.PilotInfo.Cost;
        }

        private bool FilterTargets(GenericShip ship)
        {
            return GreefLocks.Any(n => n.OtherTargetLockTokenOwner == ship as ITargetLockable);
        }

        private void AcquireTargetLockOnSelectedShip()
        {
            SubPhases.SelectShipSubPhase.FinishSelectionNoCallback();

            ActionsHolder.AcquireTargetLock(
                coorinatedShip,
                TargetShip,
                Triggers.FinishTrigger,
                Triggers.FinishTrigger
            );
        }

        private void CheckAbility(GenericAction action, ref bool isFree)
        {
            if (action is CoordinateAction)
            {
                HostShip.OnCheckCoordinateModeModification += SetCustomCoordinateMode;
            }
        }

        private void SetCustomCoordinateMode(ref CoordinateActionData coordinateActionData)
        {
            coordinateActionData.canCoordinateAllied = true;

            HostShip.OnCheckCoordinateModeModification -= SetCustomCoordinateMode;
        }
    }
}