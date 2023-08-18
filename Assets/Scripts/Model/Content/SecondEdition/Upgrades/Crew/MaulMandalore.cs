using System.Collections.Generic;
using Upgrade;
using ActionsList;
using Content;
using SquadBuilderNS;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class MaulMandalore : GenericUpgrade
    {
        public MaulMandalore() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Maul",
                types: new List<UpgradeType>()
                {
                    UpgradeType.Crew,
                    UpgradeType.Crew
                },
                cost: 10,
                isLimited: true,
                addForce: 1,
                addSlot: new UpgradeSlot(UpgradeType.Illicit),
                addAction: new Actions.ActionInfo(typeof(CoordinateAction), Actions.ActionColor.Purple),
                restriction: new FactionRestriction(Faction.Scum)
            );

            NameCanonical = "maul-crew";

        }

    }
}