using Upgrade;

namespace Ship
{
    namespace SecondEdition.NantexClassStarfighter
    {
        public class PetranakiArenaAce : NantexClassStarfighter
        {
            public PetranakiArenaAce() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Petranaki Arena Ace",
                    4,
                    36,
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}