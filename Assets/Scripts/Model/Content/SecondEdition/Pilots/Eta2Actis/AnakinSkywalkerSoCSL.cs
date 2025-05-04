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
    public class AnakinSkywalkerSoCSL : Eta2Actis
    {
        public AnakinSkywalkerSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Anakin Skywalker",
                6,
                66,
                true,
                force: 3,
                abilityType: typeof(Abilities.SecondEdition.AnakinSkywalkerSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.DarkSide,
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcons: new List<UpgradeType>
                {
                    UpgradeType.Talent,
                    UpgradeType.Cannon
                },
                isStandardLayout: true
            );

            ModelInfo.SkinName = "Yellow";

            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Malice));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R2D2Republic));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AncillaryIonWeaponsSoC));

            PilotNameCanonical = "anakinskywalker-siegeofcoruscant";
        }
    }
}


