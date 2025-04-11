using Abilities.SecondEdition;
using Content;
using System.Collections.Generic;
using Ship;
using Upgrade;
using BoardTools;
using System;
using SubPhases;
using UnityEngine;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class BraylenStrammBoELSL : ASF01BWing
        {
            public BraylenStrammBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Braylen Stramm",
                    4,
                    53,
                    isLimited: true,
                    abilityType: typeof(BraylenStrammBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    charges: 2,
                    regensCharges: 1,
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipAbilities.Add(new GyroCockpit());
                ModelInfo.SkinName = "Dark Blue";
                PilotNameCanonical = "braylenstramm-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BraylenStrammBattleOverEndorAbility : GenericAbility
    {
        //At the start of the Engagement Phase, if a friendly Gina Moonsong ship at range 0-2 is stressed, you may gain a focus token.
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= CheckAbility;
        }

        private void CheckAbility()
        {
            List<GenericShip> friendlyShipsAtRange = Board.GetShipsAtRange(HostShip, new Vector2(0, 2), Team.Type.Friendly);

            foreach (GenericShip ship in friendlyShipsAtRange)
            {
                if (ship.PilotInfo.PilotName.Equals("Gina Moonsong") && ship.IsStressed)
                {
                    RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, CheckUseAbility);
                    break;
                }
            }
        }

        private void CheckUseAbility(object sender, EventArgs e)
        {
            if (!alwaysUseAbility)
            {
                AskToUseAbility
                (
                    descriptionShort: HostShip.PilotInfo.PilotName,
                    descriptionLong: "Gina Moonsong is stressed, do you want to gain a focus token?",
                    useByDefault: AlwaysUseByDefault,
                    useAbility: GainFocusToken,
                    callback: Triggers.FinishTrigger,
                    showAlwaysUseOption: true,
                    imageHolder: HostShip,
                    showSkipButton: false
                );
            } 
            else
            {
                HostShip.Tokens.AssignToken(new Tokens.FocusToken(HostShip), Triggers.FinishTrigger);
            }
        }

        private void GainFocusToken(object sender, EventArgs e)
        {
            HostShip.Tokens.AssignToken(new Tokens.FocusToken(HostShip), DecisionSubPhase.ConfirmDecision);
        }
    }
}
