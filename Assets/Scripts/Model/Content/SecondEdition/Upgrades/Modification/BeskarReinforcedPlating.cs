using Arcs;
using BoardTools;
using Content;
using Ship;
using SubPhases;
using Upgrade;
using System.Collections.Generic;
using System;

namespace UpgradesList.SecondEdition
{
    public class BeskarReinforcedPlating : GenericUpgrade, IVariableCost
    {
        public BeskarReinforcedPlating() : base()
        {
            UpgradeInfo = new UpgradeCardInfo
            (
                "Beskar Reinforced Plating",
                UpgradeType.Modification,
                cost: 6,
                restriction: new TagRestriction(Tags.Mandalorian),
                abilityType: typeof(Abilities.SecondEdition.BeskarReinforcedPlatingAbility),
                charges: 2
            );

            ImageUrl = "https://static.wikia.nocookie.net/xwing-miniatures-second-edition/images/2/2b/Beskarreinforcedplating.png";
        }

        public void UpdateCost(GenericShip ship)
        {
            Dictionary<int, int> agilityToCost = new Dictionary<int, int>()
            {
                {0, 2},
                {1, 3},
                {2, 4},
                {3, 5}
            };

            UpgradeInfo.Cost = agilityToCost[ship.ShipInfo.Agility];
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BeskarReinforcedPlatingAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnFaceupCritCardReadyToBeDealt += RegisterBeskarTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnFaceupCritCardReadyToBeDealt -= RegisterBeskarTrigger;
        }

    private void RegisterBeskarTrigger(GenericShip ship, GenericDamageCard crit, EventArgs e)
    {
            if (Combat.CurrentCriticalHitCard.IsFaceup
                && HostUpgrade.State.Charges > 0
                && IsFaceToFaceDefense())
            {
                RegisterAbilityTrigger(TriggerTypes.OnFaceupCritCardReadyToBeDealt, AskUseChewbaccaAbility);
            }
        }

        private bool IsFaceToFaceDefense()
        {
            if (!Tools.IsSameShip(HostShip, Combat.Defender)) return false;
            if (Combat.Attacker == null) return false;
            if (Combat.Defender == null) return false;

            ShotInfo shotInfo = new ShotInfo(Combat.Defender, Combat.Attacker, Combat.Defender.PrimaryWeapons);
            return shotInfo.InArcByType(ArcType.Front);
        }

        private void AskUseChewbaccaAbility(object sender, System.EventArgs e)
        {
            BeskarDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<BeskarDecisionSubPhase>("Beskar Decision Subphase", Triggers.FinishTrigger);

            subphase.DescriptionShort = HostUpgrade.UpgradeInfo.Name;
            subphase.DescriptionLong = "You may spend charges to handle faceup damage card:";
            subphase.ImageSource = HostUpgrade;

            if (HostUpgrade.State.Charges > 0) subphase.AddDecision("1 Charge: Deal facedown instead", delegate { HandleFaceupCard(1); });
            if (HostUpgrade.State.Charges > 1) subphase.AddDecision("2 Charges: Discard instead", delegate { HandleFaceupCard(2); });

            subphase.DefaultDecisionName = (HostUpgrade.State.Charges > 1) ? "2 Charges: Discard instead" : "1 Charge: Deal facedown instead";

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            subphase.Start();
        }

        private void HandleFaceupCard(int chargesSpent)
        {
            if (chargesSpent == 1)
            {
                Messages.ShowInfo($"{HostUpgrade.UpgradeInfo.Name}: Faceup damage card is dealt facedown instead");

                HostUpgrade.State.SpendCharge();
                Combat.CurrentCriticalHitCard.IsFaceup = false;
                DecisionSubPhase.ConfirmDecision();
            }
            else // chargesSpent == 2
            {
                Messages.ShowInfo($"{HostUpgrade.UpgradeInfo.Name}: Faceup damage card is discarded instead");

                HostUpgrade.State.SpendCharges(2);
                Combat.CurrentCriticalHitCard = null;
                DecisionSubPhase.ConfirmDecision();
            }
        }

        private class BeskarDecisionSubPhase : DecisionSubPhase { }
    }
}