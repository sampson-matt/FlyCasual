using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESeBomber
    {
        public class Grudge : TIESeBomber
        {
            public Grudge() : base()
            {
                IsWIP = true;

                PilotInfo = new PilotCardInfo
                (
                    "\"Grudge\"",
                    2,
                    38,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.GrudgePilotAbility)
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GrudgePilotAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}
