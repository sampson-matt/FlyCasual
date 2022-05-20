using ActionsList;
using Ship;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class ExtraMunitions : GenericUpgrade
    {
        public ExtraMunitions() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Extra Munitions",
                UpgradeType.Torpedo,
                cost: 4,
                feIsLimitedPerShip: true,
                abilityType: typeof(Abilities.SecondEdition.ExtraMunitionsAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/torpedo/extramunitions.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ExtraMunitionsAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnSetupPlaced += AddOrdnance;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnSetupPlaced -= AddOrdnance;
        }

        private void AddOrdnance(GenericShip ship)
        {
            foreach (var upgrade in HostShip.UpgradeBar.GetUpgradesOnlyFaceup())
            {
                if ((upgrade.HasType(UpgradeType.Torpedo) || upgrade.HasType(UpgradeType.Missile) || upgrade.HasType(UpgradeType.Device))&&upgrade.State.Charges>0)
                {
                    upgrade.State.MaxCharges++;
                    upgrade.State.Charges++;                    
                }
            }
            Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);
            DeactivateAbility();
        }
    }
}