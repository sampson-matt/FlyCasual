using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Arcs;
using Content;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class TychoCelchuBoE : RZ1AWing
        {
            public TychoCelchuBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Tycho Celchu",
                    5,
                    57,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TychoCelchuBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    extraUpgradeIcons: new List<UpgradeType> 
                    { 
                        UpgradeType.Talent, 
                        UpgradeType.Talent,
                        UpgradeType.Modification
                    }
                );

                PilotNameCanonical = "tychocelchu-battleoverendor";
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(FocusAction), typeof(ReloadAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BoostAction), typeof(EvadeAction)));
                ShipInfo.ArcInfo.Arcs.Add(new ShipArcInfo(ArcType.SingleTurret, 2));
                ShipInfo.ArcInfo.Arcs.RemoveAll(n => n.ArcType == ArcType.Front);
                VectoredThrustersAbility oldAbility = (VectoredThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(VectoredThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new VectoredCannonsAbility());

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(Juke));
                MustHaveUpgrades.Add(typeof(ProtonRockets));
                MustHaveUpgrades.Add(typeof(ChaffParticlesBoE));
            }            
        }
    }
}