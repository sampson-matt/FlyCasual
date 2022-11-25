using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.FangFighter
    {
        public class ClanWrenVolunteer : FangFighter
        {
            public ClanWrenVolunteer() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Clan Wren Volunteer",
                    3,
                    44,
                    pilotTitle: "Unlikely Ally",
                    limited: 2,
                    abilityType: typeof(Abilities.SecondEdition.ClanWrenVolunteerAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    factionOverride: Faction.Rebel
                );
                ModelInfo.SkinName = "Clan Wren Volunteers";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/clanwrenvolunteer.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ClanWrenVolunteerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}