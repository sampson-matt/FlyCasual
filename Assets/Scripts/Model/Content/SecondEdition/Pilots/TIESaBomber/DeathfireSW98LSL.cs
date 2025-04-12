using ActionsList;
using Arcs;
using BoardTools;
using Bombs;
using Content;
using Movement;
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
        public class DeathfireSW98LSL : TIESaBomber
        {
            public DeathfireSW98LSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Deathfire\"",
                    2,
                    33,
                     tags: new List<Tags>
                    {
                        Tags.LsL
                    },
                    charges: 2,
                    regensCharges: 1,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DeathfireLSLAbility)
                );
                PilotNameCanonical = "deathfire-swz98-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you fully execute a speed 3-5 maneuver, if you have not dropped or launched a device this round,
    //you may spend 2 charges to drop or launch a bomb using the 3 forward template.
    public class DeathfireLSLAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnMovementFinish += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnMovementFinish -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            //AI doesn't use ability
            if (HostShip.Owner.UsesHotacAiRules) return;

            if (HostShip.AssignedManeuver.Speed >= 3 
                && HostShip.AssignedManeuver.Speed <= 5 
                && !HostShip.IsBumped && HostShip.State.Charges > 1
                && !HostShip.IsBombAlreadyDropped)
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskUseAbility);
            }
        }

        private void AskUseAbility(object sender, System.EventArgs e)
        {
            AskToUseAbility
            (
                descriptionShort: HostShip.PilotInfo.PilotName,
                descriptionLong: "Do you want to spend 2 charges to drop or launch a bomb using the [3] template?",
                useByDefault: NeverUseByDefault,
                useAbility: DropBomb,
                imageHolder: HostUpgrade
            );
        }

        private void DropBomb(object sender, System.EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            HostShip.SpendCharges(2);

            HostShip.OnGetAvailableBombDropTemplatesNoConditions += AddDropTemplate;
            HostShip.OnGetAvailableBombLaunchTemplates += AddLaunchTemplate;

            BombsManager.RegisterBombDropTriggerIfAvailable(
                HostShip,
                TriggerTypes.OnAbilityDirect,
                subType: UpgradeSubType.Bomb,
                isRealDrop: false
            );

            Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, Triggers.FinishTrigger);
        }

        private void AddDropTemplate(List<ManeuverTemplate> availableTemplates, GenericUpgrade upgrade)
        {
            if (upgrade.UpgradeInfo.SubType != UpgradeSubType.Bomb) return;
            availableTemplates.Clear();
            availableTemplates.Add(new ManeuverTemplate(ManeuverBearing.Straight, ManeuverDirection.Forward, ManeuverSpeed.Speed3, isBombTemplate: true));
        }

        protected virtual void AddLaunchTemplate(List<ManeuverTemplate> availableTemplates, GenericUpgrade upgrade)
        {
            if (upgrade.UpgradeInfo.SubType != UpgradeSubType.Bomb) return;
            availableTemplates.Clear();
            availableTemplates.Add(new ManeuverTemplate(ManeuverBearing.Straight, ManeuverDirection.Forward, ManeuverSpeed.Speed3));
        }
    }
}