using Conditions;
using Ship;
using Upgrade;
using Abilities.SecondEdition;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class WedgeAntillesBoY : T65XWing
        {
            public WedgeAntillesBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Wedge Antilles",
                    5,
                    48,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.WedgeAntillesBoYAbility)
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/wedgeantilles-boy.png";
                PilotNameCanonical = "wedgeantilles-boy";
                ModelInfo.SkinName = "Wedge Antilles";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class WedgeAntillesBoYAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += TryAddWedgeAntillesAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsAttacker -= TryAddWedgeAntillesAbility;
        }

        public void TryAddWedgeAntillesAbility()
        {
            bool friendlyShipInArc = false;
            foreach (GenericShip friendlyShip in HostShip.Owner.Ships.Values)
            {
                if (friendlyShip.ShipId == HostShip.ShipId) return;
                BoardTools.ShotInfo shotInfo = new BoardTools.ShotInfo(Combat.Defender, friendlyShip, Combat.Defender.PrimaryWeapons);
                if (shotInfo.InArc) friendlyShipInArc = true;
            }
            if (Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon && friendlyShipInArc)
            {
                if (HostShip.SectorsInfo.IsShipInSector(Combat.Defender, Arcs.ArcType.Front))
                {
                    WedgeAntillesCondition condition = new WedgeAntillesCondition(Combat.Defender, HostShip);
                    Combat.Defender.Tokens.AssignCondition(condition);
                }
            }
        }
    }
}
