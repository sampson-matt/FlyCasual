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
        public class DirkUllodin : FangFighter
        {
            public DirkUllodin() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Dirk Ullodin",
                    3,
                    46,
                    pilotTitle: "Aspiring Commando",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DirkUllodinAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Modification
                    },
                    factionOverride: Faction.Rebel
                );
                ModelInfo.SkinName = "Dirk Ullodin";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/dirkullodin.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DirkUllodinAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}