using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class Scorch : TIEFoFighter
        {
            public Scorch() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Scorch\"",
                    4,
                    33,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.ZetaLeaderAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
