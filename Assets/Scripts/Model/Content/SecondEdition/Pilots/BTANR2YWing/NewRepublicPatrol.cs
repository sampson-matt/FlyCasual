using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTANR2YWing
    {
        public class NewRepublicPatrol : BTANR2YWing
        {
            public NewRepublicPatrol() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "New Republic Patrol",
                    3,
                    30,
                    extraUpgradeIcon: UpgradeType.Talent
                );

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}
