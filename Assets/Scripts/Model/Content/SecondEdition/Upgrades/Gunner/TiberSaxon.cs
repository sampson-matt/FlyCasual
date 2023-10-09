using Upgrade;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradesList.SecondEdition
{
    public class TiberSaxon : GenericUpgrade
    {
        public TiberSaxon() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Tiber Saxon",
                UpgradeType.Gunner,
                cost: 5,
                isLimited: true,
                restriction: new FactionRestriction(Faction.Imperial),
                charges: 2,
                regensCharges: true,
                abilityType: typeof(Abilities.SecondEdition.TiberSaxonAbility)
            );
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TiberSaxonAbility : GenericAbility
    {
        // After you perform an attack at range 1-2 that hits, if the defender has no faceup damage cards, you may spend 1 or more Charge.
        // For each Charge you spend, the defender gains 1 strain token. 

        public override void ActivateAbility()
        {
            HostShip.OnAttackHitAsAttacker += RegisterHitAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackHitAsAttacker -= RegisterHitAbility;
        }

        private void RegisterHitAbility()
        {
            if (HostUpgrade.State.Charges > 0 && Combat.ShotInfo.Range < 3 && Combat.ShotInfo.Range > 0 && !Combat.Defender.Damage.HasFaceupCards)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackHit, AskUseAbility);
            }
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            DecisionSubPhase subphase = (DecisionSubPhase)Phases.StartTemporarySubPhaseNew(
                Name,
                typeof(TiberSaxonDecisionSubphase),
                Triggers.FinishTrigger
            );

            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "You may spend 1 or more Charges. For each Charge you spend, the defender gains 1 strain token.";
            subphase.ImageSource = HostUpgrade;

            subphase.DecisionOwner = HostShip.Owner;

            subphase.AddDecision(
                "1 Charge",
                delegate { SpendCharge(1); }
            );

            if(HostUpgrade.State.Charges > 1)
            {
                subphase.AddDecision(
                    "2 Charges",
                    delegate { SpendCharge(2); }
                );
            }

            subphase.ShowSkipButton = true;

            subphase.Start();
        }

        private void SpendCharge(int charges)
        {
            
            HostUpgrade.State.SpendCharges(charges);
            for (int i=0; i<charges; i++)
            {
                Combat.Defender.Tokens.AssignToken(typeof(Tokens.StrainToken), delegate { });
            }
            DecisionSubPhase.ConfirmDecision();
        }

        private class TiberSaxonDecisionSubphase : DecisionSubPhase { }
    }
}