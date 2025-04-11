using Abilities.FirstEdition;
using System.Collections.Generic;
using Content;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class GarvenDreisBoY : T65XWing
        {
            public GarvenDreisBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Garven Dreis",
                    4,
                    47,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.LsL
                    },
                    abilityType: typeof(GarvenDreisAbility)
                );
                ShipAbilities.Add(new Abilities.SecondEdition.HopeAbility());
                PilotNameCanonical = "garvendreis-battleofyavin-lsl";
            }
        }
    }
}