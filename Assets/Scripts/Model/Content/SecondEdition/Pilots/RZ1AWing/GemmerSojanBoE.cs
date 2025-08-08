using Abilities.SecondEdition;
using Arcs;
using Content;
using System;
using System.Collections.Generic;
using System.Linq;
using UpgradesList.SecondEdition;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class GemmerSojanBoE : RZ1AWing
        {
            public GemmerSojanBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Gemmer Sojan",
                    2,
                    46,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.GemmerSojanBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Cannon,
                        UpgradeType.Modification,
                        UpgradeType.Modification
                    }
                );
                PilotNameCanonical = "gemmersojan-battleoverendor";
                ShipInfo.ArcInfo.Arcs.Add(new ShipArcInfo(ArcType.SingleTurret, 2));
                ShipInfo.ArcInfo.Arcs.RemoveAll(n => n.ArcType == ArcType.Front);
                VectoredThrustersAbility oldAbility = (VectoredThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(VectoredThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new VectoredCannonsAbility());

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(PrecisionTunedCannons));
                MustHaveUpgrades.Add(typeof(TargetAssistAlgorithm));
                MustHaveUpgrades.Add(typeof(ChaffParticlesBoE));
            }            
        }
    }
}