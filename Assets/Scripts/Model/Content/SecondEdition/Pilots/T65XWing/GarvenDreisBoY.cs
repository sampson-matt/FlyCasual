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
                        Tags.BoY
                    },
                    abilityType: typeof(GarvenDreisAbility)
                );
                ShipAbilities.Add(new Abilities.SecondEdition.HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/garvendreis-boy.png";
                PilotNameCanonical = "garvendreis-battleofyavin";
            }
        }
    }
}