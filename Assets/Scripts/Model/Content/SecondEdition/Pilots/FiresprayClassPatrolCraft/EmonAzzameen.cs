using Bombs;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.FiresprayClassPatrolCraft
    {
        public class EmonAzzameen : FiresprayClassPatrolCraft
        {
            public EmonAzzameen() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Emon Azzameen",
                    4,
                    69,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.EmonAzzameenAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Crew },
                    seImageNumber: 150
                );
            }
        }
    }
}