using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.LaatIGunship
    {
        public class Hawk : LaatIGunship
        {
            public Hawk() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Hawk\"",
                    4,
                    50,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.HawkAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class HawkAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new AtTheStartOfPhase(typeof(SubPhases.EndStartSubPhase));

        public override AbilityPart Action => new EachShipCanDoAction
        (
            eachShipAction: PerformReposition,
            conditions: new ConditionsBlock
            (
                new TeamCondition(ShipTypes.Friendly),
                new RangeToHostCondition(minRange: 0, maxRange: 1),
                new RevealedManeuverCondition(minSpeed: 3, maxSpeed: 5)
            ),
            description: new AbilityDescription
            (
                "\"Hawk\"",
                "Friendly ships may gain 1 strain token to perfrom a Barrel Roll or Boost action",
                imageSource: HostShip
            )
        );

        private void PerformReposition(GenericShip ship, Action callback)
        {
            Selection.ChangeActiveShip(ship);

            ship.Tokens.AssignToken
            (
                typeof(Tokens.StrainToken),
                delegate
                {
                    ship.AskPerformFreeAction
                    (
                        new List<GenericAction>()
                        { 
                            new BarrelRollAction(){ HostShip = ship },
                            new BoostAction(){ HostShip = ship },
                        },
                        callback,
                        descriptionShort: "\"Hawk\"",
                        descriptionLong: "You can perform barrel roll or boost action",
                        imageHolder: ship
                    );
                }
            );
        }
    }
}