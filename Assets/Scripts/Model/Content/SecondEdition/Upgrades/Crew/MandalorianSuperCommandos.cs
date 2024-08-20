using Ship;
using Upgrade;
using System.Collections.Generic;
using System;
using Bombs;
using BoardTools;
using Movement;
using SubPhases;
using Remote;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class MandalorianSuperCommandos : GenericUpgrade
    {
        public MandalorianSuperCommandos() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Mandalorian Super Commandos",
                types: new List<UpgradeType>()
                {
                    UpgradeType.Crew,
                    UpgradeType.Crew
                },
                subType: UpgradeSubType.Remote,
                cost: 10,
                isLimited: true,
                charges: 2,
                cannotBeRecharged: true,
                restrictions: new UpgradeCardRestrictions(
                    new FactionRestriction(Faction.Scum),
                    new BaseSizeRestriction(BaseSize.Medium, BaseSize.Large)
                ),
                abilityType: typeof(Abilities.SecondEdition.CommandosAbility),
                remoteType: typeof(Remote.CommandoTeam)
            );
        }
        public override List<ManeuverTemplate> GetDefaultDropTemplates()
        {
            return new List<ManeuverTemplate>()
            {
                new ManeuverTemplate(ManeuverBearing.Straight, ManeuverDirection.Forward, ManeuverSpeed.Speed1, isBombTemplate: true)
            };
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CommandosAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation += CheckAbility;
            HostShip.OnSystemsAbilityActivation += RegisterOwnAbilityTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCheckSystemsAbilityActivation -= CheckAbility;
            HostShip.OnSystemsAbilityActivation -= RegisterOwnAbilityTrigger;
        }

        private void CheckAbility(GenericShip ship, ref bool flag)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                flag = true;
            }
        }

        private void RegisterOwnAbilityTrigger(GenericShip ship)
        {
            if (HostUpgrade.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToUseOwnAbility);
            }            
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                NeverUseByDefault,
                AskRemoteDirection,
                descriptionLong: "Do you want to drop a Commando Team remote?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void AskRemoteDirection(object sender, EventArgs e)
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();

            RemoteDirectionDecisionSubphase templateDecisionSubphase = Phases.StartTemporarySubPhaseNew<RemoteDirectionDecisionSubphase>(
                "Template decision",
                delegate { }
            );

            templateDecisionSubphase.DescriptionShort = "Select template";
            templateDecisionSubphase.DecisionOwner = HostShip.Owner;

            templateDecisionSubphase.ShowSkipButton = false;

            templateDecisionSubphase.AddDecision(
                "Front Guides",
                delegate { DeployRemote(true); }
            );

            templateDecisionSubphase.AddDecision(
                "Rear Guides",
                delegate { DeployRemote(false); }
            );

            templateDecisionSubphase.DefaultDecisionName = "Front Guides";

            templateDecisionSubphase.Start();
        }

        private void DeployRemote(Boolean useFrontGuides)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            BombsManager.RegisterBombDropTriggerIfAvailable(
                HostShip,
                TriggerTypes.OnAbilityDirect,
                type: HostUpgrade.GetType(),
                useFrontGuides: useFrontGuides
            );

            Triggers.ResolveTriggers(
                TriggerTypes.OnAbilityDirect,
                delegate {
                    HostUpgrade.State.SpendCharge();
                    Triggers.FinishTrigger();
                }
            );
        }

        private class RemoteDirectionDecisionSubphase : DecisionSubPhase { }
    }
}
