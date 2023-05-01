using Abilities.SecondEdition;
using SubPhases;
using Upgrade;
using Ship;
using System.Linq;
using Tokens;
using ActionsList;
using Actions;
using BoardTools;
using Content;
using System.Collections.Generic;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sigma7SL : TIEInterceptor
        {
            public Sigma7SL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sigma7",
                    4,
                    48,
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Sigma7Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    extraUpgradeIcons: new List<UpgradeType>()
                    {
                        UpgradeType.Talent,
                        UpgradeType.Sensor
                    },
                    isStandardLayout: true
                );

                PilotNameCanonical = "sigma7-battleofyavin-sl";

                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(TargetLockAction)));
                ShipInfo.Hull++;
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());

                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.Marksmanship));
                MustHaveUpgrades.Add(typeof(UpgradesList.SecondEdition.FireControlSystem));

                ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/b/bc/Sigma7-battleofyavin.png";
            }
        }
    }
}