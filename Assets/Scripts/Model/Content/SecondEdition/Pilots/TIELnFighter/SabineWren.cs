using ActionsList;
using Ship;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class SabineWren : TIELnFighter
        {
            public SabineWren() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sabine Wren",
                    3,
                    25,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.SabineWrenPilotAbility),
                    extraUpgradeIcon: UpgradeType.Talent,
                    factionOverride: Faction.Rebel
                );

                PilotNameCanonical = "sabinewren-tielnfighter";

                ModelInfo.ModelName = "TIE Fighter Rebel";
                ModelInfo.SkinName = "Rebel";
            }
        }
    }
}
