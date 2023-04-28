using System;
using System.Collections.Generic;
using System.Linq;
using Content;

namespace Ship.SecondEdition.Eta2Actis
{
    public class JediGeneral : Eta2Actis
    {
        public JediGeneral()
        {
            PilotInfo = new PilotCardInfo(
                "Jedi General",
                4,
                41,
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                force: 2
            );

            ModelInfo.SkinName = "Blue";
        }
    }
}