using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class Static : TIEFoFighter
        {
            public Static() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Static\"",
                    4,
                    30,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.OmegaAceAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
