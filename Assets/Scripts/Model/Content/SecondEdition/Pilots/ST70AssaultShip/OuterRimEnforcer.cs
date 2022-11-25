using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ST70AssaultShip
    {
        public class OuterRimEnforcer : ST70AssaultShip
        {
            public OuterRimEnforcer() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Outer Rim Enforcer",
                    2,
                    47
                );

                ModelInfo.SkinName = "Red Stripes";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/outerrimenforcer.png";
            }
        }
    }
}