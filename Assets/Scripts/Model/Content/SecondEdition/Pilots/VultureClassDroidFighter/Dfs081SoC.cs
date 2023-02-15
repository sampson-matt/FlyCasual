using Ship;
using System.Collections.Generic;
using System.Linq;
using Content;
using System;
using SubPhases;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class Dfs081SoC : VultureClassDroidFighter
    {
        public Dfs081SoC()
        {
            PilotInfo = new PilotCardInfo(
                "DFS-081",
                3,
                23,
                true,
                charges: 2,
                abilityType: typeof(Abilities.SecondEdition.Dfs081SoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC
                },
                pilotTitle: "Siege of Coruscant"
            );
            PilotNameCanonical = "dfs081-siegeofcoruscant";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/dfs081-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    //While you defend, you may spend 1 charge and one calculate token to cancel 1 crit result. 
    
    public class Dfs081SoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnTryDamagePrevention += RegisterDefendAbility;
        }

        private void RegisterDefendAbility(GenericShip ship, DamageSourceEventArgs e)
        {
            if (e.DamageType == DamageTypes.ShipAttack)
            {
                RegisterAbilityTrigger(TriggerTypes.OnTryDamagePrevention, AskToUseDefendAbility);
            }
        }

        private void AskToUseDefendAbility(object sender, EventArgs e)
        {
            if (HostShip.State.Charges > 0 && Combat.DiceRollAttack.CriticalSuccesses >= 1)
            {
                var phase = (DecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                    Name,
                    typeof(DecisionSubPhase),
                    Triggers.FinishTrigger
                );

                phase.DescriptionShort = HostName;
                phase.DescriptionLong = "You may spend 1 charge and 1 calculate token to cancel 1 Hit or Crit result";
                phase.ImageSource = HostShip;

                if (HostShip.AssignedDamageDiceroll.CriticalSuccesses > 0)
                {
                    phase.AddDecision("Cancel Crit result", delegate { PreventDamage(DieSide.Crit); });
                }

                //phase.AddDecision("No", delegate { DecisionSubPhase.ConfirmDecision(); });
                phase.DefaultDecisionName = phase.GetDecisions().Last().Name;
                phase.ShowSkipButton = true;
                phase.DecisionOwner = HostShip.Owner;
                phase.Start();
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void PreventDamage(DieSide type)
        {
            if (HostShip.State.Charges > 0)
            {
                Die dieToRemove = HostShip.AssignedDamageDiceroll.DiceList.Find(n => n.Side == type);
                HostShip.AssignedDamageDiceroll.DiceList.Remove(dieToRemove);
                HostShip.Tokens.SpendToken(typeof(Tokens.CalculateToken), delegate { });
                HostShip.SpendCharge();
                Messages.ShowInfo($"{HostName} cancels 1 {(type == DieSide.Crit ? "Crit" : "Hit")} result");
            }

            DecisionSubPhase.ConfirmDecision();
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTryDamagePrevention -= RegisterDefendAbility;
        }
    }
}