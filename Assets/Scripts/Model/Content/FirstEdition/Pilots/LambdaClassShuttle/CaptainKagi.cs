using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.LambdaClassShuttle
    {
        public class CaptainKagi : LambdaClassShuttle
        {
            public CaptainKagi() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Captain Kagi",
                    8,
                    27,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.CaptainKagiAbility)
                );
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class CaptainKagiAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            RulesList.TargetLocksRule.OnCheckTargetLockIsDisallowed += CanPerformTargetLock;
        }

        public override void DeactivateAbility()
        {
            RulesList.TargetLocksRule.OnCheckTargetLockIsDisallowed -= CanPerformTargetLock;
        }

        public void CanPerformTargetLock(ref bool result, GenericShip attacker, ITargetLockable defender)
        {
            bool abilityIsActive = false;
            if (defender is GenericShip)
            {
                if ((defender as GenericShip).ShipId != HostShip.ShipId)
                {
                    if ((defender as GenericShip).Owner.PlayerNo == HostShip.Owner.PlayerNo)
                    {
                        if (!DefenderAlsoHasAbility(defender as GenericShip))
                        {
                            BoardTools.DistanceInfo positionInfo = new BoardTools.DistanceInfo(attacker, HostShip);
                            if (positionInfo.Range >= attacker.TargetLockMinRange && positionInfo.Range <= attacker.TargetLockMaxRange)
                            {
                                abilityIsActive = true;
                            }
                        }
                    }
                }

                if (abilityIsActive)
                {
                    if (Roster.GetPlayer(Phases.CurrentPhasePlayer).GetType() == typeof(Players.HumanPlayer))
                    {
                        Messages.ShowErrorToHuman("Captain Kagi: You cannot target lock that ship");
                    }
                    result = false;
                }
            }
        }

        private bool DefenderAlsoHasAbility(GenericShip defender)
        {
            foreach (GenericUpgrade upgrade in defender.UpgradeBar.InstalledUpgradesAll_System)
            {
                if (upgrade.UpgradeInfo.Name.Equals(this.HostName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}