using Arcs;
using Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class TechnoUnionBomber : HyenaClassDroidBomber
    {
        public TechnoUnionBomber()
        {
            PilotInfo = new PilotCardInfo(
                "Techno Union Bomber",
                1,
                26,
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Torpedo, UpgradeType.Missile, UpgradeType.Device },
                tags: new List<Tags>
                {
                    Tags.Droid
                }
            );
        }
    }
}