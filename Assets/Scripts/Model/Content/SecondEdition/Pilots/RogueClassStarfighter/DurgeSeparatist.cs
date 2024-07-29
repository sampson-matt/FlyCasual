using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class DurgeSeparatist : RogueClassStarfighter
        {
            public DurgeSeparatist() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Durge",
                    5,
                    43,
                    charges: 1,
                    pilotTitle: "On His Own Time",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.DurgeSeparatistAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Separatists
                );
                PilotNameCanonical = "durge-separatistalliance";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class DurgeSeparatistAbility : GenericAbility
    {
        //private Boolean preventDestruction = false;
        public override void ActivateAbility()
        {
            HostShip.OnBeforeCheckPreventDestruction += RegisterAbility;
        }


        public override void DeactivateAbility()
        {
            HostShip.OnBeforeCheckPreventDestruction += RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship)
        {
            if (HostShip.State.Charges > 0)// && HostShip.AssignedDamageDiceroll.Count == 0 && HostShip.Damage.CountAssignedDamage() >= HostShip.ShipInfo.Hull)
            {
                RegisterAbilityTrigger(TriggerTypes.OnShipIsDestroyedCheck, CheckAbility);
            }
        }

        private void CheckAbility(object sender, EventArgs e)
        {
            if (HostShip.State.Charges > 0)
            {
                AskToUseAbility(HostShip.PilotInfo.PilotName,
                    AlwaysUseByDefault,
                    UseAbility,
                    dontUseAbility: delegate { },
                    descriptionLong: "Do you want to spend 1 charge to Charge to reveal all of your facedown damage, discard each Direct Hit! and each of your damage cards with the Pilot trait, then repair all of your faceup damage cards?",
                    imageHolder: HostShip,
                    requiredPlayer: HostShip.Owner.PlayerNo
                );
            }
        }

        private void UseAbility(object sender, System.EventArgs e)
        {
            SubPhases.DecisionSubPhase.ConfirmDecisionNoCallback();
            HostShip.SpendCharge();

            HostShip.OnBeforeCheckPreventDestruction -= RegisterAbility;

            List<GenericDamageCard> cardsToDiscard = HostShip.Damage.GetFacedownCards().FindAll(d => d.Type == CriticalCardType.Pilot || d.Name == "Direct Hit");
            //foreach (GenericDamageCard card in HostShip.Damage.GetFacedownCards())
            //{
            //    Messages.ShowInfo(HostShip.PilotInfo.PilotName + " revealed: " + card.Name + " type:" + card.Type);
            //}
            //Messages.ShowInfo(HostShip.PilotInfo.PilotName + " revealed " + cardsToDiscard.Count + " Pilot trait and Direct Hit! cards.");

            cardsToDiscard.AddRange(HostShip.Damage.GetFaceupCrits().FindAll(d => d.Type == CriticalCardType.Pilot || d.Name == "Direct Hit"));

            Messages.ShowInfo(HostShip.PilotInfo.PilotName + " discarded " + cardsToDiscard.Count + " cards.");

            int remainingDamage = HostShip.Damage.GetFacedownCards().Count + HostShip.Damage.GetFaceupCrits().Count - cardsToDiscard.Count;

            //Messages.ShowInfo(HostShip.PilotInfo.PilotName + " remaining damage: " + remainingDamage);

            if (cardsToDiscard.Count > 0 && remainingDamage < HostShip.ShipInfo.Hull)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has prevented his own destruction");
                HostShip.IsDestroyed = false;
                foreach(GenericDamageCard card in cardsToDiscard)
                {
                    HostShip.Damage.DamageCards.Remove(card);
                    HostShip.CallAfterAssignedDamageIsChanged();
                }

                foreach(GenericDamageCard card in HostShip.Damage.GetFaceupCrits())
                {
                    HostShip.Damage.FlipFaceupCritFacedown(card, delegate { });
                }
                HostShip.OnCheckPreventDestruction += PreventDestruction;
            }
            Triggers.FinishTrigger();
        }

        private void PreventDestruction(GenericShip ship, ref bool preventDestruction)
        {
            preventDestruction = true;
            HostShip.OnCheckPreventDestruction -= PreventDestruction;
        }
    }
}

