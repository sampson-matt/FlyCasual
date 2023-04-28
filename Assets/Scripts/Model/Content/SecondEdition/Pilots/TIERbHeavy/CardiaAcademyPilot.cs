using Arcs;
using Movement;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIERbHeavy
    {
        public class CardiaAcademyPilot : TIERbHeavy
        {
            public CardiaAcademyPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Carida Academy Cadet",
                    1,
                    33
                );
            }
        }
    }
}