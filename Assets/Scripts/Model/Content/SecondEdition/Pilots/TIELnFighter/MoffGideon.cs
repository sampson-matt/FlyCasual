using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;
using System;
using Tokens;
using Editions;
using SubPhases;
using Abilities.SecondEdition;
using Upgrade;
using BoardTools;
using ActionsList;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class MoffGideon : TIELnFighter
        {
            public MoffGideon() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Moff Gideon",
                    4,
                    31,
                    pilotTitle: "Ruthless Remnant Leader",
                    isLimited: true,
                    abilityType: typeof(MoffGideonAbility),
                    charges: 2,
                    regensCharges: 1,
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent }
                );

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/moffgideon.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MoffGideonAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            GenericShip.OnAttackStartAsAttackerGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackStartAsAttackerGlobal -= CheckAbility;
        }

        private void CheckAbility()
        {
            bool IsDifferentPlayer = (HostShip.Owner.PlayerNo != Combat.Defender.Owner.PlayerNo);
            DistanceInfo distanceInfo = new DistanceInfo(HostShip, Combat.Defender);

            if (IsDifferentPlayer 
                &&  distanceInfo.Range <= 3 
                && distanceInfo.Range >= 1
                && HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackStart, AskChooseFriendlyShip);
            }
        }

        private void AskChooseFriendlyShip(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                PayForAbility,
                FilterTargets,
                GetFriendlyTargetPriority,
                HostShip.Owner.PlayerNo,
                name: HostShip.PilotInfo.PilotName,
                description: "You may spend a charge token and choose a friendly ship at range 0-1 of the defender to gain a strain token. If you do, defense dice cannot be modified. ",
                imageSource: HostShip
            );
        }

        private void PayForAbility()
        {
            HostShip.SpendCharge();
            TargetShip.Tokens.AssignToken(typeof(StrainToken), SelectShipSubPhase.FinishSelection);
            Combat.Defender.OnTryAddAvailableDiceModification += PreventDiceModification;

        }

        private void PreventDiceModification(GenericShip ship, GenericAction action, ref bool canBeUsed)
        {
            if (Combat.AttackStep == CombatStep.Defence && Combat.Defender.ShipId == ship.ShipId)
            {
                ship.OnTryAddAvailableDiceModification -= PreventDiceModification;
                Messages.ShowInfo("Moff Gideon: The defender cannot use " + action.DiceModificationName);
                canBeUsed = false;
            }
            
        }

        private bool FilterTargets(GenericShip ship)
        {
            DistanceInfo distanceInfo = new DistanceInfo(ship, Combat.Defender);
            return distanceInfo.Range < 2 && Tools.IsFriendly(ship, HostShip);
        }

        private int GetFriendlyTargetPriority(GenericShip ship)
        {
            return 100-ship.PilotInfo.Cost;
        }
    }
}