using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class HotacBomberPilot : TIESaBomber
        {
            public HotacBomberPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac Bomber Pilot",
                    1,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent, UpgradeType.Pilot, UpgradeType.Pilot, UpgradeType.Torpedo, UpgradeType.Init}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/scimitarsquadronpilot.png";

                RequiredMods = new List<Type>() { typeof(HotacEliteImperialPilotsModSE) };
            }
        }
    }
}
