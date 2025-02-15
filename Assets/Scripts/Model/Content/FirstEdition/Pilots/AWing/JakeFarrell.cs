﻿using ActionsList;
using Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.AWing
    {
        public class JakeFarrell : AWing
        {
            public JakeFarrell() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Jake Farrell",
                    7,
                    24,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.JakeFarrellAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class JakeFarrellAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnTokenIsAssigned += RegisterJakeFarrellAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsAssigned -= RegisterJakeFarrellAbility;
        }

        private void RegisterJakeFarrellAbility(GenericShip ship, GenericToken token)
        {
            if (token is FocusToken)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, FreeRepositionAction);
            }
        }

        private void FreeRepositionAction(object sender, EventArgs e)
        {
            GenericShip originalSelectedShip = Selection.ThisShip;
            Selection.ThisShip = HostShip;
            List<GenericAction> actions = new List<GenericAction>() { new BoostAction(), new BarrelRollAction() };
            HostShip.AskPerformFreeAction(
                actions,
                delegate ()
                {
                    Selection.ThisShip = originalSelectedShip;
                    Triggers.FinishTrigger();
                },
                HostShip.PilotInfo.PilotName,
                "After you perform a Focus action or are assigned a Focus Token, you may perform a free Boost or Barrel Roll action",
                HostShip
            );
        }
    }
}