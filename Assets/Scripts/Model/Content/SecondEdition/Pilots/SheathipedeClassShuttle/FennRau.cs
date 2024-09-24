using System.Collections;
using System.Collections.Generic;
using Upgrade;
using Content;

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
                    45,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.FennRauRebelAbility),
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "fennrau-sheathipedeclassshuttle";
            }
        }
    }
}
