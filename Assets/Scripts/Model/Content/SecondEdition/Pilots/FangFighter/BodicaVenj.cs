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
        public class BodicaVenj : FangFighter
        {
            public BodicaVenj() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Bodica Venj",
                    4,
                    56,
                    pilotTitle: "Wrathful Warrior",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BodicaVenjAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    },
                    factionOverride: Faction.Rebel
                );
                ModelInfo.SkinName = "Bodica Venj";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/bodicavenj.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BodicaVenjAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}