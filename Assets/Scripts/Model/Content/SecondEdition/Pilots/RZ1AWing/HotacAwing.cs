using System.Collections;
using System.Collections.Generic;
using Mods.ModsList;
using System;
using Upgrade;
using Actions;
using ActionsList;
using System.Linq;

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
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Init, UpgradeType.Pilot, UpgradeType.Talent, UpgradeType.Modification }
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/greensquadronpilot.png";

                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ProtectAction)));
                ModelInfo.SkinName = "Green";
                RequiredMods = new List<Type>() { typeof(HotacPilotsModSE) };
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Configuration);
                ShipAbilities.Add(new Abilities.SecondEdition.HotacPilotUpgradeAwingAbility());
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

namespace Abilities.SecondEdition
{
    public class HotacPilotUpgradeAwingAbility : GenericAbility
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
            if (UpgradeType.Talent.Equals(upgrade.UpgradeInfo.UpgradeTypes.First()) && HostShip.UpgradeBar.GetUpgradeSlots().Where(slot => UpgradeType.Talent.Equals(slot.Type) && !slot.IsEmpty).Count() == 1)
            {
                return;
            }
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
            if (UpgradeType.Talent.Equals(upgrade.UpgradeInfo.UpgradeTypes.First()) && HostShip.UpgradeBar.GetUpgradeSlots().Where(slot => UpgradeType.Talent.Equals(slot.Type) && !slot.IsEmpty).Count() == 1)
            {
                return;
            }
            if (HardpointSlotTypes.Contains(upgrade.UpgradeInfo.UpgradeTypes.First()))
            {
                HardpointSlotTypes
                    .Where(slot => slot != upgrade.UpgradeInfo.UpgradeTypes.First())
                    .ToList()
                    .ForEach(slot => HostShip.UpgradeBar.AddSlot(slot));
            }
        }
    }
}