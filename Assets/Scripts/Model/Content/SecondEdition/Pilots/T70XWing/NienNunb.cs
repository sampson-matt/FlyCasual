using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class NienNunb : T70XWing
        {
            public NienNunb() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Nien Nunb",
                    5,
                    56,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.NienNunbAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
