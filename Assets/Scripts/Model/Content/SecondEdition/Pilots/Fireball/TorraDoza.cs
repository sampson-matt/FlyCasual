using System;
using System.Collections.Generic;
using System.Linq;
using BoardTools;
using Ship;
using SubPhases;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.Fireball
    {
        public class TorraDoza : Fireball
        {
            public TorraDoza() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Torra Doza",
                    3,
                    28,
                    isLimited: true,
                    abilityText: "After a friendly ship at range 0-2 exposes a damage card, you may gain a deplete or stress token to repair that card without resolving its effects.",
                    abilityType: typeof(Abilities.SecondEdition.TorraDozaAbility),
                    extraUpgradeIcons: new List<UpgradeType> { UpgradeType.Talent, UpgradeType.Astromech }
                );
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/TorraDoza.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{

    public class TorraDozaAbility : GenericAbility
    {
        GenericShip damagedShip;
        GenericDamageCard critCard;
        public override void ActivateAbility()
        {
            GenericShip.OnCritExposedGlobal += RegisterTorraDozaTrigger;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnCritExposedGlobal -= RegisterTorraDozaTrigger;
        }

        private void RegisterTorraDozaTrigger(GenericShip ship, bool isFaceUp)
        {
            if (isFaceUp
                && Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0,2),Team.Type.Friendly).Contains(ship))
            {
                damagedShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnCritExposed, AskUseTorraDozaAbility);
            }
        }        

        private void AskUseTorraDozaAbility(object sender, System.EventArgs e)
        {
            TorraDozaDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<TorraDozaDecisionSubPhase>("Torra Doza Decision Subphase", Triggers.FinishTrigger);

            subphase.DescriptionShort = HostShip.PilotInfo.PilotName;
            subphase.DescriptionLong = "You may gain a deplete or stress token to repair that card without resolving its effects:";
            subphase.ImageSource = HostShip;

            subphase.AddDecision("Gain a deplete", delegate { HandleFaceupCardDeplete(); });
            subphase.AddDecision("Gain a stress", delegate { HandleFaceupCardStress(); });

            subphase.DefaultDecisionName = "Gain a deplete";

            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;

            subphase.Start();
        }

        private void HandleFaceupCardDeplete()
        {
            HostShip.Tokens.AssignToken(typeof(DepleteToken), cleanup);
        }

        private void HandleFaceupCardStress()
        {
            HostShip.Tokens.AssignToken(typeof(StressToken), cleanup);
        }

        private void cleanup()
        {
            Messages.ShowInfo($"{HostShip.PilotInfo.PilotName}: {Combat.CurrentCriticalHitCard.Name} is repaired without resolving its effects");
            Combat.CurrentCriticalHitCard.IsFaceup = false;
            DecisionSubPhase.ConfirmDecision();
        }

        private class TorraDozaDecisionSubPhase : DecisionSubPhase { }
    }
}