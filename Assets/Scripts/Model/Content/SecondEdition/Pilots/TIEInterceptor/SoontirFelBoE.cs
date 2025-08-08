using Abilities.SecondEdition;
using ActionsList;
using Content;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using UpgradesList.SecondEdition;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class SoontirFelBoE : TIEInterceptor
        {
            public SoontirFelBoE() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Soontir Fel",
                    6,
                    68,
                    isLimited: true,
                    abilityType: typeof(SoontirFelBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.SL
                    },
                    isStandardLayout: true,
                    charges: 2,
                    extraUpgradeIcons: new List<UpgradeType>
                    {
                        UpgradeType.Talent,
                        UpgradeType.Talent,
                        UpgradeType.Sensor,
                        UpgradeType.Illicit
                    }
                );
                PilotNameCanonical = "soontirfel-battleoverendor";
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
                ModelInfo.SkinName = "Red Stripes";

                MustHaveUpgrades.Add(typeof(ApexPredator));
                MustHaveUpgrades.Add(typeof(NoEscapeBoE));
                MustHaveUpgrades.Add(typeof(BlankSignature));
                MustHaveUpgrades.Add(typeof(FeedbackEmitter));
            }
        }
    }
}