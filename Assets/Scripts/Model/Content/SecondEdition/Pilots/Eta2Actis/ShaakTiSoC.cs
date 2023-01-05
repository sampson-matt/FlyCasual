using Abilities.Parameters;
using Ship;
using SubPhases;
using System;
using Actions;
using ActionsList;
using Upgrade;

namespace Ship.SecondEdition.Eta2Actis
{
    public class ShaakTiSoC : Eta2Actis
    {
        public ShaakTiSoC()
        {
            PilotInfo = new PilotCardInfo(
                "Shaak Ti",
                4,
                43,
                true,
                force: 2,
                abilityType: typeof(Abilities.SecondEdition.ShaakTiSoCAbility),
                extraUpgradeIcon: UpgradeType.Talent
            );
            PilotNameCanonical = "shaakti-soc";

            ModelInfo.SkinName = "Red";

            ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/main/SiegeOfCoruscant/shaakti-soc.png";
        }
    }
}

namespace Abilities.SecondEdition
{
    public class ShaakTiSoCAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnEndPhaseStart_Triggers += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnEndPhaseStart_Triggers -= CheckAbility;
        }
        private void CheckAbility()
        {
            if (HostShip.State.Force > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.OnEndPhaseStart, AskUseAbility);
            }
        }

        private void AskUseAbility(object sender, EventArgs e)
        {
            Selection.ChangeActiveShip(HostShip);
            HostShip.OnCheckCoordinateModeModification += SetCustomCoordinateMode;
            HostShip.AskPerformFreeAction(
                new CoordinateAction() { CanBePerformedWhileStressed = true, Color = ActionColor.Purple },
                CleanUp,
                HostName,
                "At the start of the End Phase, you may perform a purple Coordinate action, even while stressed. If the chosen ship has the Born For This ship ability, you may coordinate 1 additional ship.",
                HostShip
            );
        }

        private void CleanUp()
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            Triggers.FinishTrigger();
        }

        private void SetCustomCoordinateMode(ref CoordinateActionData coordinateActionData)
        {
            coordinateActionData.MaxTargets = 2;
            coordinateActionData.BornForThisLimit = true;
            
            HostShip.OnCheckCoordinateModeModification -= SetCustomCoordinateMode;
        }
    }
}
