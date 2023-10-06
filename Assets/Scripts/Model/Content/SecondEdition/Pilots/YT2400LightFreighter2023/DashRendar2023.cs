using System;
using System.Collections.Generic;
using Upgrade;
using Ship;

namespace Ship
{
    namespace SecondEdition.YT2400LightFreighter2023
    {
        public class DashRendar2023 : YT2400LightFreighter2023
        {
            public DashRendar2023() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Dash Rendar",
                    5,
                    76,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DashRendar2023Ability),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent}
                );

                PilotNameCanonical = "dashrendar-swz103";
                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/dashrendar-freighterforhire.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DashRendar2023Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCombatActivation += RegisterTriggerCombatPhase;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCombatActivation -= RegisterTriggerCombatPhase;
        }

        public void RegisterTriggerCombatPhase(GenericShip host)
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, UseAbilityCombatPhase);
        }

        private void UseAbilityCombatPhase(object sender, EventArgs e)
        {
            HostShip.IgnoreObstaclesList.AddRange(HostShip.ObstaclesLanded);
            HostShip.OnCanAttackWhileLandedOnObstacle += CanAttack;
            Phases.Events.OnCombatPhaseEnd_NoTriggers += TurnOffIgnoreObstaclesCombatPhase;
            Triggers.FinishTrigger();
        }

        private void TurnOffIgnoreObstaclesCombatPhase()
        {
            GenericShip.OnCanAttackWhileLandedOnObstacleGlobal -= CanAttack;
            HostShip.IgnoreObstaclesList.Clear();
            Phases.Events.OnCombatPhaseEnd_NoTriggers -= TurnOffIgnoreObstaclesCombatPhase;
        }

        private void CanAttack(GenericShip ship, ref bool canAttack)
        {
            canAttack = true;
        }
    }
}
