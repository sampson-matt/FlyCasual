using Upgrade;
using System.Collections.Generic;
using Ship;
using BoardTools;
using ActionsList;
using System;
using SubPhases;
using System.Linq;
using UnityEngine;
using Tokens;

namespace UpgradesList.SecondEdition
{
    public class Opportunist : GenericUpgrade
    {
        public Opportunist() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Opportunist",
                UpgradeType.Talent,
                cost: 8,
                abilityType: typeof(Abilities.SecondEdition.OpportunistAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/talent/opportunist.png";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class OpportunistAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackStartAsAttacker += RegisterOpportunistAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackStartAsAttacker -= RegisterOpportunistAbility;
        }

        private void RegisterOpportunistAbility()
        {
            if (!Combat.Attacker.Tokens.HasToken(typeof(StressToken)) && (!Combat.Defender.Tokens.HasToken(typeof(FocusToken)) && !Combat.Defender.Tokens.HasToken(typeof(Tokens.EvadeToken))))
            {
                Combat.Attacker.AfterGotNumberOfAttackDice += IncreaseByOne;
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + " has no stress and his target has no Focus or Evade tokens, he rolls +1 attack die");
            }
        }
        private void IncreaseByOne(ref int value)
        {
            value++;
            Combat.Attacker.AfterGotNumberOfAttackDice -= IncreaseByOne;
        }
    }
}