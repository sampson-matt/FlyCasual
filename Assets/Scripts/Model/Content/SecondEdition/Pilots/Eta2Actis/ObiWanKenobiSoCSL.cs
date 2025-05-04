using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Content;
using ActionsList;
using Upgrade;

namespace Ship.SecondEdition.Eta2Actis
{
    public class ObiWanKenobiSoCSL : Eta2Actis
    {
        public ObiWanKenobiSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Obi-Wan Kenobi",
                5,
                52,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.ObiWanKenobiSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.Jedi,
                    Tags.LightSide
                },
                extraUpgradeIcons: new List<UpgradeType>
                {
                    UpgradeType.Talent,
                    UpgradeType.Cannon
                },
                isStandardLayout: true
            );

            ModelInfo.SkinName = "Blue";

            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Patience));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R4P17SoC));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AncillaryIonWeaponsSoC));

            PilotNameCanonical = "obiwankenobi-siegeofcoruscant";
        }
    }
}