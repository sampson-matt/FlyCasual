using Content;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship.SecondEdition.Delta7Aethersprite
{
    public class JediKnight : Delta7Aethersprite
    {
        public JediKnight()
        {
            PilotInfo = new PilotCardInfo(
                "Jedi Knight",
                3,
                35,
                force: 1,
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.ForcePower
            );
        }
    }
}