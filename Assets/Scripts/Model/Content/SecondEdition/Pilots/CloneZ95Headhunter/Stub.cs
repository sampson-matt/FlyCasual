﻿using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.CloneZ95Headhunter
    {

        public class Stub : CloneZ95Headhunter
        {
            public Stub() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Stub\"",
                    3,
                    31,
                    pilotTitle: "Scrappy Flier",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.StubAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class StubAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfPrimaryWeaponAttackDice += CheckAttackAbility;
            HostShip.AfterGotNumberOfDefenceDice += CheckDefensebility;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfPrimaryWeaponAttackDice -= CheckAttackAbility;
            HostShip.AfterGotNumberOfDefenceDice -= CheckDefensebility;
        }

        private void CheckAttackAbility(ref int count)
        {
            if (HostShip.RevealedManeuver == null) return;

            if (HostShip.RevealedManeuver.Speed % 2 != 0)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": +1 attack die");
                count++;
            }
        }

        private void CheckDefensebility(ref int count)
        {
            if (HostShip.RevealedManeuver == null) return;

            if (HostShip.RevealedManeuver.Speed % 2 == 0)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": +1 defense die");
                count++;
            }
        }
    }
}