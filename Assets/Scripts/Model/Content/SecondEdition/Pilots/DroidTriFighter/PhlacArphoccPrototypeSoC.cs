using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using Tokens;
using Upgrade;
using Content;

namespace Ship.SecondEdition.DroidTriFighter
{
    public class PhlacArphoccPrototypeSoC : DroidTriFighter
    {
        public PhlacArphoccPrototypeSoC()
        {
            PilotInfo = new PilotCardInfo(
                "Phlac-Arphocc Prototype",
                5,
                40,
                limited: 2,
                extraUpgradeIcon: UpgradeType.Talent,
                abilityType: typeof(Abilities.SecondEdition.PhlacArphoccPrototypeSoCAbility),
                tags: new List<Tags>
                {
                    Tags.SoC
                },
                pilotTitle: "Siege of Coruscant"
            );

            PilotNameCanonical = "phlacarphoccprototype-soc";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/phlacarphoccprototype-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class PhlacArphoccPrototypeSoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterPhlacArphoccPrototypeSoCAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterPhlacArphoccPrototypeSoCAbility;
        }

        private void RegisterPhlacArphoccPrototypeSoCAbility()
        {
            if (BoardTools.Board.GetShipsInBullseyeArc(HostShip, Team.Type.Enemy).Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, OnCombatAssignCalculate);
            }
        }

        private void OnCombatAssignCalculate(object sender, EventArgs e)
        {
            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has an enemy ship in Bullseye arc and gains a Calculate token");
            HostShip.Tokens.AssignToken(typeof(CalculateToken), Triggers.FinishTrigger);
        }
    }
}