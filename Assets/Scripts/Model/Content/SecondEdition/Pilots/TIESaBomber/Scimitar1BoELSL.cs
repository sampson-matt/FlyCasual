using ActionsList;
using Bombs;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIESaBomber
    {
        public class Scimitar1BoELSL : TIESaBomber
        {
            public Scimitar1BoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Scimitar 1",
                    3,
                    36,
                     tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    isLimited: true,
                    charges: 2,
                    abilityType: typeof(Abilities.SecondEdition.Scimitar1Ability),
                    extraUpgradeIcon: UpgradeType.Talent
                );
                PilotNameCanonical = "scimitar1-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After a friendly ship at range 0-3 performs an attack, you may spend 1 charge to acquire a lock on the defender.
    public class Scimitar1Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackFinishGlobal += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackFinishGlobal -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            var range = new BoardTools.DistanceInfo(HostShip, Combat.Attacker).Range;

            if (HostShip.State.Charges > 0 
                && Tools.IsFriendly(Combat.Attacker, HostShip)
                && range >= 0 && range <= 3)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskAcquireLock);
            }
        }

        private void AskAcquireLock(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                AlwaysUseByDefault,
                AcquireLock,
                descriptionLong: "Do you want to acquire a lock on the defender?",
                imageHolder: HostShip
            );
        }

        private void AcquireLock(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostName + ": Acquires lock on " + Combat.Defender.PilotInfo.PilotName);
            ActionsHolder.AcquireTargetLock(HostShip, Combat.Defender, CleanUp, CleanUp);
        }

        private void CleanUp()
        {
            HostShip.SpendCharge();
            SubPhases.DecisionSubPhase.ConfirmDecision();
        }
    }
}