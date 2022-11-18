using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class OuterRimHunter : RogueClassStarfighter
        {
            public OuterRimHunter() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Outer Rim Hunter",
                    3,
                    35
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/outerrimhunter.png";
            }
        }
    }
}