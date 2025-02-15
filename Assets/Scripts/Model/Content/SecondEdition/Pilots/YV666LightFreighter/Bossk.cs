﻿using Ship;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.YV666LightFreighter
    {
        public class Bossk : YV666LightFreighter
        {
            public Bossk() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Bossk",
                    4,
                    63,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BosskPilotAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BosskPilotAbility : Abilities.FirstEdition.BosskPilotAbility
    {
        protected override void RegisterBosskPilotAbility()
        {
            if (Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon)
            {
                base.RegisterBosskPilotAbility();
            }
        }
    }
}