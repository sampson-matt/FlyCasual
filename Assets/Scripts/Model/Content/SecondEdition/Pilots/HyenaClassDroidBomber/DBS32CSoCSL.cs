using Actions;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship.SecondEdition.HyenaClassDroidBomber
{
    public class DBS32CSoCSL : HyenaClassDroidBomber
    {
        public DBS32CSoCSL()
        {
            PilotInfo = new PilotCardInfo(
                "DBS-32C",
                3,
                38,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.DBS32CSoCAbility),
                charges: 2,
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.SL,
                    Tags.Droid
                },
                extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Torpedo, UpgradeType.Configuration },
                pilotTitle: "Siege of Coruscant",
                isStandardLayout: true
            );

            ShipInfo.ActionIcons.RemoveActions(typeof(ReloadAction));
            ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(JamAction), ActionColor.Red));

            MustHaveUpgrades.Add(typeof(PlasmaTorpedoes));
            MustHaveUpgrades.Add(typeof(ContingencyProtocolSoC));
            MustHaveUpgrades.Add(typeof(StrutLockOverride));

            PilotNameCanonical = "dbs32c-siegeofcoruscant";
        }
    }
}