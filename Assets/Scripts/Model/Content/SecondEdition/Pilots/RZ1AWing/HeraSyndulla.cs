using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class HeraSyndulla : RZ1AWing
        {
            public HeraSyndulla() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hera Syndulla",
                    6,
                    47,
                    pilotTitle: "Phoenix Leader",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.HeraSyndullaABWingAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent }
                );

                PilotNameCanonical = "herasyndulla-rz1awing";

                ModelInfo.SkinName = "Hera Syndulla";
            }
        }
    }
}