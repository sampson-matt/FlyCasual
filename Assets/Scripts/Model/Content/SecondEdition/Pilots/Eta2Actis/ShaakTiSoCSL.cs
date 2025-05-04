using Abilities.Parameters;
using Ship;
using SubPhases;
using System;
using Actions;
using ActionsList;
using Upgrade;
using Content;
using System.Collections.Generic;
using Tokens;
using System.Linq;

namespace Ship.SecondEdition.Eta2Actis
{
    public class ShaakTiSoCSL : Eta2Actis
    {
        public ShaakTiSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "Shaak Ti",
                4,
                49,
                true,
                force: 2,
                abilityType: typeof(Abilities.SecondEdition.ShaakTiSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
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
            PilotNameCanonical = "shaakti-siegeofcoruscant";

            ModelInfo.SkinName = "Red";

            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Marksmanship));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.BrilliantEvasion));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.AncillaryIonWeaponsSoC));
            MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R4PAstromech));
        }
    }
}