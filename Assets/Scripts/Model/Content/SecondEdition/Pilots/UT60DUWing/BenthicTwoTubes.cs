﻿using Upgrade;

namespace Ship
{
    namespace SecondEdition.UT60DUWing
    {
        public class BenthicTwoTubes : UT60DUWing
        {
            public BenthicTwoTubes() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Benthic Two Tubes",
                    2,
                    45,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.BenthicTwoTubesAbility),
                    extraUpgradeIcon: UpgradeType.Illicit
                );

                ModelInfo.SkinName = "Partisan";
            }
        }
    }
}