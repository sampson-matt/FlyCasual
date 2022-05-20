using ActionsList;
using Ship;
using SquadBuilderNS;
using Upgrade;
using Actions;

namespace UpgradesList.SecondEdition
{
    public class Autothrusters : GenericUpgrade
    {
        public Autothrusters() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Autothrusters",
                UpgradeType.Modification,
                cost: 4,
                abilityType: typeof(Abilities.FirstEdition.AutothrustersAbility),
                restriction: new ActionBarRestriction(typeof(BoostAction))
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/modification/autothrusters.png";
        }

        public override bool IsAllowedForSquadBuilderPostCheck(SquadList squadList)
        {
            // TODO

            bool result = false;

            result = IsAllowedForShip(HostShip);
            if (!result) Messages.ShowError("Autothrusters can only be installed if ship has a Boost action icon");

            return result;
        }
    }
}