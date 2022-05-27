using Upgrade;
using System.Collections.Generic;
using Mods.ModsList;
using System;

namespace Ship
{
    namespace SecondEdition.BTLA4YWing
    {
        public class HotacYwing : BTLA4YWing, IHotacShip
        {
            private int previousInit = 2;
            public HotacYwing() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac Y-Wing",
                    2,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Init },
                    seImageNumber: 18
                );
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) }; 
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Missile);
                ShipInfo.UpgradeIcons.Upgrades.Add(UpgradeType.Gunner);
            }
            public void RecheckSlots()
            {
                if (PilotInfo.Initiative > previousInit)
                {
                    previousInit = PilotInfo.Initiative;
                    switch (PilotInfo.Initiative)
                    {
                        case 3:
                            UpgradeBar.AddSlot(UpgradeType.Pilot);
                            break;
                        case 4:
                            UpgradeBar.AddSlot(UpgradeType.Modification);
                            break;
                        case 5:
                            UpgradeBar.AddSlot(UpgradeType.Pilot);
                            break;
                        case 6:
                            UpgradeBar.AddSlot(UpgradeType.Pilot);
                            UpgradeBar.AddSlot(UpgradeType.Modification);
                            break;
                        default:
                            break;
                    }
                }
                if (PilotInfo.Initiative < previousInit)
                {
                    previousInit = PilotInfo.Initiative;
                    switch (previousInit)
                    {
                        case 2:
                            UpgradeBar.RemoveSlot(UpgradeType.Pilot);
                            break;
                        case 3:
                            UpgradeBar.RemoveSlot(UpgradeType.Modification);
                            break;
                        case 4:
                            UpgradeBar.RemoveSlot(UpgradeType.Pilot);
                            break;
                        case 5:
                            UpgradeBar.RemoveSlot(UpgradeType.Pilot);
                            UpgradeBar.RemoveSlot(UpgradeType.Modification);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
