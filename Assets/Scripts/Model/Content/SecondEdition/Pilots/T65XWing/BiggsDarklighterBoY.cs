using Abilities.SecondEdition;
using System.Collections.Generic;
using Ship;
using SubPhases;
using BoardTools;
using Content;

namespace Ship
{
    namespace SecondEdition.T65XWing
    {
        public class BiggsDarklighterBoY : T65XWing
        {
            public BiggsDarklighterBoY() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Biggs Darklighter",
                    3,
                    44,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoY
                    },
                    abilityType: typeof(Abilities.SecondEdition.BiggsDarklighterBoYAbility)
                );
                ShipAbilities.Add(new HopeAbility());
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/BattleOfYavin/biggsdarklighter-boy.png";
                PilotNameCanonical = "biggsdarklighter-boy";
                ModelInfo.SkinName = "Biggs Darklighter";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BiggsDarklighterBoYAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation += CheckForAbility;
            HostShip.OnSystemsAbilityActivation += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation -= CheckForAbility;
            HostShip.OnSystemsAbilityActivation -= RegisterAbility;
        }

        private void CheckForAbility(GenericShip ship, ref bool flag)
        {
            if (Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 1), Team.Type.Friendly).Count > 0) flag = true;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(1, 1), Team.Type.Friendly).Count > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToUseBiggsAbility);
            }
        }

        private void AskToUseBiggsAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                    ChangeInitiative,
                    FilterAbilityTargets,
                    GetAiAbilityPriority,
                    HostShip.Owner.PlayerNo,
                    HostName,
                    "You may choose 1 friendly ship at range 1, treat your initiative as equal to the chosen ship's until the end of the Activation Phase",
                    HostShip
                );
        }

        private void ChangeInitiative()
        {
            new BiggsPilotSkillModifier(HostShip, TargetShip.State.Initiative);
            MovementTemplates.ReturnRangeRuler();

            SelectShipSubPhase.FinishSelection();
        }

        private class BiggsPilotSkillModifier : IModifyPilotSkill
        {
            private GenericShip host;
            private int newPilotSkill;

            public BiggsPilotSkillModifier(GenericShip host, int newPilotSkill)
            {
                this.host = host;
                this.newPilotSkill = newPilotSkill;

                host.State.AddPilotSkillModifier(this);
                Phases.Events.OnActivationPhaseEnd_NoTriggers += RemoveBiggsModifieer;
            }

            public void ModifyPilotSkill(ref int pilotSkill)
            {
                pilotSkill = newPilotSkill;
            }

            private void RemoveBiggsModifieer()
            {
                host.State.RemovePilotSkillModifier(this);

                Phases.Events.OnActivationPhaseEnd_NoTriggers -= RemoveBiggsModifieer;
            }
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int priority = 0;

            priority += ship.State.Initiative;

            if (ship.State.Initiative < HostShip.State.Initiative) priority = 0;

            return priority;
        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            var range = new DistanceInfo(HostShip, ship).Range;
            return Tools.IsFriendly(ship, HostShip) && range == 1;
        }
    }
}
