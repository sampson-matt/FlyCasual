using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEVnSilencer
    {
        public class FirstOrderTestPilot : TIEVnSilencer
        {
            public FirstOrderTestPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "First Order Test Pilot",
                    4,
                    55,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
