using ActionsList;
using Ship;
using System.Linq;
using UnityEngine;
using Upgrade;
using SquadBuilderNS;
using System.Collections.Generic;
using BoardTools;
using UpgradesList.SecondEdition;
using System;
using SubPhases;

namespace UpgradesList.SecondEdition
{
    public class FormedUp : GenericUpgrade
    {
        public FormedUp() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Formed Up",
                UpgradeType.Talent,
                cost: 1,
                limited: 3,
                restrictions: new UpgradeCardRestrictions(new FactionRestriction(Faction.Imperial), new ShipRestriction(typeof(Ship.SecondEdition.TIELnFighter.TIELnFighter))),
                abilityType: typeof(Abilities.SecondEdition.FormedUpAbility)
            );
            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/RSLUpgrades/FormedUp.jpg";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class FormedUpAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnRoundEnd += CheckEndPhaseAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnRoundEnd -= CheckEndPhaseAbility;
        }

        private void CheckEndPhaseAbility()
        {
            if(HostShip.Tokens.GetNonLockRedTokens().Count > 0
                && hasFriendlyShipsInRange())
            {
                RegisterAbilityTrigger(TriggerTypes.OnRoundEnd, AskRemoveToken);
            }
        }

        private void AskRemoveToken(object sender, EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);
            FormedUpRemoveRedTokenAbilityDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<FormedUpRemoveRedTokenAbilityDecisionSubPhase>(
                "Formed Up: You may remove 1 non-lock red token",
                Triggers.FinishTrigger
            );
            subphase.ImageSource = HostReal as IImageHolder;
            subphase.AbilityHostShip = HostShip;
            subphase.RemoveOnlyNonLocks = true;
            subphase.Start();
        }

        private class FormedUpRemoveRedTokenAbilityDecisionSubPhase : RemoveRedTokenDecisionSubPhase
        {
            public GenericShip AbilityHostShip;

            public override void PrepareCustomDecisions()
            {
                DescriptionShort = AbilityHostShip.PilotInfo.PilotName;
                DescriptionLong = "You may remove 1 non-lock red token";

                DecisionOwner = Selection.ThisShip.Owner;
                DefaultDecisionName = decisions.First().Name;
            }
        }

        private bool hasFriendlyShipsInRange()
        {
            List<GenericShip> friendlyShips = Board.GetShipsAtRange(HostShip, new Vector2(0, 1), Team.Type.Friendly).Where(n => n is Ship.SecondEdition.TIELnFighter.TIELnFighter).ToList();
            List<GenericShip> friendlyFormedUpShips = friendlyShips.Where(n=> n.UpgradeBar.HasUpgradeInstalled(typeof(FormedUp))).ToList();

            return friendlyShips.Count > 2 || friendlyFormedUpShips.Count > 1;
        }
    }
}