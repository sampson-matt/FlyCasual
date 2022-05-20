using Upgrade;
using System;
using Tokens;
using Ship;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class Elusiveness : GenericUpgrade
    {
        public Elusiveness() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Elusiveness",
                UpgradeType.Talent,
                cost: 2,
                abilityType: typeof(Abilities.SecondEdition.ElusivenessAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/talent/elusiveness.png";
        }        
    }
}

namespace Abilities.SecondEdition
{
    public class ElusivenessAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnGenerateDiceModificationsOpposite += ElusivenessActionEffect;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateDiceModificationsOpposite -= ElusivenessActionEffect;
        }

        private void ElusivenessActionEffect(GenericShip host)
        {
            ActionsList.GenericAction newAction = new ActionsList.ElusivenessHotacActionEffect()
            {
                ImageUrl = HostUpgrade.ImageUrl,
                HostShip = host
            };
            host.AddAvailableDiceModificationOwn(newAction);
        }
    }
}

namespace ActionsList
{
    public class ElusivenessHotacActionEffect : GenericAction
    {

        public ElusivenessHotacActionEffect()
        {
            Name = DiceModificationName = "Elusiveness";
            DiceModificationTiming = DiceModificationTimingType.Opposite;
        }

        public override int GetDiceModificationPriority()
        {
            return 80;
        }

        public override bool IsDiceModificationAvailable()
        {
            bool result = false;

            if (Combat.AttackStep == CombatStep.Attack && !HostShip.Tokens.HasToken(typeof(Tokens.StressToken)))
            {
                result = true;
            }

            return result;
        }

        public override void ActionEffect(System.Action callBack)
        {
            DiceRerollManager diceRerollManager = new DiceRerollManager
            {
                NumberOfDiceCanBeRerolled = 1,
                SidesCanBeRerolled = new List<DieSide> { DieSide.Success, DieSide.Crit },
                IsOpposite = true,
                CallBack = delegate { callBack(); }
            };
            diceRerollManager.Start();
        }

        private void AssignStress(System.Action callBack)
        {
            HostShip.Tokens.AssignToken(typeof(StressToken), callBack);
        }

    }

}