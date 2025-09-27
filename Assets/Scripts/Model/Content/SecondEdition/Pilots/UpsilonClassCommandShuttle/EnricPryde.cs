using System;
using Ship;
using System.Linq;
using Upgrade;
using System.Collections.Generic;
using SubPhases;

namespace Ship
{
    namespace SecondEdition.UpsilonClassCommandShuttle
    {
        public class EnricPryde : UpsilonClassCommandShuttle
        {
            public EnricPryde() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Enric Pryde",
                    3,
                    62,
                    isLimited: true,
                    abilityType: typeof(Abilities.SecondEdition.EnricPrydeAbility)
                );
                ImageUrl = "https://raw.githubusercontent.com/sampson-matt/FlyCasualLegacyCustomCards/refs/heads/main/Homebrew/X2PO-homebrewPilot-watenricprydev22.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class EnricPrydeAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker += RegisterTrigger;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackFinishAsAttacker -= RegisterTrigger;
        }

        private void RegisterTrigger(GenericShip ship)
        {
            if (Roster.AllShips.Values.Any(s => FilterAbilityTargets(s)))
            {
                RegisterAbilityTrigger(TriggerTypes.OnAttackFinish, AskAbility);
            }

        }

        private bool FilterAbilityTargets(GenericShip ship)
        {
            return BoardTools.Board.GetShipsAtRange(HostShip, new UnityEngine.Vector2(0, 3), Team.Type.Friendly).ToList().Contains(ship);
        }

        private void AskAbility(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                SetupBonusAttack,
                FilterAbilityTargets,
                AiPriority,
                HostShip.Owner.PlayerNo,
                HostName,
                "You may choose a a friendly ship at range 0-3. You and the chosen ship may perform a bonus attack. Then the chosen ship is destroyed",
                HostShip
            );
        }

        private int AiPriority(GenericShip ship)
        {
            return 0;
        }

        private void SetupBonusAttack()
        {
            TargetShip.OnAttackFinishAsAttacker += DestroyShip;
            BonusAttack(TargetShip, () => BonusAttack(HostShip, SelectShipSubPhase.FinishSelection));
        }

        private void DestroyShip(GenericShip ship)
        {
            ship.OnAttackFinishAsAttacker -= DestroyShip;
            Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " is destroyed.");
            ship.DestroyShipForced(delegate { });
        }

        private void BonusAttack(GenericShip ship, Action callback)
        {
            if (!ship.IsCannotAttackSecondTime)
            {
                Messages.ShowInfo(ship.PilotInfo.PilotName + " can perform a bonus attack.");

                ship.IsCannotAttackSecondTime = true;

                Combat.StartSelectAttackTarget
                (
                    ship,
                    callback,
                    AnyTarget,
                    HostShip.PilotInfo.PilotName,
                    "You may perform a bonus attack",
                    HostShip
                );
            }
            else
            {
                Messages.ShowErrorToHuman(ship.PilotInfo.PilotName + " cannot perform a second attack");
                callback();
            }
        }

        private bool AnyTarget(GenericShip ship, IShipWeapon weapon, bool isSilent)
        {
            return true;
        }
    }
}
