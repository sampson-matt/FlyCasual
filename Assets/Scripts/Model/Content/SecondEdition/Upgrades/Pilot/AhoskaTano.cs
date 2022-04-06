using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;

namespace UpgradesList.SecondEdition
{
    public class AhsokaTanoPilotAbility : GenericUpgrade
    {
        public AhsokaTanoPilotAbility() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ahsoka Tano Pilot Ability",
                UpgradeType.Pilot,

                cost: 8,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.AhsokaTanoAbility),
                addForce: 2
            );
            ImageUrl = "https://images-cdn.fantasyflightgames.com/filer_public/f2/84/f284aa2b-9e09-4c3c-968b-935360a65edc/swz83_pilot_ahsokatano.png";
        }


    }
}