using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ARC170Starfighter
    {
        public class Jag : ARC170Starfighter
        {
            public Jag() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Jag\"",
                    3,
                    47,
                    isLimited: true,
                    factionOverride: Faction.Republic,
                    abilityType: typeof(Abilities.SecondEdition.JagAbility)
                );

                ModelInfo.SkinName = "Red";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After a friendly ship at range 1-2 in your left or right arc defends, you may acquire a lock on the attacker.
    public class JagAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackFinishGlobal += CheckAbility;
        }
        public override void DeactivateAbility()
        {
            GenericShip.OnAttackFinishGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
    {
            var rangeLeft = HostShip.SectorsInfo.RangeToShipBySector(Combat.Defender, Arcs.ArcType.Left);
            var rangeRight = HostShip.SectorsInfo.RangeToShipBySector(Combat.Defender, Arcs.ArcType.Right);

            if (Tools.IsFriendly(Combat.Defender, HostShip)
                && ((HostShip.SectorsInfo.IsShipInSector(Combat.Defender, Arcs.ArcType.Left) && rangeLeft >= 1 && rangeLeft <= 2)
                    || (HostShip.SectorsInfo.IsShipInSector(Combat.Defender, Arcs.ArcType.Right) && rangeRight >= 1 && rangeRight <= 2)))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, delegate
                {
                    AskToUseAbility(
                        HostShip.PilotInfo.PilotName,
                        AlwaysUseByDefault,
                        AcquireLock,
                        descriptionLong: "Do you want to acquire a lock on the attacker?",
                        imageHolder: HostShip
                    );
                });                
            }
        }

        private void AcquireLock(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostName + ": Acquires lock on " + Combat.Attacker.PilotName);
            ActionsHolder.AcquireTargetLock(HostShip, Combat.Attacker, SubPhases.DecisionSubPhase.ConfirmDecision, SubPhases.DecisionSubPhase.ConfirmDecision);
        }
    }
}