
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class Chopper : GauntletFighter
        {
            public Chopper() : base()
            {

                PilotInfo = new PilotCardInfo
                (
                    "\"Chopper\"",
                    2,
                    51,
                    pilotTitle: "Spectre-3",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ChopperPilotAbility),
                    factionOverride: Faction.Rebel
                );

                ModelInfo.SkinName = "Red";

                ShipInfo.ActionIcons.SwitchToDroidActions();

                PilotNameCanonical = "chopper-gauntletfighter";
            }
        }
    }
}