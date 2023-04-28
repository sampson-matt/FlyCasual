using Abilities.SecondEdition;
using ActionsList;
using Ship;
using System;
using System.Linq;

namespace Ship
{
    namespace SecondEdition.ResistanceTransport
    {
        public class TakaJamoreesa : ResistanceTransport
        {
            public TakaJamoreesa() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Taka Jamoreesa",
                    2,
                    31,
                    isLimited: true,
                    abilityText: "After you jam, you must assign 1 jam token to another ship at range 0-1 of the jammed ship, if able.",
                    abilityType: typeof(TakaJamoreesaAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TakaJamoreesaAbility : GenericAbility
    {
        GenericShip JammedShip;
        public override void ActivateAbility()
        {
            HostShip.OnJamTargetIsSelected += CheckAssignJam;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnJamTargetIsSelected -= CheckAssignJam;
        }

        private void CheckAssignJam(GenericShip ship)
        {
            if(BoardTools.Board.GetShipsAtRange(ship, new UnityEngine.Vector2(0,1), Team.Type.Any).Count > 1)
            {
                JammedShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskToJam);
            }
        }
        private void AskToJam(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                ShipIsSelected,
                FilterTargets,
                GetAiPriority,
                HostShip.Owner.PlayerNo,
                name: HostShip.PilotInfo.PilotName,
                description: "You must to assign a Jam token to ship at range 0-1 of "+JammedShip.PilotInfo.PilotName,
                imageSource: HostShip,
                showSkipButton: false
            );
        }

        private void ShipIsSelected()
        {
            SubPhases.SelectShipSubPhase.FinishSelectionNoCallback();
            TargetShip.Tokens.AssignToken(
                new Tokens.JamToken(TargetShip, HostShip.Owner),
                Triggers.FinishTrigger
            );
        }

        private bool FilterTargets(GenericShip ship)
        {
            return BoardTools.Board.GetShipsAtRange(JammedShip, new UnityEngine.Vector2(0, 1), Team.Type.Any).Contains(ship) && ship.ShipId != JammedShip.ShipId;
        }

        private int GetAiPriority(GenericShip ship)
        {
            int teamModifier = (Tools.IsSameTeam(ship, HostShip)) ? 1 : 10;
            int tokensModifier = (teamModifier == 1) ? 0 : ship.Tokens.GetAllTokens().Count(n => n.TokenColor == Tokens.TokenColors.Green) * 10;
            int shipCostPriority = (teamModifier == 1) ? 200 - ship.PilotInfo.Cost : 200 + ship.PilotInfo.Cost;
            return (shipCostPriority + tokensModifier) * teamModifier;
        }
    }
}
