using System.Collections.Generic;
using Upgrade;
using Content;

namespace Ship
{
    namespace SecondEdition.FangFighter
    {
        public class KadSolus : FangFighter
        {
            public KadSolus() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Kad Solus",
                    4,
                    48,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.KadSolusAbility),
                    tags: new List<Tags>
                    {
                        Tags.Mandalorian
                    },
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Modification }
                );

                ModelInfo.SkinName = "Skull Squadron Pilot";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you fully execute a red maneuver, gain 2 focus tokens.
    public class KadSolusAbility : Abilities.FirstEdition.KadSolusAbility
    {
        protected override bool CheckAbility()
        {
            if (HostShip.IsBumped) return false;

            return base.CheckAbility();
        }
    }
}