using ActionsList;
using Content;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.RZ1AWing
    {
        public class TychoCelchu : RZ1AWing
        {
            public TychoCelchu() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Tycho Celchu",
                    5,
                    36,
                    pilotTitle: "Son of Alderaan",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.TychoCelchuAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Talent }
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class TychoCelchuAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnCheckCanPerformActionsWhileStressed += ConfirmThatIsPossible;
            HostShip.OnCanPerformActionWhileStressed += CheckTwoOrFewerStress;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnCanPerformActionWhileStressed -= CheckTwoOrFewerStress;
            HostShip.OnCheckCanPerformActionsWhileStressed -= ConfirmThatIsPossible;
        }


        private void ConfirmThatIsPossible(ref bool isAllowed)
        {
            isAllowed = (HostShip.Tokens.CountTokensByType<Tokens.StressToken>() <= 2);
        }

        private void CheckTwoOrFewerStress(GenericAction action, ref bool isAllowed)
        {
            isAllowed = (HostShip.Tokens.CountTokensByType<Tokens.StressToken>() <= 2);
        }
    }
}