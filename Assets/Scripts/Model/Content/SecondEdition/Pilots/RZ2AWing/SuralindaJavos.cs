using Abilities.Parameters;
using Arcs;
using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ2AWing
    {
        public class SuralindaJavos  : RZ2AWing
        {
            public SuralindaJavos() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Suralinda Javos",
                    3,
                    34,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.SuralindaJavosAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent } 
                );

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class SuralindaJavosAbility : TriggeredAbility
    {
        public override TriggerForAbility Trigger => new AfterManeuver
        (
            onlyIfPartialExecuted: true
        );

        public override AbilityPart Action => new AskToUseAbilityAction
        (
            description: new AbilityDescription
            (
                name: "Suralinda Javos",
                description: "Do you want to gain Strain token to rotate 90 or 180 degrees?",
                imageSource: HostShip
            ),
            onYes: new AssignTokenAction
            (
                tokenType: typeof(StrainToken),
                targetShipRole: ShipRole.HostShip,
                showMessage: GetGainedStrainTokenMessage,
                afterAction: new AskToRotateShipAction
                (
                    description: new AbilityDescription
                    (
                        name: "Suralinda Javos",
                        description: "Choose how do you rotate",
                        imageSource: HostShip
                    ),
                    rotate90allowed: true,
                    rotate180allowed: true
                )
            )
        );

        private string GetGainedStrainTokenMessage()
        {
            return "Suralinda Javos: Gained strain token to rotate";
        }
    }
}