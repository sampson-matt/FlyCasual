using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEVnSilencer
    {
        public class Blackout : TIEVnSilencer
        {
            public Blackout() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Blackout\"",
                    5,
                    61,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.TestPilotBlackoutAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}
