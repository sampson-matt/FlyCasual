using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
using Actions;
using ActionsList;
using System.Linq;
using Ship;

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
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Init}
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/bluesquadronescort.png";

                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ProtectAction)));
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ShipAbilities.Add(new Abilities.SecondEdition.HotacPilotUpgradeAbility());
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
                            UpgradeBar.AddSlot(UpgradeType.Talent);
                            break;
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
                if (PilotInfo.Initiative<previousInit)
                {
                    previousInit = PilotInfo.Initiative;
                    switch (previousInit)
                    {
                        case 2:
                            UpgradeBar.RemoveSlot(UpgradeType.Pilot);
                            UpgradeBar.RemoveSlot(UpgradeType.Talent);
                            UpgradeBar.RemoveSlot(UpgradeType.Pilot);
                            break;
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

namespace Abilities.SecondEdition
{
    public class HotacPilotUpgradeAbility : GenericAbility
    {
        private readonly List<UpgradeType> HardpointSlotTypes = new List<UpgradeType>
        {
            UpgradeType.Pilot,
            UpgradeType.Talent
        };

        public override void ActivateAbilityForSquadBuilder()
        {
            HostShip.OnPreInstallUpgrade += OnPreInstallUpgrade;
            HostShip.OnRemovePreInstallUpgrade += OnRemovePreInstallUpgrade;
        }

        public override void ActivateAbility()
        {
            
        }

        public override void DeactivateAbility()
        {
            
        }

        public override void DeactivateAbilityForSquadBuilder() { }

        private void OnPreInstallUpgrade(GenericUpgrade upgrade)
        {
            if (HardpointSlotTypes.Contains(upgrade.UpgradeInfo.UpgradeTypes.First()))
            {
                HardpointSlotTypes
                    .Where(slot => slot != upgrade.UpgradeInfo.UpgradeTypes.First())
                    .ToList()
                    .ForEach(slot => HostShip.UpgradeBar.RemoveSlot(slot));
            }
        }

        private void OnRemovePreInstallUpgrade(GenericUpgrade upgrade)
        {
            if (upgrade != null  && HardpointSlotTypes.Contains(upgrade.UpgradeInfo.UpgradeTypes.First()))
            {
                HardpointSlotTypes
                    .Where(slot => slot != upgrade.UpgradeInfo.UpgradeTypes.First())
                    .ToList()
                    .ForEach(slot => HostShip.UpgradeBar.AddSlot(slot));
            }
        }
    }
}
