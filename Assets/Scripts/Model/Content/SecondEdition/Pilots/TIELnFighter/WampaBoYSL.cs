using Upgrade;
using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class WampaBoYSL : TIELnFighter
        {
            public WampaBoYSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Wampa\"",
                    1,
                    39,
                    isLimited: true,
                    abilityType: typeof(WampaAbility),
                    charges: 1,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    regensCharges: 1,
                    isStandardLayout: true
                );
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(TargetLockAction)));
                ShipInfo.Hull++;
                PilotNameCanonical = "wampa-battleofyavin-sl";
                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/0/0c/Wampa-battleofyavin.png";

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Elusive));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Vengeful));
            }
        }
    }
}