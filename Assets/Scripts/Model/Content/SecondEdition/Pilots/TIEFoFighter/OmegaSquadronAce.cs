using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class OmegaSquadronAce : TIEFoFighter
        {
            public OmegaSquadronAce() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Omega Squadron Ace",
                    3,
                    28,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
