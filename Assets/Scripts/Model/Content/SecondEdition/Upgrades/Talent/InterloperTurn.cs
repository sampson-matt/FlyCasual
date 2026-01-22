using Upgrade;
using Movement;
using Abilities.Parameters;

namespace UpgradesList.SecondEdition
{
    public class InterloperTurn : GenericUpgrade
    {
        public InterloperTurn() : base()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "Interloper Turn",
                UpgradeType.Talent,
                cost: 1,
                abilityType: typeof(Abilities.SecondEdition.InterloperTurnAbility),
                restriction: new ShipRestriction(typeof(Ship.SecondEdition.TIEDDefender.TIEDDefender))
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    // Before you execute a speed 1-2 Turn or speed 1-2 Koiogran K-Turn maneuver,
    // if you are at range 0-1 of an asteroid, structure, or huge ship, you may gain 1 tractor token.
    public class InterloperTurnAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new BeforeYouExecuteManeuver
        (
            conditions: new ConditionsBlock
            (
                new ManeuverSpeedCondition(1, 2),
                new ManeuverBearingCondition(ManeuverBearing.Turn, ManeuverBearing.KoiogranTurn),
                new AsteroidRangeCondition(0, 1)
            )
        );

        public override AbilityPart Action => new AskToUseAbilityAction
        (
            new AbilityDescription
            (
                HostUpgrade.UpgradeInfo.Name,
                "Do you want to gain tractor token?",
                HostUpgrade
            ),
            onYes: new AssignTokenAction
            (
                tokenType: typeof(Tokens.TractorBeamToken),
                targetShipRole: ShipRole.HostShip
            ),
            aiUseByDefault: NeverUseByDefault
        );
    }
}