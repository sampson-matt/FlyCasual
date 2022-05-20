using Upgrade;
using System;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class Calculation : GenericUpgrade
    {
        public Calculation() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Calculation",
                UpgradeType.Talent,
                cost: 2,
                abilityType: typeof(Abilities.SecondEdition.CalculationAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/talent/calculation.png";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class CalculationAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterCalculationAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterCalculationAbility;
        }

        private void RegisterCalculationAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, OnCombatAssignCalculate);
        }

        private void OnCombatAssignCalculate(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " gains Calculate token");
            HostShip.Tokens.AssignToken(typeof(CalculateToken), Triggers.FinishTrigger);
        }
    }
}