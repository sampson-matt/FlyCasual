using Upgrade;

namespace Ship
{
    namespace SecondEdition.FiresprayClassPatrolCraft
    {
        public class BountyHunter : FiresprayClassPatrolCraft
        {
            public BountyHunter() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Bounty Hunter",
                    2,
                    62,
                    extraUpgradeIcon: UpgradeType.Crew
                );

                ModelInfo.SkinName = "Mandalorian Mercenary";
            }
        }
    }
}
