using Abilities.Parameters;
using ActionsList;
using Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using Upgrade;
using BoardTools;

namespace Ship
{
    namespace SecondEdition.CloneZ95Headhunter
    {
        public class Boost : CloneZ95Headhunter
        {
            public Boost() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Boost\"",
                    3,
                    25,
                    pilotTitle: "CT-4860",
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.BoostAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent}
                );
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class BoostAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterBoostAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterBoostAbility;
        }

        private void RegisterBoostAbility()
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, FreeBoostAbility);
        }

        private void FreeBoostAbility(object sender, System.EventArgs e)
        {
            
            if (Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0, 1), Team.Type.Friendly).FindAll(n => n.RevealedManeuver.ColorComplexity == Movement.MovementComplexity.Easy).Count >= 1)
            {
                Selection.ThisShip = HostShip;
                HostShip.AskPerformFreeAction(
                    new BoostAction() { HostShip = HostShip },
                    Triggers.FinishTrigger,
                    HostShip.PilotInfo.PilotName,
                    "At the start of the Engagement Phase, if there is a friendly ship at range 0-1 whose revealed maneuver is blue, you may perform a boost action.",
                    HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }
    }
}