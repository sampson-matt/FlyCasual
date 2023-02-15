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
                    44,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(JekPorkinsAbility)
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/jekporkins-boy.png";
                PilotNameCanonical = "jekporkins-battleofyavin";
                ModelInfo.SkinName = "Jek Porkins";
            }
        }
    }
}