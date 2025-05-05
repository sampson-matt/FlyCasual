using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class ContingencyProtocolSoC : GenericUpgrade
    {
        public ContingencyProtocolSoC() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo
            (
                "Contingency Protocol",
                UpgradeType.Modification,
                cost: 0,
                abilityType: typeof(Abilities.SecondEdition.ContingencyProtocolAbility)
            );

            ImageUrl = "https://i.imgur.com/5MMMAtf.jpg";
        }

    }
}