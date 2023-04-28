using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using Tokens;

namespace Ship
{
    namespace SecondEdition.CloneZ95Headhunter
    {
        public class Killer : CloneZ95Headhunter
        {
            public Killer() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Killer\"",
                    2,
                    25,
                    pilotTitle: "Dependable Closer",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.KillerAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class KillerAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += RegisterKillerAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsAttacker -= RegisterKillerAbility;
        }

        private void RegisterKillerAbility()
        {
            if (Combat.Defender.State.HullCurrent<=2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackStart, AskKillerAbility);
            }
        }

        protected void AskKillerAbility(object sender, System.EventArgs e)
        {
            AskToUseAbility(
                HostShip.PilotInfo.PilotName,
                AlwaysUseByDefault,
                UseKillerAbility,
                descriptionLong: "Do you want to roll 1 additional attack die, and then gain 1 deplete token?",
                imageHolder: HostShip
            );
        }

        private void UseKillerAbility(object sender, System.EventArgs e)
        {
            HostShip.AfterGotNumberOfAttackDice += IncreaseByOne;
            SubPhases.DecisionSubPhase.ConfirmDecision();
            
        }

        private void IncreaseByOne(ref int value)
        {
            value++;
            HostShip.AfterGotNumberOfAttackDice -= IncreaseByOne;
            HostShip.AfterAttackWindow += AssignDepelete;
           
        }

        private void AssignDepelete()
        {
            HostShip.AfterAttackWindow -= AssignDepelete;
            HostShip.Tokens.AssignToken(typeof(DepleteToken), delegate { });
        }
    }
}