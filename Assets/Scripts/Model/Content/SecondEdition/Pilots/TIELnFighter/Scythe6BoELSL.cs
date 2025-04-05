using Abilities.SecondEdition;
using Actions;
using ActionsList;
using BoardTools;
using Content;
using Ship;
using SubPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.TIELnFighter
    {
        public class Scythe6BoELSL : TIELnFighter
        {
            public Scythe6BoELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Scythe 6",
                    2,
                    42,
                    isLimited: true,
                    tags: new List<Tags>
                    {
                        Tags.BoE
                    },
                    abilityType: typeof(Scythe6Ability)
                );
                ShipAbilities.Add(new FormedUpBoEAbility());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(EvadeAction)));
                ShipInfo.Hull++;
                ShipInfo.UpgradeIcons.Upgrades.Remove(UpgradeType.Modification);
                PilotNameCanonical = "scythe6-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class Scythe6Ability : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.AfterGotNumberOfAttackDice += CheckAbility;
        }

        private void CheckAbility(ref int value)
        {
            if (Combat.ShotInfo.Range >= 1 && Combat.ShotInfo.Range <= 2 )
            {
                Messages.ShowInfo(HostShip.PilotInfo.PilotName + ": The attack is at range 1-2, attacker gains +1 attack die");
                value++;
            }
        }
    }

    public class FormedUpBoEAbility : GenericAbility
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
            if (HostShip.Tokens.GetNonLockRedTokens().Count > 0
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
            List<GenericShip> friendlyTies = Board.GetShipsAtRange(HostShip, new Vector2(0, 1), Team.Type.Friendly).Where(n => n is Ship.SecondEdition.TIELnFighter.TIELnFighter).ToList();
            return friendlyTies.Count > 1;
        }
    }
}