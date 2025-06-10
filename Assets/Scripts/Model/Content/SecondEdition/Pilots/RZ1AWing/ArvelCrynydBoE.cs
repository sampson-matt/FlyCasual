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
        public class ArvelCrynydBoE : RZ1AWing
        {
            public ArvelCrynydBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Arvel Crynyd",
                    3,
                    51,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.ArvelCrynydBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Talent }
                );
                PilotNameCanonical = "arvelcrynyd-battleoverendor";
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(SlamAction)));
                ShipInfo.ArcInfo.Arcs.Add(new ShipArcInfo(ArcType.SingleTurret, 2));
                ShipInfo.ArcInfo.Arcs.RemoveAll(n => n.ArcType == ArcType.Front);
                VectoredThrustersAbility oldAbility = (VectoredThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(VectoredThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new VectoredCannonsAbility());

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(ProtonRockets));
                MustHaveUpgrades.Add(typeof(HeroicSacrifice));
            }            
        }
    }
}