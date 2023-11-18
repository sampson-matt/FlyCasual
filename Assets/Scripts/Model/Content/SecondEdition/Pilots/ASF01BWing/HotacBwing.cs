using System.Collections;
using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
using Actions;
using ActionsList;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class HotacBwing : ASF01BWing, IHotacShip
        {
            private int previousInit = 3;
            public HotacBwing() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Hotac B-Wing",
                    3,
                    5,
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Init, UpgradeType.Pilot, UpgradeType.Talent }
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/bladesquadronveteran.png";

                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ProtectAction)));
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);
                ShipAbilities.Add(new Abilities.SecondEdition.HotacPilotUpgradeAbility());
                ModelInfo.SkinName = "Blue";
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
                            UpgradeBar.AddSlot(UpgradeType.Talent);
                            break;
                        case 6:
                            UpgradeBar.AddSlot(UpgradeType.Pilot);
                            UpgradeBar.AddSlot(UpgradeType.Talent);
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
                            UpgradeBar.RemoveSlot(UpgradeType.Talent);
                            break;
                        case 5:
                            UpgradeBar.RemoveSlot(UpgradeType.Pilot);
                            UpgradeBar.RemoveSlot(UpgradeType.Talent);
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
