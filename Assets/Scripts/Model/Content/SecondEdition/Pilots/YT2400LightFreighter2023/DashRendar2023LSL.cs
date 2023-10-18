using System;
using System.Collections.Generic;
using Upgrade;
using Ship;
using Tokens;
using SubPhases;

namespace Ship
{
    namespace SecondEdition.YT2400LightFreighter2023
    {
        public class DashRendar2023LSL : YT2400LightFreighter2023
        {
            public DashRendar2023LSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Dash Rendar",
                    5,
                    73,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DashRendar2023SLAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent}
                );

                PilotNameCanonical = "dashrendar-swz103-lsl-rebelalliance";
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/YT2400/dashRendar-rebel.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DashRendar2023SLAbility : GenericAbility
    {
        //After you gain a red token as a result of moving through or overlapping an obstacle, you may transfer that red token to a friendly ship at range 0-1.

        private GenericToken transferToken;

        public override void ActivateAbility()
        {
            HostShip.OnRedTokenGainedFromOverlappingObstacle += CheckDashRendarAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnRedTokenGainedFromOverlappingObstacle -= CheckDashRendarAbility;
        }

        public void CheckDashRendarAbility(GenericShip host, GenericToken token)
        {
            if(BoardTools.Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0,1), Team.Type.Friendly).Count > 0)
            {
                transferToken = token;
                RegisterAbilityTrigger(TriggerTypes.OnRedTokenGainedFromOverlappingObstacle, SelectTargetForDashRendarAbility);
            }
            
        }

        private void SelectTargetForDashRendarAbility(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                TargetIsSelected,
                FilterTargets,
                GetAiPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                "After you gain a red token as a result of moving through or overlapping an obstacle, you may transfer that red token to a friendly ship at range 0-1",
                HostShip
            );
        }

        private bool FilterTargets(GenericShip ship)
        {
            return FilterByTargetType(ship, TargetTypes.OtherFriendly)
                && FilterTargetsByRange(ship, 0, 1);
        }

        private int GetAiPriority(GenericShip ship)
        {
            return 0;
        }

        private void TargetIsSelected()
        {
            HostShip.Tokens.RemoveToken(transferToken.GetType(), AssignFocusTokenToTarget);
        }

        private void AssignFocusTokenToTarget()
        {
            TargetShip.Tokens.AssignToken(transferToken.GetType(), SelectShipSubPhase.FinishSelection);
        }
    }
}
