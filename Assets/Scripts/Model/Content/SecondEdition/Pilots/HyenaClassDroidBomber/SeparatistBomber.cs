using System.Collections.Generic;
using Upgrade;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class SeparatistBomber : HyenaClassDroidBomber
    {
        public SeparatistBomber()
        {
            PilotInfo = new PilotCardInfo(
                "Separatist Bomber",
                3,
                28,
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Torpedo, UpgradeType.Missile, UpgradeType.Device }
            );
        }
    }
}