using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using Upgrade;
using Mods.ModsList;
using Actions;
using ActionsList;

namespace Ship
{
    namespace SecondEdition.Hwk290LightFreighter
    {
        public class HotacHWK : Hwk290LightFreighter, IHotacShip
        {
            private int previousInit = 3;
            public HotacHWK() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac HWK",
                    3,
                    0,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Illicit, UpgradeType.Init, UpgradeType.Pilot }
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/rebelscout.png";

                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ProtectAction)));
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
            }
            public void RecheckSlots()
            {
                if (PilotInfo.Initiative > previousInit)
                {
                    previousInit = PilotInfo.Initiative;
                    switch (PilotInfo.Initiative)
                    {
                        case 4:
                            UpgradeBar.AddSlot(UpgradeType.Modification);
                            break;
                        case 5:
                            UpgradeBar.AddSlot(UpgradeType.Sensor);
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
                        case 3:
                            UpgradeBar.RemoveSlot(UpgradeType.Modification);
                            break;
                        case 4:
                            UpgradeBar.RemoveSlot(UpgradeType.Sensor);
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
