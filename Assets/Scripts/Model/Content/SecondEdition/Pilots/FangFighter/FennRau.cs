using Upgrade;
using System.Collections.Generic;
using Content;

namespace Ship
{
    namespace SecondEdition.FangFighter
    {
        public class FennRau : FangFighter
        {
            public FennRau() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Fenn Rau",
                    6,
                    69,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    abilityType: typeof(Abilities.FirstEdition.FennRauScumAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ModelInfo.SkinName = "Zealous Recruit";
            }
        }
    }
}