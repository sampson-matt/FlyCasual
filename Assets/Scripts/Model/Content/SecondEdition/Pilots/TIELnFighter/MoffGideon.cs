using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;
using System;
using Tokens;
using Editions;
using SubPhases;
using Abilities.SecondEdition;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class MoffGideon : TIELnFighter
        {
            public MoffGideon() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Moff Gideon",
                    4,
                    31,
                    pilotTitle: "Ruthless Remnant Leader",
                    isLimited: true,
                    abilityType: typeof(MoffGideonAbility),
                    charges: 2,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent }
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/moffgideon.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MoffGideonAbility : GenericAbility
    {

        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}