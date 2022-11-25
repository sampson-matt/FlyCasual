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
        public class MandalorianRoyalGuard : FangFighter
        {
            public MandalorianRoyalGuard() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Mandalorian Royal Guard",
                    4,
                    51,
                    pilotTitle: "Selfless Protector",
                    limited: 2,
                    abilityType: typeof(Abilities.SecondEdition.MandalorianRoyalGuardAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    }
                );
                ModelInfo.SkinName = "Skull Squadron Pilot";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/mandalorianroyalguard.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MandalorianRoyalGuardAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }
    }
}