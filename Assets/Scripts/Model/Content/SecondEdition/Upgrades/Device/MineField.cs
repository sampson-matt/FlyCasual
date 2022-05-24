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
using Players;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class MineField : GenericUpgrade
    {        
        public MineField() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Mine Field",
                UpgradeType.Device,
                subType: UpgradeSubType.Remote,
                charges: 99,
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
        private int availableMines = 0;
        public override void ActivateAbility()
        {
            availableMines = setAvailableMines();
            Phases.Events.OnRoundStart += CheckAbility;
        }

        private int setAvailableMines()
        {
            int availableMines = 0;
            JSONObject campaignMission = SquadBuilderNS.CampaignLoader.CampaignMission;
            if (campaignMission.HasField("obstacles"))
            {
                JSONObject obstaclesJson = campaignMission["obstacles"];
                foreach (JSONObject obstacle in obstaclesJson.list)
                {
                    if (obstacle.HasField("type") && "mineField" == obstacle["type"].str)
                    {
                        if (obstacle.HasField("count"))
                        {
                            availableMines = Int16.Parse(obstacle["count"].str);
                        }
                        if (obstacle.HasField("squadCount"))
                        {
                            availableMines = Roster.GetPlayer(PlayerNo.Player1).Ships.Count *Int16.Parse(obstacle["squadCount"].str);
                        }
                    }
                }
            }
            return availableMines;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnRoundStart -= CheckAbility;
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
                AIShouldUseAbility,
                DeployRemote,
                descriptionLong: "Do you want to drop a mine field?",
                imageHolder: HostUpgrade,
                requiredPlayer: HostShip.Owner.PlayerNo
            );
        }

        private bool AIShouldUseAbility()
        {
            if(!Global.IsCampaignGame)
            {
                return false;
            }
            return isMineFieldAvailable();
        }

        private bool isMineFieldAvailable()
        {
            int currentMines = Roster.GetPlayer(PlayerNo.Player2).Remotes.Where(r => r.Value.GetType().Equals(typeof(Remote.MineField))).Count();
            return currentMines < availableMines;
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