using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESfFighter
    {
        public class ZetaSquadronSurvivor : TIESfFighter
        {
            public ZetaSquadronSurvivor() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Zeta Squadron Survivor",
                    2,
                    32
                );
            }
        }
    }
}
