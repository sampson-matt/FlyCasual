using ActionsList;
using System;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIEAdvancedX1
    {
        public class JunoEclipse : TIEAdvancedX1
        {
            public JunoEclipse() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Juno Eclipse",
                    5,
                    43,
                    pilotTitle: "Corulag's Finest",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.JunoEclipseAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }            
        }
    }
}

namespace Abilities.SecondEdition
{
    public class JunoEclipseAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= RegisterAbility;
        }

        private void RegisterAbility(GenericAction action)
        {
            RegisterAbilityTrigger(TriggerTypes.OnActionIsPerformed, AskToPerformRedBoost);
        }

        private void AskToPerformRedBoost(object sender, EventArgs e)
        {
            HostShip.AskPerformFreeAction(
                new BoostAction() { HostShip = TargetShip, Color = Actions.ActionColor.Red },
                Triggers.FinishTrigger,
                HostShip.PilotInfo.PilotName,
                "You may perform a red Boost action",
                HostShip
            );
        }
    }
}
