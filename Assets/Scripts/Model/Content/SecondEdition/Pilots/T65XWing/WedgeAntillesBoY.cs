using Conditions;
using Ship;
using Content;
using Abilities.SecondEdition;
using System.Collections.Generic;

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
                    49,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(Abilities.SecondEdition.WedgeAntillesBoYAbility)
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/wedgeantilles-boy.png";
                PilotNameCanonical = "wedgeantilles-battleofyavin";
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
                BoardTools.ShotInfo shotInfo = new BoardTools.ShotInfo(Combat.Defender, friendlyShip, Combat.Defender.PrimaryWeapons);
                if (shotInfo.InArc && friendlyShip.ShipId != HostShip.ShipId && Tools.IsFriendly(friendlyShip, HostShip))  friendlyShipInArc = true;
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
