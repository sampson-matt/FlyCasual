using Ship;
using Upgrade;
using System.Linq;
using System.Collections.Generic;
using System;
using ActionsList;
using SubPhases;
using Actions;

namespace UpgradesList.SecondEdition
{
    public class Leebo : GenericUpgrade
    {
        public Leebo() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Leebo",
                UpgradeType.Crew,
                cost: 1,
                isLimited: true,
                restrictions: new UpgradeCardRestrictions(
                    new FactionRestriction(Faction.Scum, Faction.Rebel)
                ),
                abilityType: typeof(Abilities.SecondEdition.LeeboCrewAbility)
            );
            NameCanonical = "leebo-rsl";
        }
    }
}
namespace Abilities.SecondEdition
{
    //After you repair a damage card, you may perform an action on your action bar.
    public class LeeboCrewAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnFacedownDamageCardIsRepaired += CheckFacedownAbility;
            HostShip.OnFaceupDamageCardIsRepaired += CheckFaceUpAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnFacedownDamageCardIsRepaired -= CheckFacedownAbility;
            HostShip.OnFaceupDamageCardIsRepaired -= CheckFaceUpAbility;
        }

        private void CheckFacedownAbility(GenericDamageCard damageCard)
        {
            RegisterAbilityTrigger(TriggerTypes.OnFacedownDamageCardIsRepaired, PerformAction);
        }

        private void CheckFaceUpAbility(GenericDamageCard damageCard)
        {
            RegisterAbilityTrigger(TriggerTypes.OnFaceupDamageCardIsRepaired, PerformAction);
        }

        private void PerformAction(object sender, System.EventArgs e)
        {
            List<GenericAction> actions = HostShip.GetAvailableActions();
            List<GenericAction> actionBarActions = actions
                .Where(n => n.IsInActionBar)
                .ToList();

            Selection.ChangeActiveShip(HostShip);

            HostShip.AskPerformFreeAction(
                actionBarActions,
                delegate
                {
                    Triggers.FinishTrigger();
                },
                HostUpgrade.UpgradeInfo.Name,
                "After you repair a damage card, you may perform an action on your action bar.",
                HostUpgrade
            );
        }
    }
}