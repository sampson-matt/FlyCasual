using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
using Actions;
using ActionsList;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class HotacXwing : T65XWing, IHotacShip
        {
            private int previousInit = 2;
            public HotacXwing() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac X-Wing",
                    2,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Init},
                    seImageNumber: 11
                );
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ProtectAction)));
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }

            public void RecheckSlots()
            {
                if(PilotInfo.Initiative>previousInit)
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
                if (PilotInfo.Initiative<previousInit)
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
