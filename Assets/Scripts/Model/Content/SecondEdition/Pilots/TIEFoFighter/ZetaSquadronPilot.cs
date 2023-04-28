using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class ZetaSquadronPilot : TIEFoFighter
        {
            public ZetaSquadronPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Zeta Squadron Pilot",
                    2,
                    27
                );
            }
        }
    }
}
