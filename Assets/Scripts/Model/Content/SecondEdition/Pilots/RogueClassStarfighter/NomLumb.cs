using System.Collections.Generic;
using Upgrade;
using System;
using SubPhases;
using Ship;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class NomLumb : RogueClassStarfighter
        {
            public NomLumb() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Nom Lumb",
                    1,
                    35,
                    pilotTitle: "Laughing Bandit",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.NomLumbRogueAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );

                PilotNameCanonical = "nomlumb-rogueclassstarfighter";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class NomLumbRogueAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += PlanNomLumbPilotAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= PlanNomLumbPilotAbility;
        }

        private void PlanNomLumbPilotAbility()
        {
            var trigger = RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, NomLumbPilotAbility);
            trigger.Name = $"{HostName} - {HostShip.PilotInfo.PilotName} (#{HostShip.ShipId})";
        }

        private void NomLumbPilotAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                SelectNomLumbTarget,
                FilterAbilityTargets,
                GetAiAbilityPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                "Select 1 Enemy ship in your forward arc, if you do, treat your initiative as equal to that ship's until the end of the round.",
                HostShip
            );
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            BoardTools.DistanceInfo distanceInfo = new BoardTools.DistanceInfo(HostShip, ship);
            return (HostShip.SectorsInfo.IsShipInSector(ship, Arcs.ArcType.Front)) && (ship.Owner != HostShip.Owner);
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int result = 0;
            if (ActionsHolder.HasTarget(ship)) result += 100;
            result += (ship.State.Initiative);

            if (ship.State.Initiative <= HostShip.State.Initiative) result = 0;

            return result;
        }

        private void SelectNomLumbTarget()
        {
            new NomLumbPilotSkillModifier(HostShip, TargetShip.State.Initiative);
            MovementTemplates.ReturnRangeRuler();

            SelectShipSubPhase.FinishSelection();
        }

        private class NomLumbPilotSkillModifier : IModifyPilotSkill
        {
            private GenericShip host;
            private int newPilotSkill;

            public NomLumbPilotSkillModifier(GenericShip host, int newPilotSkill)
            {
                this.host = host;
                this.newPilotSkill = newPilotSkill;

                host.State.AddPilotSkillModifier(this);
                Phases.Events.OnEndPhaseStart_NoTriggers += RemoveNomLumbModifieer;
            }

            public void ModifyPilotSkill(ref int pilotSkill)
            {
                pilotSkill = newPilotSkill;
            }

            private void RemoveNomLumbModifieer()
            {
                host.State.RemovePilotSkillModifier(this);

                Phases.Events.OnEndPhaseStart_NoTriggers -= RemoveNomLumbModifieer;
            }
        }
    }
}