using BoardTools;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace UpgradesList.SecondEdition
{
    public class WolfpackSoC : GenericUpgrade
    {
        public WolfpackSoC() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Wolfpack",
                types: new List<UpgradeType>() { UpgradeType.Crew, UpgradeType.Gunner },
                cost: 0,
                isLimited: true,
                abilityType: typeof(Abilities.SecondEdition.WolfpackSoCAbility),
                restriction: new FactionRestriction(Faction.Republic)
            );

            NameCanonical = "wolfpack-soc";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class WolfpackSoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
               HostUpgrade.UpgradeInfo.Name,
               CheckIsAvailable,
               GetAiPriority,
               DiceModificationType.Reroll,
               int.MaxValue,
               payAbilityCost: PayAbilityCost
           );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        private bool CheckIsAvailable()
        {
            return Combat.AttackStep == CombatStep.Attack && IsLockedByAnotherFriendly(Combat.Defender);
        }

        private bool IsLockedByAnotherFriendly(GenericShip ship)
        {
            return ship.Tokens.GetAllTokens()
                .Where(n => n is RedTargetLockToken)
                .Where(n => isFriendlyPloKoonOrBornForThis((n as RedTargetLockToken).OtherTargetLockTokenOwner as GenericShip))
                .Where(n => ((n as RedTargetLockToken).OtherTargetLockTokenOwner as GenericShip).ShipId != HostShip.ShipId)
                .Count() != 0;
        }

        private bool HasLockOnDefenderAndIsFriendly(GenericShip ship)
        {
            if (!Tools.IsFriendly(ship, HostShip)) return false;
            if (ship.ShipId == HostShip.ShipId) return false;
            if (!isFriendlyPloKoonOrBornForThis(ship)) return false;

            return ship.Tokens.GetAllTokens()
                .Where(n => n is BlueTargetLockToken)
                .Where(n => ((n as BlueTargetLockToken).OtherTargetLockTokenOwner as GenericShip).ShipId == Combat.Defender.ShipId)
                .Count() != 0;
        }

        private bool isFriendlyPloKoonOrBornForThis(GenericShip ship)
        {
            if (!Tools.IsFriendly(ship, HostShip)) return false;
            if (ship.ShipAbilities.Any(n => n.GetType() == typeof(Abilities.SecondEdition.BornForThisAbility))) return true;
            if (ship.PilotInfo.PilotName == "Plo Koon") return true;
            return false;
        }

        private int GetAiPriority()
        {
            return int.MaxValue;
        }

        private void PayAbilityCost(Action<bool> callback)
        {
            RegisterAbilityTrigger(TriggerTypes.OnAbilityDirect, SelectShipToSpendTL);

            Triggers.ResolveTriggers(TriggerTypes.OnAbilityDirect, delegate { callback(true); });
        }

        private void SelectShipToSpendTL(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                SpendTLonDefender,
                HasLockOnDefenderAndIsFriendly,
                GetAiPriority,
                HostShip.Owner.PlayerNo,
                HostUpgrade.UpgradeInfo.Name,
                "Spend another friendly ship's lock on the defender",
                imageSource: HostUpgrade,
                showSkipButton: false
            );
        }

        private int GetAiPriority(GenericShip ship)
        {
            ShotInfo shotInfo = new ShotInfo(ship, Combat.Defender, ship.PrimaryWeapons.First());
            return (shotInfo.IsShotAvailable) ? 100 - ship.State.Firepower + shotInfo.Range : 100 - ship.PilotInfo.Cost;
        }

        private void SpendTLonDefender()
        {
            SelectShipSubPhase.FinishSelectionNoCallback();

            List<char> tlLetters = ActionsHolder.GetTargetLocksLetterPairs(TargetShip, Combat.Defender);
            TargetShip.Tokens.SpendToken(typeof(BlueTargetLockToken), Triggers.FinishTrigger, tlLetters.First());
        }
    }
}