using Abilities.SecondEdition;
using Actions;
using ActionsList;
using Content;
using System.Collections.Generic;
using Upgrade;
using UpgradesList.SecondEdition;

namespace Ship
{
    namespace SecondEdition.ModifiedYT1300LightFreighter
    {
        public class LandoCalrissianBoE : ModifiedYT1300LightFreighter
        {
            public LandoCalrissianBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lando Calrissian",
                    5,
                    101,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    abilityType: typeof(LandoCalrissianBattleOverEndorAbility),
                    charges: 2,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent
                    }
                );
                ShipAbilities.Add(new HighStakesAbility());
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(EvadeAction)));
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(CoordinateAction), typeof(FocusAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(CoordinateAction), ActionColor.Red));

                MustHaveUpgrades.Add(typeof(ItsATrap));
                MustHaveUpgrades.Add(typeof(AceInTheHole));
                MustHaveUpgrades.Add(typeof(NienNunb));
                MustHaveUpgrades.Add(typeof(AirenCracken));
                MustHaveUpgrades.Add(typeof(MillenniumFalconBoE));

                PilotNameCanonical = "landocalrissian-battleoverendor";
            }
            
        }
        
    }
}