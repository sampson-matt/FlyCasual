using Upgrade;
using System.Collections.Generic;
using ActionsList;
using Ship;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class CommanderKenkirkPilotAbility : GenericUpgrade
    {
        public CommanderKenkirkPilotAbility() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);

            UpgradeInfo = new UpgradeCardInfo(
                "Commander Kenkirk Pilot Ability",
                UpgradeType.Pilot,

                cost: 10,
                restriction: new StatValueRestriction(
                        StatValueRestriction.Stats.Initiative,
                        StatValueRestriction.Conditions.HigherThanOrEqual,
                        5
                    ),
                abilityType: typeof(Abilities.SecondEdition.CommanderKenkirkPilotAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/PilotAbilities/Imperial/commanderkenkirk.png";
        }


    }
}

namespace Abilities.SecondEdition
{
    public class CommanderKenkirkPilotAbility : GenericAbility
    {
        private bool AbilityIsActive;

        public override void ActivateAbility()
        {
            AbilityIsActive = false;
            HostShip.OnDamageWasSuccessfullyDealt += CheckAbility;
            HostShip.OnShieldLost += checkAbilityShieldLost;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDamageWasSuccessfullyDealt -= CheckAbility;
            HostShip.OnShieldLost -= checkAbilityShieldLost;
            TryDeactivateAbilityBonus();
        }

        private void checkAbilityShieldLost()
        {
            CheckAbility(HostShip, false);
        }

        private void CheckAbility(GenericShip ship, bool flag)
        {
            bool isDamaged = HostShip.Damage.IsDamaged;
            bool noShields = HostShip.State.ShieldsCurrent <= 0;
            if(isDamaged && !noShields)
            {
                TryActivateAbilityBonus();
            }
            else
            {
                TryDeactivateAbilityBonus();
            }
        }

        private void TryActivateAbilityBonus()
        {
            if (!AbilityIsActive)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has no shields and is damanged and gains +1 Agility");
                HostShip.ChangeAgilityBy(+1);
                HostShip.Tokens.AssignCondition(typeof(Conditions.CommanderKenkirkCondition));
                AbilityIsActive = true;
            }
        }

        private void TryDeactivateAbilityBonus()
        {
            if (AbilityIsActive)
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": -1 Agility");
                HostShip.ChangeAgilityBy(-1);
                HostShip.Tokens.RemoveCondition(typeof(Conditions.CommanderKenkirkCondition));
                AbilityIsActive = false;
            }
        }
    }
}

namespace Conditions
{
    public class CommanderKenkirkCondition : GenericToken
    {
        public CommanderKenkirkCondition(GenericShip host) : base(host)
        {
            Name = ImageName = "Buff Token";
            Temporary = false;
            Tooltip = UpgradesList.SecondEdition.CommanderKenkirkPilotAbility.CurrentUpgrade.ImageUrl;
        }
    }
}