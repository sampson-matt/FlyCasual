using Upgrade;
using Ship;
using Tokens;
using BoardTools;
using Bombs;

namespace Ship
{
    namespace SecondEdition.BTANR2YWing
    {
        public class LegaFossang : BTANR2YWing
        {
            public LegaFossang() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Lega Fossang",
                    3,
                    31,
                    extraUpgradeIcon: UpgradeType.Talent,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.LegaFossangAbility)
                );

                ModelInfo.SkinName = "Blue";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class LegaFossangAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification
            (
                HostShip.PilotInfo.PilotName,
                IsAvailable,
                GetDiceModificationPriority,
                DiceModificationType.Reroll,
                GetRerollCount
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool IsAvailable()
        {
            return Combat.AttackStep == CombatStep.Attack && 
                (Combat.ChosenWeapon.WeaponType == Ship.WeaponTypes.Turret || Combat.ChosenWeapon.WeaponType == Ship.WeaponTypes.PrimaryWeapon) && 
                GetRerollCount() > 0;
        }

        private int GetDiceModificationPriority()
        {
            return 90; // Free rerolls
        }

        private int GetRerollCount()
        {
            int friendlies = 0;
            foreach (GenericShip ship in Roster.AllShips.Values)
            {
                if (ship.Tokens.HasToken(typeof(CalculateToken)))
                {
                    ShotInfo shotInfo = new ShotInfo(HostShip, ship, Combat.ChosenWeapon);
                    if (shotInfo.InArc) friendlies++;
                }
            }

            foreach (var bombHolder in BombsManager.GetBombsOnBoard())
            {
                if (Tools.IsFriendly(bombHolder.Value.HostShip, HostShip))
                {
                    if(BombsManager.IsDeviceInArc(HostShip, bombHolder.Key, Combat.ArcForShot, Combat.ChosenWeapon))
                    {
                        friendlies++;
                    }
                }
            }
            return friendlies;
        }
    }
}
