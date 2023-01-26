
using System;
using System.Collections.Generic;
using Upgrade;
using Ship;

namespace Ship
{
    namespace SecondEdition.GauntletFighter
    {
        public class GarSaxon : GauntletFighter
        {
            public GarSaxon() : base()
            {
                PilotInfo = new PilotCardInfo
                (
                    "Gar Saxon",
                    3,
                    59,
                    charges: 2,
                    regensCharges: 1,
                    pilotTitle: "Treacherous Viceroy",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.GarSaxonAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent},
                    factionOverride: Faction.Imperial
                );

                ModelInfo.SkinName = "CIS Dark";

                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Title);

                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/garsaxon.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GarSaxonAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnAttackStartAsAttackerGlobal += RegisterGarSaxonAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnAttackStartAsAttackerGlobal -= RegisterGarSaxonAbility;
        }

        protected void RegisterGarSaxonAbility()
        {
            if (Tools.IsFriendly(Combat.Attacker, HostShip) 
                && Combat.Attacker.ShipId != HostShip.ShipId 
                && Combat.ChosenWeapon.WeaponType == WeaponTypes.PrimaryWeapon 
                && Combat.Defender.SectorsInfo.IsShipInSector(Combat.Attacker, Arcs.ArcType.Rear))
            {
                BoardTools.DistanceInfo distanceInfo = new BoardTools.DistanceInfo(Combat.Attacker, HostShip);
                if (distanceInfo.Range < 3)
                {
                    RegisterAbilityTrigger(TriggerTypes.OnAttackStart, AskGarSaxonAbility);
                }
            }
        }
        protected void AskGarSaxonAbility(object sender, System.EventArgs e)
        {
            if (HostShip.State.Charges > 0)
            {
                AskToUseAbility(
                    HostShip.PilotInfo.PilotName,
                    AlwaysUseByDefault,
                    UseGarSaxonAbility,
                    descriptionLong: "Do you want to spend 1 charge to allow attacker to roll 1 additional attack die?",
                    imageHolder: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void UseGarSaxonAbility(object sender, System.EventArgs e)
        {
            HostShip.SpendCharge();
            AllowRollAdditionalDice();
        }

        private void AllowRollAdditionalDice()
        {
            Combat.Attacker.AfterGotNumberOfAttackDice += IncreaseByOne;
            SubPhases.DecisionSubPhase.ConfirmDecision();
        }

        private void IncreaseByOne(ref int value)
        {
            value++;
            Combat.Attacker.AfterGotNumberOfAttackDice -= IncreaseByOne;
        }
    }
}