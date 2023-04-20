using Upgrade;

namespace Ship
{
    namespace SecondEdition.StarViperClassAttackPlatform
    {
        public class BlackSunAssassin : StarViperClassAttackPlatform
        {
            public BlackSunAssassin() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Black Sun Assassin",
                    3,
                    47,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Black Sun Assassin";
            }
        }
    }
}