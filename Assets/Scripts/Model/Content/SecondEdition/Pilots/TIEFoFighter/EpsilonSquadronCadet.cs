using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEFoFighter
    {
        public class EpsilonSquadronCadet : TIEFoFighter
        {
            public EpsilonSquadronCadet() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Epsilon Squadron Cadet",
                    1,
                    26
                );
            }
        }
    }
}
