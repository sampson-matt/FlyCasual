using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTLBYWing
    {
        public class RedSquadronBomber : BTLBYWing
        {
            public RedSquadronBomber() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Red Squadron Bomber",
                    2,
                    30,
                    extraUpgradeIcon: UpgradeType.Astromech
                );
            }
        }
    }
}
