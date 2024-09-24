using Ship;
using Upgrade;
using SubPhases;
using Conditions;
using System.Linq;
using Tokens;
using ActionsList;
using System;
using Actions;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class SatineKryze : GenericUpgrade
    {
        public SatineKryze()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Satine Kryze",
                UpgradeType.Crew,
                cost: 7,
                isLimited: true,                
                restriction: new FactionRestriction(Faction.Republic),
                abilityType: typeof(Abilities.SecondEdition.SatineKryzeAbility),
                charges: 2,
                regensCharges: true
            );
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class SatineKryzeAbility : GenericAbility
    {
        //At the start of the Engagement Phase, you may spend 2 Charge.
        //If you do, each friendly ship may choose to gain 1 deplete token and 1 focus token or to gain 1 disarm token and 1 evade token. 
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterAbility;
        }

        private void RegisterAbility()
        {
            if (HostUpgrade.State.Charges >= 2)
            {
                RegisterAbilityTrigger(TriggerTypes.OnSystemsAbilityActivation, AskToUseSatineAbility);
            }
        }

        private void AskToUseSatineAbility(object sender, EventArgs e)
        {
            AskToUseAbility(
                HostUpgrade.UpgradeInfo.Name,
                NeverUseByDefault, 
                UseAbility,
                dontUseAbility: delegate { DecisionSubPhase.ConfirmDecision(); },                
                descriptionLong: "Do you want to spend 2 Charges? (If you do, each friendly ship may gain 1 deplete token and 1 focus token or gain 1 disarm token and 1 evade token.)",
                imageHolder: HostUpgrade
            );
        }

        protected void UseAbility(object sender, EventArgs e)
        {
            HostUpgrade.State.SpendCharges(2);
            var friendlies = HostShip.Owner.Ships.Values                
                .Where(f => Tools.IsFriendly(f, HostShip))
                .ToArray();
            foreach (var friendly in friendlies)
            {
                Triggers.RegisterTrigger(
                    new Trigger()
                    {
                        Name = friendly.PilotInfo.PilotName + " (" + friendly.ShipId + ") " + HostUpgrade.UpgradeInfo.Name + " ability",
                        TriggerOwner = HostShip.Owner.PlayerNo,
                        TriggerType = TriggerTypes.OnAbilityDirect,
                        EventHandler = AskFriendlyToUseSatineAbility,
                        Sender = HostShip,
                        EventArgs = new SatineAbilityEventArgs
                        {
                            friendlyShip = friendly
                        }
                    }
                );
            }
            Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, DecisionSubPhase.ConfirmDecision);
        }

        public class SatineAbilityEventArgs : EventArgs
        {
            public GenericShip friendlyShip;
        }

        protected void AskFriendlyToUseSatineAbility(object sender, EventArgs e)
        {
            var args = e as SatineAbilityEventArgs;
            var ship = args.friendlyShip;

            SelectTokensToGainSubphase subphase = Phases.StartTemporarySubPhaseNew<SelectTokensToGainSubphase>(
               "Gain Tokens",
               Triggers.FinishTrigger
           );

            subphase.Name = HostUpgrade.UpgradeInfo.Name;
            subphase.DescriptionShort = ship.PilotInfo.PilotName + " (" + ship.ShipId + ") Select tokens to gain";
            subphase.ImageSource = HostUpgrade;

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            subphase.AddDecision(
                "1 Deplete and 1 Focus", delegate { ship.Tokens.AssignToken(typeof(DepleteToken), delegate { }); ship.Tokens.AssignToken(typeof(FocusToken), DecisionSubPhase.ConfirmDecision); });
            subphase.AddDecision(
                "1 Disarm and 1 Evade", delegate { ship.Tokens.AssignToken(typeof(WeaponsDisabledToken), delegate { }); ship.Tokens.AssignToken(typeof(EvadeToken), DecisionSubPhase.ConfirmDecision); });

            subphase.Start();
        }

        private class SelectTokensToGainSubphase : DecisionSubPhase { }
    }
}