using ActionsList;
using BoardTools;
using Ship;
using SquadBuilderNS;
using SubPhases;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using UpgradesList.SecondEdition;

namespace UpgradesList.SecondEdition
{
    public class ContingencyProtocol : GenericUpgrade, IVariableCost
    {
        public ContingencyProtocol() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Contingency Protocol",
                UpgradeType.Modification,
                cost: 2,
                restrictions: new UpgradeCardRestrictions
                (
                    new FactionRestriction(Faction.Separatists)
                ),
                abilityType: typeof(Abilities.SecondEdition.ContingencyProtocolAbility)
            );
            NameCanonical = "contingencyprotocol-rsl";
        }
        public void UpdateCost(GenericShip ship)
        {
            Dictionary<BaseSize, int> sizeToCost = new Dictionary<BaseSize, int>()
            {
                {BaseSize.Small, 1},
                {BaseSize.Medium, 2},
                {BaseSize.Large, 2}
            };

            UpgradeInfo.Cost = sizeToCost[ship.ShipInfo.BaseSize];
        }
        public override bool IsAllowedForSquadBuilderPostCheck(SquadList squadList)
        {
            if (HostShip.UpgradeBar.GetUpgradesAll().Any(n => n.HasType(UpgradeType.TacticalRelay)) ||
                HostShip.PilotInfo.Tags.Contains(Content.Tags.Droid))
            {
                return true;
            }
            else
            {
                Messages.ShowError("Contingency Protocol: Must be a droid pilot or have Tactical Relay equiped");
                return false;
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ContingencyProtocolAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnShipIsDestroyed += RegisterAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnShipIsDestroyed -= RegisterAbility;
        }

        private void RegisterAbility(GenericShip ship, bool flag)
        {
            RegisterAbilityTrigger(TriggerTypes.OnShipIsDestroyed, CheckAbility);
        }

        private void CheckAbility(object sender, System.EventArgs e)
        {
            if (TargetsForAbilityExist(FilterTargets))
            {
                Selection.ChangeActiveShip(HostShip);

                SelectTargetForAbility(
                    ActivateContingencyProtocol,
                    FilterTargets,
                    GetAiPriority,
                    HostShip.Owner.PlayerNo,
                    name: HostUpgrade.UpgradeInfo.Name,
                    description: "Selected ship may perform an action even while stressed",
                    imageSource: HostUpgrade
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void ActivateContingencyProtocol()
        {
            if (TargetShip == null)
            {
                Triggers.FinishTrigger();
            }
            else
            {
                SelectShipSubPhase.FinishSelectionNoCallback();

                Selection.ChangeActiveShip(TargetShip);

                TargetShip.OnCanPerformActionWhileStressed += AllowActionsWhileStressed;

                TargetShip.AskPerformFreeAction
                (
                    TargetShip.GetAvailableActions(),
                    FinishAbility,
                    descriptionShort: HostUpgrade.UpgradeInfo.Name,
                    descriptionLong: "You may perform an action even while stressed"
                );
            }
        }

        private bool FilterTargets(GenericShip ship)
        {
            return ship != HostShip && Tools.IsFriendly(HostShip, ship) && new DistanceInfo(HostShip, ship).Range <= 3 && ship.UpgradeBar.HasUpgradeInstalled(typeof(ContingencyProtocol));
        }

        private int GetAiPriority(GenericShip ship)
        {
            return 45;
        }

        private void AllowActionsWhileStressed(GenericAction action, ref bool isAllowed)
        {
            isAllowed = true;
        }

        private void FinishAbility()
        {
            TargetShip.OnCanPerformActionWhileStressed -= AllowActionsWhileStressed;

            Selection.ChangeActiveShip(HostShip);

            Triggers.FinishTrigger();
        }
    }
}