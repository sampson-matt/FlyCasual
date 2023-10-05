using Arcs;
using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ2AWing
    {
        public class MerlCobben : RZ2AWing
        {
            public MerlCobben() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Merl Cobben",
                    1,
                    34,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.MerlCobbenAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent } 
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MerlCobbenAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.AfterGotNumberOfDefenceDiceGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.AfterGotNumberOfDefenceDiceGlobal -= CheckAbility;
        }

        private void CheckAbility(ref int defenseDiceCount)
        {
            if (AreConditionsMet())
            {
                Messages.ShowInfo("Merl Cobben: Defender rolls 1 fewer defense die");
                defenseDiceCount--;
            }
        }

        private bool AreConditionsMet()
        {
            bool result = false;

            if (Tools.IsFriendly(Combat.Attacker, HostShip))
            {
                if (new DistanceInfo(Combat.Attacker, HostShip).Range < 3)
                {
                    if (Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon)
                    {
                        if (Combat.Defender.SectorsInfo.IsShipInSector(HostShip, ArcType.Bullseye))
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}