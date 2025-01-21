using Upgrade;
using Ship;
using System.Linq;
using Actions;
using ActionsList;
using System;
using System.Collections.Generic;
using SubPhases;
using SquadBuilderNS;

namespace UpgradesList.SecondEdition
{
    public class RazorCrest : GenericUpgrade
    {
        public RazorCrest() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Razor Crest",
                UpgradeType.Title,
                cost: 4,
                isLimited: true,
                addActionLink: new LinkedActionInfo(typeof(EvadeAction), typeof(BarrelRollAction), ActionColor.Red),
                restrictions: new UpgradeCardRestrictions(
                    new ShipRestriction(typeof(Ship.SecondEdition.ST70AssaultShip.ST70AssaultShip)),
                    new FactionRestriction(Faction.Scum)
                ),
                abilityType: typeof(Abilities.SecondEdition.RazorCrestAbility)
            );

            ImageUrl = "https://infinitearenas.com/xw2/images/upgrades/razorcrest.png";
        }
    }
}

namespace Abilities.SecondEdition
{    
    public class RazorCrestAbility : GenericAbility
    {
        private GenericUpgrade hiddenUpgrade;
        public override void ActivateAbility()
        {
            Phases.Events.OnSetupEnd += RegisterRazorCrestAbility;
            HostShip.OnSystemsPhaseStart += RegisterOwnAbilityTrigger;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnSetupEnd -= RegisterRazorCrestAbility;
            HostShip.OnSystemsPhaseStart -= RegisterOwnAbilityTrigger;
        }

        private void RegisterOwnAbilityTrigger(GenericShip ship)
        {
            if (hiddenUpgrade != null)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsPhaseStart, AskToUseTitleAbility);
            }            
        }

        private void AskToUseTitleAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                AlwaysUseByDefault,
                equipUpgrade,
                descriptionLong: "Do you want to reveal your illicit upgrade and equip it?",
                imageHolder: HostUpgrade
            );
        }

        private void equipUpgrade(object sender, EventArgs e)
        {
            hiddenUpgrade.PreAttachToShip(HostShip);
            hiddenUpgrade.AttachToShip(HostShip);
            Roster.UpdateUpgradesPanel(HostShip, HostShip.InfoPanel);
            Roster.SubscribeUpgradesPanel(HostShip, HostShip.InfoPanel);
            hiddenUpgrade = null;
            DecisionSubPhase.ConfirmDecisionNoCallback();
            Triggers.FinishTrigger();
        }

        private void RegisterRazorCrestAbility()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = HostShip.ShipId + ": Select Illicit Upgrade",
                TriggerType = TriggerTypes.OnSetupEnd,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = SelectIllicitUpgrade,
            });
        }

        private void SelectIllicitUpgrade(object sender, EventArgs e)
        {
            if(HostShip.UpgradeBar.HasFreeUpgradeSlot(new List<UpgradeType>() { UpgradeType.Illicit }))
            {
                SelectIllicitUpgradeDecisionSubphase subphase = Phases.StartTemporarySubPhaseNew<SelectIllicitUpgradeDecisionSubphase>(
               "Choose illicit upgrade",
               Triggers.FinishTrigger
                );

                subphase.DescriptionShort = "Razor Crest";
                subphase.DescriptionLong = "Choose illicit upgrade";
                subphase.ImageSource = HostUpgrade;

                subphase.RequiredPlayer = HostShip.Owner.PlayerNo;
                subphase.ShowSkipButton = false;

                if (Global.SquadBuilder.Database.AllUpgrades.Count == 0)
                {
                    Global.SquadBuilder.GenerateDatabase();
                }

                foreach (UpgradeRecord upgradeType in Global.SquadBuilder.Database.AllUpgrades.Where(n => n.Instance.HasType(UpgradeType.Illicit) && !n.Instance.UpgradeInfo.IsLimited && n.Instance.UpgradeInfo.Restrictions.IsAllowedForShip(HostShip)
                     && n.Instance.IsAllowedForShip(HostShip) && ShipDoesntHaveUpgradeWithSameName(HostShip, n.Instance)).ToList())
                {
                    GenericUpgrade illicitUpgrade = (GenericUpgrade)System.Activator.CreateInstance(Type.GetType(upgradeType.UpgradeTypeName));
                    subphase.AddDecision(
                        illicitUpgrade.UpgradeInfo.Name,
                        delegate { doWithIllicitUpgrade(illicitUpgrade); },
                        illicitUpgrade.ImageUrl
                    );
                }

                subphase.DefaultDecisionName = subphase.GetDecisions().First().Name;

                subphase.Start();
            }
        }

        private bool ShipDoesntHaveUpgradeWithSameName(GenericShip ship, GenericUpgrade upgrade)
        {
            return !ship.UpgradeBar.GetUpgradesAll().Any(n => n.UpgradeInfo.Name == upgrade.UpgradeInfo.Name);
        }

        private void doWithIllicitUpgrade(GenericUpgrade illicitUpgrade)
        {
            hiddenUpgrade = illicitUpgrade;
            SelectIllicitUpgradeDecisionSubphase.ConfirmDecision();
        }

        public class SelectIllicitUpgradeDecisionSubphase : DecisionSubPhase { }
    }
}