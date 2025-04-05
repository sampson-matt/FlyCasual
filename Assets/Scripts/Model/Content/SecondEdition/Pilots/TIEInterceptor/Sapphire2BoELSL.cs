using Abilities.SecondEdition;
using ActionsList;
using Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEInterceptor
    {
        public class Sapphire2BoELSL : TIEInterceptor
        {
            public Sapphire2BoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Sapphire 2",
                    1,
                    38,
                    isLimited: true,
                    abilityType: typeof(Saphire2Ability),
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    }
                );
                PilotNameCanonical = "sapphire2-battleoverendor-lsl";
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                AutoThrustersAbility oldAbility = (AutoThrustersAbility)ShipAbilities.First(n => n.GetType() == typeof(AutoThrustersAbility));
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new SensitiveControlsRealAbility());
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //While you defend, if you are focused, roll 1 additional defense die.
    public class Saphire2Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfDefenceDice += CheckDefenseBonus;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfDefenceDice -= CheckDefenseBonus;
        }

        private void CheckDefenseBonus(ref int count)
        {
            if (HostShip.Tokens.HasToken(typeof(FocusToken)))
            {
                Messages.ShowInfo($"{HostShip.PilotInfo.PilotName} rolls 1 additional defense die");
                count++;
            }
        }
    }
}