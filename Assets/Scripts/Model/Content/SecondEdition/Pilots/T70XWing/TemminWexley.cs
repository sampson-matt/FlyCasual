using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class TemminWexley : T70XWing
        {
            public TemminWexley() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Temmin Wexley",
                    4,
                    48,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.SnapWexleyAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
