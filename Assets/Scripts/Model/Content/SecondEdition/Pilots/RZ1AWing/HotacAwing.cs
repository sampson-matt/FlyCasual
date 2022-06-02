using System.Collections;
using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
using Actions;
using ActionsList;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class HotacAwingPilot : RZ1AWing, IHotacShip
        {
            private int previousInit = 3;
            public HotacAwingPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac A-Wing",
                    3,
                    5,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Init, UpgradeType.Pilot, UpgradeType.Modification },
                    seImageNumber: 21
                );
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ProtectAction)));
                ModelInfo.SkinName = "Green";
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
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