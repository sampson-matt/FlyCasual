using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class RearAdmiralChiraneauPilotAbility : GenericUpgrade
    {
        public RearAdmiralChiraneauPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Rear Admiral Chiraneau Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.FirstEdition.RearAdmiralChiraneauAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/rearadmiralchiraneau.png";
        }


    }
}