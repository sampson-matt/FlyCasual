using Abilities.Parameters;
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
        public class SeventhSkyCorpsPilot : CloneZ95Headhunter
        {
            public SeventhSkyCorpsPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "7th Sky Corps Pilot",
                    2,
                    22
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/7thskycorpspilot.png";
            }
        }
    }
}