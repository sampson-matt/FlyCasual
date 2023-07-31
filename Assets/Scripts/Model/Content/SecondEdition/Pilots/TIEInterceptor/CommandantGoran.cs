using Abilities.SecondEdition;
using ActionsList;
using BoardTools;
using Ship;
using System;
using System.Collections.Generic;
using Upgrade;

namespace Ship.SecondEdition.TIEInterceptor
{
    public class CommandantGoran : TIEInterceptor
    {
        public CommandantGoran() : base()
        {
            PilotInfo = new PilotCardInfo(
                "Commandant Goran",
                4,
                43,
                isLimited: true,
                abilityType: typeof(CommandantGoranAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );

            ModelInfo.SkinName = "Skystrike Academy";

            ImageUrl = "https://raw.githubusercontent.com/eirikmun/x-wing2.0-project-goldenrod/2.0/src/images/En/pilots/commandantgoran.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class CommandantGoranAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            GenericShip.OnMovementFinishUnsuccessfullyGlobal += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnMovementFinishUnsuccessfullyGlobal -= CheckAbility;
        }

        private void CheckAbility(GenericShip ship)
        {
            if (Tools.IsFriendly(HostShip, ship) && HostShip.State.Initiative > ship.State.Initiative)
            {
                DistanceInfo distInfo = new DistanceInfo(HostShip, ship);
                if (distInfo.Range <= 3) RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskToUseGorransAbility);
            }
        }

        private void AskToUseGorransAbility(object sender, EventArgs e)
        {
            if (Selection.ThisShip != null)
            {
                Selection.ThisShip.AskPerformFreeAction
                (
                    new FocusAction() { HostShip = Selection.ThisShip, Color = Actions.ActionColor.Red },
                    Triggers.FinishTrigger,
                    descriptionShort: HostShip.PilotInfo.PilotName,
                    descriptionLong: "You may perform red Focus acton",
                    imageHolder: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }
    }
}