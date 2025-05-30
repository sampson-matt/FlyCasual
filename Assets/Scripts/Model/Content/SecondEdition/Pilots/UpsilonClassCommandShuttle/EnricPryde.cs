using BoardTools;
using Ship;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.UpsilonClassCommandShuttle
    {
        public class EnricPryde : UpsilonClassCommandShuttle
        {
            public EnricPryde() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Enric Pryde",
                    3,
                    62,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.EnricPrydeAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class EnricPrydeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }

        

    }
}
