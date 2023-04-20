using Actions;
using ActionsList;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.EscapeCraft
    {
        public class L337EscapeCraft : EscapeCraft
        {
            public L337EscapeCraft() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "L3-37",
                    2,
                    25,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.L337Ability),
                    extraUpgradeIcon: UpgradeType.Talent
                );

                PilotNameCanonical = "l337-escapecraft";

                ShipInfo.ActionIcons.SwitchToDroidActions();

                ShipAbilities.Add(new Abilities.SecondEdition.CoPilotAbility());
            }
        }
    }
}