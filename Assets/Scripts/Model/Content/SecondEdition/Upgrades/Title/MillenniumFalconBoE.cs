using Ship;
using Upgrade;
using System.Collections.Generic;
using Actions;
using ActionsList;
using Tokens;
using System.Linq;

namespace UpgradesList.SecondEdition
{
    public class MillenniumFalconBoE : GenericUpgrade
    {
        public MillenniumFalconBoE() : base()
        {
            IsHidden = true;

            UpgradeInfo = new UpgradeCardInfo(
                "Millennium Falcon",
                UpgradeType.Title,
                cost: 0,
                addAction: new ActionInfo(typeof(EvadeAction)),
                abilityType: typeof(Abilities.SecondEdition.MilleniumFalconBoEAbility)
            );

            NameCanonical = "millenniumfalcon-boe";
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/BattleOverEndor/MilleniumFalcon.jpg";
        }        
    }
}

namespace Abilities.SecondEdition
{
    //    While attacking or defending, if you have a non-lock red or orange token, you may reroll 1 die.
    public class MilleniumFalconBoEAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            AddDiceModification(
                HostUpgrade.UpgradeInfo.Name,
                IsDiceModificationAvailable,
                GetDiceModificationAiPriority,
                DiceModificationType.Reroll,
                1
            );
        }

        public override void DeactivateAbility()
        {
            RemoveDiceModification();
        }

        public bool IsDiceModificationAvailable()
        {
            return ((HostShip.IsAttacking || HostShip.IsDefending) && HasOrangeOrRedeNonLockTokens());
        }

        private bool HasOrangeOrRedeNonLockTokens()
        {
            if (HostShip.Tokens.CountTokensByColor(TokenColors.Orange) > 0) return true;
            if (HostShip.Tokens.GetTokensByColor(TokenColors.Red).Count(n => !(n is RedTargetLockToken)) > 0) return true;
            return false;
        }

        public int GetDiceModificationAiPriority()
        {
            if (Combat.AttackStep == CombatStep.Attack)
            {
                return 80;
            }

            if (Combat.AttackStep == CombatStep.Defence)
            {
                return 85;
            }

            else return 0;
        }
    }
}
