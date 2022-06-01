using ActionsList;
using Ship;
using SquadBuilderNS;
using Upgrade;
using Actions;
using System.Collections.Generic;

namespace UpgradesList.SecondEdition
{
    public class ExperimentalInterface : GenericUpgrade
    {
        public ExperimentalInterface() : base()
        {
            FromMod = typeof(Mods.ModsList.HotacEliteImperialPilotsModSE);
            UpgradeInfo = new UpgradeCardInfo(
                "Experimental Interface",
                UpgradeType.Modification,
                cost: 6,
                abilityType: typeof(Abilities.SecondEdition.ExperimentalInterfaceAbility)
            );

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/Hotac-Upgrade-Cards/main/Upgrades/modification/experimentalinterface.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ExperimentalInterfaceAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckAbility;
        }

        private void CheckAbility(GenericAction action)
        {
            if (action is FocusAction)
            {
                RegisterAbilityTrigger(TriggerTypes.OnActionDecisionSubPhaseEnd, FreeCoordinateAction);
            }
        }

        private void FreeCoordinateAction(object sender, System.EventArgs e)
        {
            List<GenericAction> actions = new List<GenericAction>() { new CoordinateAction() };

            HostShip.AskPerformFreeAction(
                actions,
                Triggers.FinishTrigger,
                HostUpgrade.UpgradeInfo.Name,
                "After you perform a Focus action, perform a free coordinate action",
                HostUpgrade
            );
        }

       
    }
}