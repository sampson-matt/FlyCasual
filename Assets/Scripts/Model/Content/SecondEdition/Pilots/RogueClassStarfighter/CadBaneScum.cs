using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class CadBaneScum : RogueClassStarfighter
        {
            public CadBaneScum() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Cad Bane",
                    4,
                    40,
                    pilotTitle: "Infamous Bounty Hunter",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.CadBaneScumAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                PilotNameCanonical = "cadbanescum-rogueclassstarfighter";

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/cadbane.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CadBaneScumAbility : GenericAbility
    {
        public override void ActivateAbility()
        {

        }

        public override void DeactivateAbility()
        {

        }
    }
}