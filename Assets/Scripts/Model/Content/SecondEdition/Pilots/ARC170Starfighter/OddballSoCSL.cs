using System;
using System.Collections.Generic;
using Upgrade;
using Ship;
using Tokens;
using BoardTools;
using SubPhases;
using UnityEngine;
using ActionsList;
using Content;
using Movement;
using Editions;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class OddballSoCSL : ARC170Starfighter
        {
            public OddballSoCSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Odd Ball\"",
                    5,
                    56,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.OddBallSoCAbility),
                    tags: new List<Tags>
                    {
                        Tags.SoC,
                        Tags.SL
                    },
                    extraUpgradeIcon: UpgradeType.Talent,
                    isStandardLayout: true
                );
                ShipInfo.Shields++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipAbilities.Add(new Abilities.SecondEdition.BornForThisAbility());

                PilotNameCanonical = "oddball-siegeofcoruscant";

                ModelInfo.SkinName = "Red";

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Selfless));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.VeteranTailGunner));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.R4PAstromech));
            }
        }
    }
}