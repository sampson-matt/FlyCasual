using Abilities.SecondEdition;
using ActionsList;
using Ship;
using SubPhases;
using Content;
using System.Collections.Generic;
using Upgrade;
using Tokens;
using System;
using BoardTools;

namespace Ship
{
    namespace SecondEdition.NabooRoyalN1Starfighter
    {
        public class RhysDallows : NabooRoyalN1Starfighter
        {
            public RhysDallows() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Rhys Dallows",
                    6,
                    45,
                    isLimited: true,
                    abilityText: "When you or a friendly Rhys Dallows at range 0-3 would receive a green token, you may spend 1 [Charge]. If you do, that ship may perform a [Barrel Roll] action instead.",
                    abilityType: typeof(RhysDallowsAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit }
                );

                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/X2PO-homebrewPilot-watrhysdallowsv23.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class RhysDallowsAbility : GenericAbility
    {
        GenericToken GreenToken;
        public override void ActivateAbility()
        {
            GenericShip.BeforeTokenIsAssignedGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.BeforeTokenIsAssignedGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship, GenericToken token)
        {
            if (token.TokenColor == TokenColors.Green
                && (IsMe(ship) || IsEssaraTillInRange(ship)))
            {
                TargetShip = ship;
                GreenToken = token;
                RegisterAbilityTrigger(TriggerTypes.OnBeforeTokenIsAssigned, AskPerformFreeAction);
            }
        }

        private void AskPerformFreeAction(object sender, EventArgs e)
        {
            if (TargetShip.Tokens.TokenToAssign != null)
            {
                TargetShip.BeforeActionIsPerformed += PrepareRemoveToken;

                TargetShip.AskPerformFreeAction
                (
                    new List<GenericAction>()
                    {
                        new BarrelRollAction()
                    },
                    CleanUp,
                    HostShip.PilotInfo.PilotName,
                    $"Rhys Dallows: You may spend 1 charge to allow {TargetShip.PilotInfo.PilotName} to perform a Barrel Roll action instead of receiving a {GreenToken.Name}?",
                    HostUpgrade
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void PrepareRemoveToken(GenericAction action, ref bool isFreeAction)
        {
            TargetShip.BeforeActionIsPerformed -= PrepareRemoveToken;

            RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, RemoveToken);
        }

        private void RemoveToken(object sender, EventArgs e)
        {
            TargetShip.Tokens.TokenToAssign = null;
            Triggers.FinishTrigger();
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= PrepareRemoveToken;
            Triggers.FinishTrigger();
        }

        private bool IsEssaraTillInRange(GenericShip ship)
        {
            bool result = false;

            if (ship.PilotInfo.PilotName == "Essara Till" && Tools.IsFriendly(ship, HostShip))
            {
                DistanceInfo distanceInfo = new DistanceInfo(HostShip, ship);
                if (distanceInfo.Range <= 3)
                {
                    result = true;
                }
            }

            return result;
        }

        private bool IsMe(GenericShip ship)
        {
            return ship.ShipId == HostShip.ShipId;
        }




    }
}
