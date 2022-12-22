using Upgrade;
using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class WampaBoY : TIELnFighter
        {
            public WampaBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Wampa\"",
                    1,
                    30,
                    isLimited: true,
                    abilityType: typeof(WampaAbility),
                    charges: 1,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    regensCharges: 1
                );
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(TargetLockAction)));
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                PilotNameCanonical = "wampa-boy";
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/wampa-boy.png";
            }
        }
    }
}