using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using ActionsList;
using Upgrade;

namespace Ship.SecondEdition.Eta2Actis
{
    public class ObiWanKenobiSoC : Eta2Actis
    {
        public ObiWanKenobiSoC()
        {
            PilotInfo = new PilotCardInfo(
                "Obi-Wan Kenobi",
                5,
                47,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.ObiWanKenobiSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC
                },
                extraUpgradeIcon: UpgradeType.Talent
            );

            ModelInfo.SkinName = "Blue";

            PilotNameCanonical = "obiwankenobi-siegeofcoruscant";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/obiwankenobi-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ObiWanKenobiSoCAbility : GenericAbility
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
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskPerformFreeAction);
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

        private void AskPerformFreeAction(object sender, EventArgs e)
        {
            if (HostShip.State.Force > 0 && HostShip.IsCanUseForceNow())
            {
                HostShip.BeforeActionIsPerformed += PayForceToken;

                TriggedShip.AskPerformFreeAction
                (
                    new List<GenericAction>()
                    {
                        new BoostAction()
                    },
                    CleanUp,
                    HostShip.PilotInfo.PilotName,
                    "Obi-Wan Kenobi: You may spend 1 Force to perform a Boost.",
                    HostUpgrade
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void PayForceToken(GenericAction action, ref bool isFreeAction)
        {
            HostShip.BeforeActionIsPerformed -= PayForceToken;

            RegisterAbilityTrigger(TriggerTypes.BeforeActionIsPerformed, SpendForce);
        }

        private void SpendForce(object sender, EventArgs e)
        {
            HostShip.State.SpendForce(1, Triggers.FinishTrigger);
        }

        private void CleanUp()
        {
            HostShip.BeforeActionIsPerformed -= PayForceToken;
            Triggers.FinishTrigger();
        }
    }
}
