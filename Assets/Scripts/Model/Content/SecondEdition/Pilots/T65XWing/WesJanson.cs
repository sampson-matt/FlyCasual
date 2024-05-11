using System.Collections.Generic;
using System;
using Upgrade;
using Content;
using Abilities.Parameters;
using Tokens;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class WesJanson : T65XWing
        {
            public WesJanson() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Wes Janson",
                    5,
                    49,
                    charges: 1,
                    regensCharges: 1,
                    pilotTitle: "Wisecracking Wingman",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.WesJansonAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class WesJansonAbility : CombinedAbility
    {
        public override List<Type> CombinedAbilities => new List<Type>()
        {
            typeof(WesJansonAttackAbility),
            typeof(WesJansonDefenseAbility)
        };
    }

    public class WesJansonAttackAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new AfterYouPerformAttack();

        public override AbilityPart Action => new AskToUseAbilityAction
        (
            description: new AbilityDescription
            (
                "Wes Janson",
                "Do you want do assign the defender 1 jam token?",
                HostShip
            ),
            onYes: new SpendPilotChargeAction
            (
                next: new AssignTokenAction
                (
                    tokenType: typeof(JamToken),
                    targetShipRole: ShipRole.Defender,
                    showMessage: ShowJamDefenderMessage
                )
            ),
            conditions: new ConditionsBlock
            (
                new HasPilotChargesAbility(1)
            ),
            aiUseByDefault: AlwaysUseByDefault
        );

        private string ShowJamDefenderMessage()
        {
            return $"{HostShip.PilotInfo.PilotName}: Jam token is assigned to {Combat.Defender.PilotInfo.PilotName}";
        }
    }

    public class WesJansonDefenseAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new AfterYouDefend();

        public override AbilityPart Action => new AskToUseAbilityAction
        (
            description: new AbilityDescription
            (
                "Wes Janson",
                "Do you want do assign the attacker 1 jam token?",
                HostShip
            ),
            onYes: new SpendPilotChargeAction
            (
                next: new AssignTokenAction
                (
                    tokenType: typeof(JamToken),
                    targetShipRole: ShipRole.Attacker,
                    showMessage: ShowJamAttackerMessage
                )
            ),
            conditions: new ConditionsBlock
            (
                new HasPilotChargesAbility(1)
            ),
            aiUseByDefault: AlwaysUseByDefault
        );

        private string ShowJamAttackerMessage()
        {
            return $"{HostShip.PilotInfo.PilotName}: Jam token is assigned to {Combat.Attacker.PilotInfo.PilotName}";
        }
    }
}
