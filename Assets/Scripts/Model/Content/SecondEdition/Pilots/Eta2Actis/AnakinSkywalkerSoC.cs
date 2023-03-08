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
    public class AnakinSkywalkerSoC : Eta2Actis
    {
        public AnakinSkywalkerSoC()
        {
            PilotInfo = new PilotCardInfo(
                "Anakin Skywalker",
                6,
                51,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.AnakinSkywalkerSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.DarkSide,
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.Talent
            );

            ModelInfo.SkinName = "Yellow";

            PilotNameCanonical = "anakinskywalker-siegeofcoruscant";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/anakinskywalker-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AnakinSkywalkerSoCAbility : GenericAbility
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
            if (((IsMe(ship)) || (IsObiWanKenobiInRange(ship)))
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

        private bool IsObiWanKenobiInRange(GenericShip ship)
        {
            bool result = false;

            if (ship.PilotInfo.PilotName == "Obi-Wan Kenobi" && Tools.IsFriendly(ship, HostShip))
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
                        new BarrelRollAction()
                    },
                    CleanUp,
                    HostShip.PilotInfo.PilotName,
                    "Anakin Skywalker: You may spend 1 Force to perform a Barrel Roll.",
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
