using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;
using System;
using Bombs;
using BoardTools;
using Movement;
using SubPhases.SecondEdition;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class MineField : GenericUpgrade
    {        
        public MineField() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Mine Field",
                UpgradeType.Device,
                subType: UpgradeSubType.Remote,
                charges: 2,
                cannotBeRecharged: true,
                cost: 5,
                isLimited: false,
                restriction: new FactionRestriction(Faction.Imperial),
                abilityType: typeof(Abilities.SecondEdition.MineFieldDeployAbility),
                remoteType: typeof(Remote.MineField)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/payload/minefield.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MineFieldDeployAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnRoundStart += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnSystemsPhaseStart -= CheckAbility;
            Phases.Events.OnSystemsPhaseStart -= RegisterOwnAbilityTrigger;
        }

        private void CheckAbility()
        {
            if (HostUpgrade.State.Charges > 0)
            {
                Phases.Events.OnSystemsPhaseStart += RegisterOwnAbilityTrigger;
            }
        }

        private void RegisterOwnAbilityTrigger()
        {
            Phases.Events.OnSystemsPhaseStart -= RegisterOwnAbilityTrigger;

            RegisterAbilityTrigger(TriggerTypes.OnSystemsPhaseStart, AskToUseOwnAbility);
        }

        private void AskToUseOwnAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                NeverUseByDefault,
                DeployRemote,
                descriptionLong: "Do you want to drop a mine field?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private void DeployRemote(object sender, EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();

            BombsManager.RegisterBombDropTriggerIfAvailable(
                HostShip,
                TriggerTypes.OnAbilityDirect,
                type: HostUpgrade.GetType()
            );

            Triggers.ResolveTriggers(
                TriggerTypes.OnAbilityDirect,
                delegate {
                    HostUpgrade.State.SpendCharge();
                    Triggers.FinishTrigger();
                }
            );
        }
    }
}