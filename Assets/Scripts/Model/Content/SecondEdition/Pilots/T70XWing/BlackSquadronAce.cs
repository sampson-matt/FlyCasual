using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class BlackSquadronAce : T70XWing
        {
            public BlackSquadronAce() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Black Squadron Ace",
                    4,
                    46,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "blacksquadronace-t70xwing";

                ModelInfo.SkinName = "Black One";
            }
        }
    }
}
