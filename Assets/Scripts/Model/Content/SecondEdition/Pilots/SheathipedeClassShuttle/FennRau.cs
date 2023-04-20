using System.Collections;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.SheathipedeClassShuttle
    {
        public class FennRau : SheathipedeClassShuttle
        {
            public FennRau() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Fenn Rau",
                    6,
                    46,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.FennRauRebelAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "fennrau-sheathipedeclassshuttle";
            }
        }
    }
}
