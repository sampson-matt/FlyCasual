using System.Collections.Generic;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class SyncedLaserCannons : GenericSpecialWeapon
    {
        public SyncedLaserCannons() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Synced Laser Cannons",
                types: new List<UpgradeType>()
                {
                    UpgradeType.Cannon,
                    UpgradeType.Cannon
                },
                cost: 7,
                weaponInfo: new SyncedLaserCannonsWeaponInfo(this)                
            );
        }

        private class SyncedLaserCannonsWeaponInfo : SpecialWeaponInfo
        {
            private GenericUpgrade HostUpgrade;
            public SyncedLaserCannonsWeaponInfo(GenericUpgrade hostUpgrade) : base(3, 2, 3)
            {
                HostUpgrade = hostUpgrade;
            }
                
            public override bool NoRangeBonus 
            { 
                get 
                {
                    if (Combat.AttackStep == CombatStep.Defence
                        && Combat.Attacker == HostUpgrade.HostShip
                        && HostUpgrade.HostShip.Tokens.HasToken<Tokens.CalculateToken>())
                        return true;
                    else 
                        return false;
                } 
            }
        
        }
    }
}