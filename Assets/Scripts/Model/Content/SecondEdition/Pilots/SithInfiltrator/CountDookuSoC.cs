using System.Collections.Generic;
using Upgrade;
using ActionsList;
using Actions;
using Content;

namespace Ship.SecondEdition.SithInfiltrator
{
    public class CountDookuSoC : SithInfiltrator
    {
        public CountDookuSoC()
        {
            PilotInfo = new PilotCardInfo(
                "Count Dooku",
                5,
                64,
                true,
                abilityType: typeof(Abilities.SecondEdition.CountDookuCrewAbility),
                tags: new List<Tags>
                {
                    Tags.SoC,
                    Tags.LsL,
                    Tags.DarkSide,
                    Tags.Sith
                },
                pilotTitle: "Siege of Coruscant",
                force: 3,
                extraUpgradeIcon: UpgradeType.ForcePower
            );
            ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);
            ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(CloakAction), ActionColor.Red), new ActionInfo(typeof(JamAction)));

            PilotNameCanonical = "countdooku-siegeofcoruscant-lsl";
        }
    }
}

