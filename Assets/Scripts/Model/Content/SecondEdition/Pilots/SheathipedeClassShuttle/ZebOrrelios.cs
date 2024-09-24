using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.SheathipedeClassShuttle
    {
        public class ZebOrrelios : SheathipedeClassShuttle
        {
            public ZebOrrelios() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Zeb\" Orrelios",
                    2,
                    31,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.ZebOrreliosPilotAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "zeborrelios-sheathipedeclassshuttle";
            }
        }
    }
}
