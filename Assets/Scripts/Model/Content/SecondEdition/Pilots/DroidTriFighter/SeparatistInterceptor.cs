using Content;
using System.Collections.Generic;
using Upgrade;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class SeparatistInterceptor : DroidTriFighter
    {
        public SeparatistInterceptor()
        {
            PilotInfo = new PilotCardInfo(
                "Separatist Interceptor",
                3,
                35,
                extraUpgradeIcon: UpgradeType.Talent,
                tags: new List<Tags>
                {
                    Tags.Droid
                }
            );
        }
    }
}