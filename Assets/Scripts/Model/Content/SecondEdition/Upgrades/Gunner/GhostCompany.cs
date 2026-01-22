using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Actions;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class GhostCompany : GenericUpgrade
    {
        public GhostCompany() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Ghost Company",
                types: new List<UpgradeType>() { UpgradeType.Crew, UpgradeType.Gunner },
                cost: 5,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.BistanGunnerAbility),
                restrictions: new UpgradeCardRestrictions
                (
                    new FactionRestriction(Faction.Republic),
                    new ActionBarRestriction(typeof(RotateArcAction))
                ),
                addActionLink: new LinkedActionInfo(typeof(RotateArcAction), typeof(FocusAction), ActionColor.Red)
            );

            Avatar = new AvatarInfo(
                Faction.Republic,
                new Vector2(304, 31),
                new Vector2(50, 50)
            );
        }
    }
}