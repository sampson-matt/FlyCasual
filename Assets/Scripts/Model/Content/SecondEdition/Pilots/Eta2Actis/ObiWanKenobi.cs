using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Content;
using Upgrade;

namespace Ship.SecondEdition.Eta2Actis
{
    public class ObiWanKenobi : Eta2Actis
    {
        public ObiWanKenobi()
        {
            PilotInfo = new PilotCardInfo(
                "Obi-Wan Kenobi",
                5,
                49,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.ObiWanKenobiActisAbility),
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.Talent
            );

            ModelInfo.SkinName = "Blue";

            PilotNameCanonical = "obiwankenobi-eta2actis";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ObiWanKenobiActisAbility : GenericAbility
    {
        private GenericShip TriggedShip;

        public override void ActivateAbility()
        {
            GenericShip.OnMovementFinishGlobal += CheckFirstPartOfConditions;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnMovementFinishGlobal -= CheckFirstPartOfConditions;
        }

        private void CheckFirstPartOfConditions(GenericShip ship)
        {
            if (((IsMe(ship)) || (IsAnakinSkywalkerInRange(ship)))
                && HasMoreEnemyShipsInRange(ship)
            )
            {
                TriggedShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, CheckSecondPartOfConditions);
            }
        }

        private bool IsMe(GenericShip ship)
        {
            return ship.ShipId == HostShip.ShipId;
        }

        private bool IsAnakinSkywalkerInRange(GenericShip ship)
        {
            bool result = false;

            if (ship.PilotInfo.PilotName == "Anakin Skywalker" && Tools.IsFriendly(ship, HostShip))
            {
                DistanceInfo distanceInfo = new DistanceInfo(HostShip, ship);
                if (distanceInfo.Range <= 3)
                {
                    result = true;
                }
            }

            return result;
        }

        private bool HasMoreEnemyShipsInRange(GenericShip ship)
        {
            int anotherFriendlyShipsInRange = 0;
            int enemyShipsInRange = 0;

            foreach (GenericShip anotherShip in Roster.AllShips.Values)
            {
                if (anotherShip.ShipId == ship.ShipId) continue;

                DistanceInfo distInfo = new DistanceInfo(ship, anotherShip);
                if (distInfo.Range <= 1)
                {
                    if (Tools.IsFriendly(ship, anotherShip))
                    {
                        anotherFriendlyShipsInRange++;
                    }
                    else
                    {
                        enemyShipsInRange++;
                    }
                }
            }

            return enemyShipsInRange > anotherFriendlyShipsInRange;
        }

        private void CheckSecondPartOfConditions(object sender, EventArgs e)
        {
            if (HostShip.State.Force > 0)
            {
                AskToUseObiWanKenobisAbility();
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void AskToUseObiWanKenobisAbility()
        {
            AskToUseAbility(
                "Obi-Wan Kenobi",
                AlwaysUseByDefault,
                AssignFocusToken,
                descriptionLong: "Do you want to spend 1 force to assign 1 focus token?",
                imageHolder: HostShip,
                showSkipButton: true,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void AssignFocusToken(object sender, EventArgs e)
        {
            HostShip.State.SpendForce(
                1,
                delegate { TriggedShip.Tokens.AssignToken(typeof(FocusToken), DecisionSubPhase.ConfirmDecision); }
            );
        }
    }
}
