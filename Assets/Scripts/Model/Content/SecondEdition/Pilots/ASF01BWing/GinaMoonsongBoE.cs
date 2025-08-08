using Abilities.SecondEdition;
using Actions;
using ActionsList;
using BoardTools;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using UnityEngine;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class GinaMoonsongBoE : ASF01BWing
        {
            public GinaMoonsongBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Gina Moonsong",
                    5,
                    80,
                    isLimited: true,
                    abilityType: typeof(GinaMoonsongBattleOverEndorAbility),
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Device
                    },
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,

                    charges: 2,
                    regensCharges: 1
                );
                ShipAbilities.Add(new GyroCockpit());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(TargetLockAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ReloadAction), ActionColor.Red));

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(Juke));
                MustHaveUpgrades.Add(typeof(ProtonTorpedoes));
                MustHaveUpgrades.Add(typeof(IonBombs));

                ModelInfo.SkinName = "Gina Moonsong";            
                PilotNameCanonical = "ginamoonsong-battleoverendor";
            }
        }
    }
}