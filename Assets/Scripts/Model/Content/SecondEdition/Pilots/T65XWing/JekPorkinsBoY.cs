using Abilities.SecondEdition;
using System.Collections.Generic;
using Content;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class JekPorkinsBoY : T65XWing
        {
            public JekPorkinsBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Jek Porkins",
                    4,
                    42,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoY,
                        Tags.LsL
                    },
                    abilityType: typeof(JekPorkinsAbility)
                );
                ShipAbilities.Add(new HopeAbility());
                PilotNameCanonical = "jekporkins-battleofyavin-lsl";
                ModelInfo.SkinName = "Jek Porkins";
            }
        }
    }
}