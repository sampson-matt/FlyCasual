using Upgrade;

namespace Ship
{
    namespace SecondEdition.BTANR2YWing
    {
        public class CaiThrenalli : BTANR2YWing
        {
            public CaiThrenalli() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "C'ai Threnalli",
                    2,
                    32,
                    extraUpgradeIcon: UpgradeType.Talent,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CaiThrenalliAbility)
                );

                ModelInfo.SkinName = "Red";

                PilotNameCanonical = "caithrenalli-btanr2ywing";
            }
        }
    }
}
