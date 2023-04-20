using Upgrade;

namespace Ship
{
    namespace SecondEdition.ModifiedTIELnFighter
    {
        public class MiningGuildSurveyor : ModifiedTIELnFighter
        {
            public MiningGuildSurveyor() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Mining Guild Surveyor",
                    2,
                    23,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
