using BoardTools;
using Tokens;
using SubPhases;
using System;
using System.Collections;
using System.Collections.Generic;
using ActionsList;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.Hwk290LightFreighter
    {
        public class JanusKasmir : Hwk290LightFreighter
        {
            public JanusKasmir() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Janus Kasmir",
                    4,
                    34,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.JanusKasmirAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent, UpgradeType.Illicit },
                    factionOverride: Faction.Scum
                );
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/X2PO-homebrewPilot-watjanuskasmirv23.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class JanusKasmirAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= CheckAbility;
        }

        private void CheckAbility()
        {
            if (HostShip.Tokens.HasGreenTokens)
            {
                RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, AskToSelectToken);
            }
        }

        private void AskToSelectToken(object sender, EventArgs e)
        {
            JanusKasmirAbilityDecisionSubPhase subphase = Phases.StartTemporarySubPhaseNew<JanusKasmirAbilityDecisionSubPhase>(
                    "Janus Kasmir: You may spend 1 green token to perform a Jam action even while stressed.",
                    CleanUp
                );

            Dictionary<string, GenericToken> tokens = new Dictionary<string, GenericToken>();
            foreach (GenericToken token in HostShip.Tokens.GetAllTokens())
            {
                if (token.TokenColor != TokenColors.Green)
                    continue;

                if (tokens.ContainsKey(token.Name))
                    continue;

                tokens[token.Name] = token;
            }
            foreach (KeyValuePair<string, GenericToken> kv in tokens)
            {
                subphase.AddDecision(
                    "Spend " + kv.Key.ToLower(),
                    delegate {
                        Messages.ShowInfo(HostShip.PilotInfo.PilotName + " spent " + kv.Key.ToLower());
                        HostShip.Tokens.SpendToken(
                            kv.Value.GetType(),
                            performAction
                        );
                    }
                );
            }
            subphase.DescriptionShort = "Janus Kasmir";
            subphase.DescriptionLong = "You may spend 1 green token to perform a Jam action even while stressed.";
            subphase.ImageSource = HostShip;
            subphase.DefaultDecisionName = "";
            subphase.DecisionOwner = HostShip.Owner;
            subphase.ShowSkipButton = true;
            subphase.Start();
        }

        private void CleanUp()
        {
            Triggers.FinishTrigger();
        }

        private void performAction()
        {
            DecisionSubPhase.ConfirmDecisionNoCallback();
            Selection.ChangeActiveShip(HostShip);
            HostShip.AskPerformFreeAction(
                new JamAction() { CanBePerformedWhileStressed = true },
                CleanUp,
                HostShip.PilotInfo.PilotName,
                "You may perform a Jam action even while stressed.",
                HostShip
            );
        }
    }
}

namespace SubPhases
{
    public class JanusKasmirAbilityDecisionSubPhase : DecisionSubPhase { }
}