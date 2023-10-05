using Abilities.Parameters;
using ActionsList;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class CaiThrenalli : T70XWing
        {
            public CaiThrenalli() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "C’ai Threnalli",
                    4,
                    47,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaiThrenalliAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CaiThrenalliAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new AfterManeuver
        (
            onlyIfFullyExecuted: true,
            onlyIfMovedThroughFriendlyShip: true
        );

        public override AbilityPart Action => new AskToPerformAction
        (
            new AbilityDescription
            (
                "C’ai Threnalli",
                "You may perform an Evade action",
                HostShip
            ),
            new ActionInfo
            (
                typeof(EvadeAction)
            )
        );
    }
}