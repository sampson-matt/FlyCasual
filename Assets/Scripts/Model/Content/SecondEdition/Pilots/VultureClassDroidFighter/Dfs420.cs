using ActionsList;
using BoardTools;
using Content;
using Movement;
using Ship;
using SubPhases;
using System.Collections.Generic;
using System.Linq;
using Tokens;
using Upgrade;

namespace Ship.SecondEdition.VultureClassDroidFighter
{
    public class Dfs420 : VultureClassDroidFighter
    {
        public Dfs420()
        {
            PilotInfo = new PilotCardInfo(
                "DFS-420",
                4,
                24,
                true,
                abilityType: typeof(Abilities.SecondEdition.Dfs420Ability),
                tags: new List<Tags>
                {
                    Tags.Droid
                },
                extraUpgradeIcon: UpgradeType.Talent
            );
            PilotNameCanonical = "dfs420-wat1";
        }
    }
}

namespace Abilities.SecondEdition
{
    //After you fully execute a red maneuver or perform a red barrel roll, if there is an enemy ship at Range 0-1, you may remove 1 stress token.
    //You may perform primary attacks at Range 0.
    public class Dfs420Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnActionIsPerformed += CheckConditions;
            HostShip.OnMovementFinishSuccessfully += RegisterMovementTrigger;
            HostShip.PrimaryWeapons.ForEach(n => n.WeaponInfo.MinRange = 0);
        }

        public override void DeactivateAbility()
        {
            HostShip.OnActionIsPerformed -= CheckConditions;
            HostShip.OnMovementFinishSuccessfully -= RegisterMovementTrigger;
            HostShip.PrimaryWeapons.ForEach(n => n.WeaponInfo.MinRange = 1);
        }

        protected void CheckConditions(GenericAction action)
        {
            if (action.IsRed && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0, 1), Team.Type.Enemy).Any())
            {
                HostShip.OnActionDecisionSubphaseEnd += RegisterActionTrigger;
            }
        }

        private void RegisterActionTrigger(GenericShip ship)
        {
            HostShip.OnActionDecisionSubphaseEnd -= RegisterActionTrigger;

            RegisterAbilityTrigger(TriggerTypes.OnFreeAction, AskAbility);
        }

        protected void RegisterMovementTrigger(GenericShip ship)
        {
            if (HostShip.GetLastManeuverColor() == MovementComplexity.Complex && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0,1), Team.Type.Enemy).Any())
            {
                RegisterAbilityTrigger(TriggerTypes.OnMovementFinish, AskAbility);
            }
        }

        private void AskAbility(object sender, System.EventArgs e)
        {
            if (HostShip.IsStressed == true)
            {
                AskToUseAbility(
                    HostShip.PilotInfo.PilotName,
                    AlwaysUseByDefault,
                    UseAbility,
                    descriptionLong: "Do you want to remove 1 Stress Token?",
                    imageHolder: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void UseAbility(object sender, System.EventArgs e)
        {
            HostShip.Tokens.RemoveToken(typeof(StressToken), DecisionSubPhase.ConfirmDecision);
        }
    }
}
