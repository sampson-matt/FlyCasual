using Content;
using Ship;
using System;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;

namespace Ship.SecondEdition.Delta7Aethersprite
{
    public class DepaBillaba : Delta7Aethersprite
    {
        public DepaBillaba()
        {
            PilotInfo = new PilotCardInfo(
                "Depa Billaba",
                3,
                38,
                true,
                force: 2,
                abilityType: typeof(Abilities.SecondEdition.DepaBillabaAbility),
                tags: new List<Tags>
                {
                    Tags.LightSide,
                    Tags.Jedi
                },
                extraUpgradeIcon: UpgradeType.ForcePower
            );
            PilotNameCanonical = "depabillaba-wat1";
        }
    }
}

namespace Abilities.SecondEdition
{
    // While a friendly ship with [force] in your [full front arc] defends or performs an attack,
    // if it has spent more inactive [force] than active [force], it may spend your [force] as if it were theirs.
    public class DepaBillabaAbility : GenericAbility
    {
        private HashSet<GenericShip> FriendlyShips = new HashSet<GenericShip>();
        private GenericShip FriendlyShip;
        public override void ActivateAbility()
        {
            GenericShip.OnBeforeAttackStartAsAttackerGlobal += CheckForceAttacker;
            GenericShip.OnBeforeAttackStartAsDefenderGlobal += CheckForceDefender;
            GenericShip.OnForceTokensAreSpent += CheckForceWhenTokensSpent;
        }

        public override void DeactivateAbility()
        {
            GenericShip.OnBeforeAttackStartAsAttackerGlobal -= CheckForceAttacker;
            GenericShip.OnBeforeAttackStartAsDefenderGlobal -= CheckForceDefender;
            GenericShip.OnForceTokensAreSpent -= CheckForceWhenTokensSpent;
        }

        private void CheckForceAttacker()
        {
            CheckForce(Combat.Attacker);
        }

        private void CheckForceDefender()
        {
            CheckForce(Combat.Defender);
        }

        private void CheckForceWhenTokensSpent(GenericShip ship, ref int count)
        {
            if (Phases.CurrentPhase is MainPhases.CombatPhase
                && Combat.Attacker != null
                && Combat.Defender != null
                && (Tools.IsSameShip(ship, Combat.Attacker) || Tools.IsSameShip(ship, Combat.Defender)))
            {
                CheckForce(ship);
            }
        }

        private void CheckForce(GenericShip ship)
        {
            if (ship != HostShip &&
                ship.State.MaxForce > 0 &&
                ship.State.MaxForce - ship.State.Force > ship.State.Force &&
                HostShip.State.Force > 0 &&
                BoardTools.Board.GetShipsInArcAtRange(HostShip, Arcs.ArcType.FullFront, new Vector2(0, 3), Team.Type.Friendly).Contains(ship))
            {
                FriendlyShip = ship;
                RegisterAbilityTrigger(TriggerTypes.OnBeforeAttackStart, delegate
                {
                    AskToUseAbility(
                        HostShip.PilotInfo.PilotName,
                        AlwaysUseByDefault,
                        useForce,
                        descriptionLong: $"Do you want to use Depa Billaba's Force as if it were {ship.PilotInfo.PilotName}'s?",
                        imageHolder: HostShip
                    );
                });
            }
        }

        private void useForce(object sender, EventArgs e)
        {
            FriendlyShip.BeforeGetForce += GetHostForce;
            FriendlyShip.BeforeForceTokensAreSpent += SpendForce;
            FriendlyShip.OnAttackFinishAsAttacker += CleanUp;
            FriendlyShip.OnAttackFinishAsDefender += CleanUp;
            SubPhases.DecisionSubPhase.ConfirmDecision();
        }

        private void SpendForce(GenericShip ship, ref int count)
        {
            HostShip.State.SpendForce(count, delegate {  });
            count = 0;
        }

        private void GetHostForce(ref int count)
        {
            count = HostShip.State.Force;
        }

        private void CleanUp(GenericShip ship)
        {
            FriendlyShip.BeforeGetForce -= GetHostForce;
            FriendlyShip.BeforeForceTokensAreSpent -= SpendForce;
        }
    }
}
