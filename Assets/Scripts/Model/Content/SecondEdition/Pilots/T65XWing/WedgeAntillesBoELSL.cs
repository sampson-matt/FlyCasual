using Abilities.SecondEdition;
using System.Collections.Generic;
using Ship;
using SubPhases;
using BoardTools;
using Content;
using Actions;
using ActionsList;
using Tokens;
using System;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class WedgeAntillesBoELSL : T65XWing
        {
            public WedgeAntillesBoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Wedge Antilles",
                    6,
                    55,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    abilityType: typeof(Abilities.FirstEdition.WhisperAbility)
                );
                ShipAbilities.Add(new LockedSFoils());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(BoostAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(BoostAction)));
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                PilotNameCanonical = "wedgeantilles-battleoverendor-lsl";
                ModelInfo.SkinName = "Wedge Antilles";
            }
        }
    }
}
