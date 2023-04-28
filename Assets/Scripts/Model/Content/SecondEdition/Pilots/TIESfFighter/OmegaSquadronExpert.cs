using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESfFighter
    {
        public class OmegaSquadronExpert : TIESfFighter
        {
            public OmegaSquadronExpert() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Omega Squadron Expert",
                    3,
                    33,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
