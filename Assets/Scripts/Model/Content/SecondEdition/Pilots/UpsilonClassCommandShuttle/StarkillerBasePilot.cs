using Upgrade;

namespace Ship
{
    namespace SecondEdition.UpsilonClassCommandShuttle
    {
        public class StarkillerBasePilot : UpsilonClassCommandShuttle
        {
            public StarkillerBasePilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Starkiller Base Pilot",
                    2,
                    58
                );
            }
        }
    }
}
