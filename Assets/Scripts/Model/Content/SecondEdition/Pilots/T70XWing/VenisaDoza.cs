using Movement;
using Ship;
using Arcs;
using Upgrade;
using System.Collections.Generic;
using BoardTools;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class VenisaDoza : T70XWing
        {
            public VenisaDoza() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Venisa Doza",
                    4,
                    48,
                    isLimited: true,
                    pilotTitle: "Jade Leader",
                    abilityType: typeof(Abilities.SecondEdition.VenisaDozaAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class VenisaDozaAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnGameStart += UpdateArcRequirements;
            HostShip.OnUpdateWeaponRange += CheckRange;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGameStart -= UpdateArcRequirements;
            HostShip.OnUpdateWeaponRange -= CheckRange;
        }

        private void CheckRange(IShipWeapon weapon, ref int minRange, ref int maxRange, GenericShip target)
        {
            if (weapon is GenericSpecialWeapon)
            {
                var specialWeapon = weapon as GenericSpecialWeapon;
                if ((specialWeapon.UpgradeInfo.HasType(UpgradeType.Missile) || specialWeapon.UpgradeInfo.HasType(UpgradeType.Torpedo)) && Board.GetShipsInArcAtRange(HostShip, ArcType.Rear, new UnityEngine.Vector2(0,4),Team.Type.Enemy).Contains(target))
                {
                    minRange = 1;
                    maxRange = 2;
                }
            }
        }

        private void UpdateArcRequirements()
        {
            foreach (GenericUpgrade weaponUpgrade in HostShip.UpgradeBar.GetSpecialWeaponsAll())
            {
                IShipWeapon specialWeapon = weaponUpgrade as IShipWeapon;
                if (specialWeapon.WeaponType == WeaponTypes.Torpedo || specialWeapon.WeaponType == WeaponTypes.Missile)
                {
                    if (specialWeapon.WeaponInfo.ArcRestrictions.Contains(ArcType.Front))
                    {
                        specialWeapon.WeaponInfo.ArcRestrictions.Add(ArcType.Rear);
                    }
                }
            }
        }
    }
}
