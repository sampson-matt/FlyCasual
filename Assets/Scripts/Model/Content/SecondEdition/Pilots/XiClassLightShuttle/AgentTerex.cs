using System.Collections;
using System.Collections.Generic;
using Upgrade;
using Abilities.Parameters;
using System;

namespace Ship
{
    namespace SecondEdition.XiClassLightShuttle
    {
        public class AgentTerex : XiClassLightShuttle
        {
            public AgentTerex() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Agent Terex",
                    3,
                    36,
                    isLimited: true,
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Illicit,
                        UpgradeType.Illicit,
                        UpgradeType.Illicit
                    },
                    abilityType: typeof(Abilities.SecondEdition.AgentTerexPilotAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AgentTerexPilotAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new AfterPlacingForces();

        public override AbilityPart Action => new EachUpgradeCanDoAction
        (
            eachUpgradeAction: new SelectShipAction
            (
                action: new TransferUpgradeAction(),
                conditions: new ConditionsBlock
                (
                    new ShipTypeCondition
                    (
                        typeof(Ship.SecondEdition.TIEFoFighter.TIEFoFighter),
                        typeof(Ship.SecondEdition.TIESfFighter.TIESfFighter)
                    )
                ),
                abilityDescription: new AbilityDescription
                (
                    "Agent Terex",
                    "Select a ship to equip",
                    imageSource: HostShip
                ),
                aiSelectShipPlan: new AiSelectShipPlan
                (
                    AiSelectShipTeamPriority.Friendly,
                    AiSelectShipSpecial.None
                )
            ),
            conditions: new ConditionsBlock
            (
                new UpgradeTypeCondition(UpgradeType.Illicit)
            )
        );
    }
}

