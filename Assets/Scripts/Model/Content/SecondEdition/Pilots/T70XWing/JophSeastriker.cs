using Upgrade;

namespace Ship
{
    namespace SecondEdition.T70XWing
    {
        public class JophSeastriker : T70XWing
        {
            public JophSeastriker() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Joph Seastriker",
                    3,
                    45,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.JophSeastrikerAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class JophSeastrikerAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnShieldLost += RegisterJophSeastrikerAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnShieldLost -= RegisterJophSeastrikerAbility;
        }

        private void RegisterJophSeastrikerAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnShieldIsLost, GetEvadeToken);
        }

        private void GetEvadeToken(object sender, System.EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " gains Evade token");
            HostShip.Tokens.AssignToken(typeof(Tokens.EvadeToken), Triggers.FinishTrigger);
        }
    }
}