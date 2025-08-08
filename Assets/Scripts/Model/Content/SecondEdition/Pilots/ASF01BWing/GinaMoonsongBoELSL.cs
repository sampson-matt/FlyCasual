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
using Tokens;
using UnityEngine;
using Upgrade;

namespace Ship
{
    namespace SecondEdition.ASF01BWing
    {
        public class GinaMoonsongBOELSL : ASF01BWing
        {
            public GinaMoonsongBOELSL() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Gina Moonsong",
                    5,
                    55,
                    isLimited: true,
                    abilityType: typeof(GinaMoonsongBattleOverEndorAbility),
                    tags: new List<Tags>
                    {
                        Tags.BoE,
                        Tags.LsL
                    },
                    charges: 2,
                    regensCharges: 1,
                    extraUpgradeIcon: UpgradeType.Talent
                );
                ShipAbilities.Add(new GyroCockpit());
                ShipInfo.ActionIcons.AddLinkedAction(new LinkedActionInfo(typeof(BarrelRollAction), typeof(TargetLockAction)));
                ShipInfo.ActionIcons.AddActions(new ActionInfo(typeof(ReloadAction), ActionColor.Red));
                ModelInfo.SkinName = "Gina Moonsong";            
                PilotNameCanonical = "ginamoonsong-battleoverendor-lsl";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class GinaMoonsongBattleOverEndorAbility : GenericAbility
    {
        //At the start of the Engagement Phase, if a friendly Braylen Stramm ship at range 0-2 is stressed, you may acquire a lock.
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += CheckAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= CheckAbility;
        }
        protected virtual void CheckAbility()
        {
            List<GenericShip> friendlyShipsAtRange = Board.GetShipsAtRange(HostShip, new Vector2(0, 2), Team.Type.Friendly);

            foreach (GenericShip ship in friendlyShipsAtRange)
            {
                if (ship.PilotInfo.PilotName.Equals("Braylen Stramm") && ship.IsStressed)
                {
                    RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, SelectTarget);
                }
            }
        }

        private void SelectTarget(object sender, EventArgs e)
        {
            SelectTargetForAbility(
                AcquireLock,
                FilterTargets,
                GetAiPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                "You may acquire a lock",
                HostShip,
                showSkipButton: true
            );
        }

        private int GetAiPriority(GenericShip ship)
        {
            int result = 0;

            if (!Tools.IsSameTeam(ship, Selection.ThisShip))
            {
                ShotInfo shotInfo = new ShotInfo(Selection.ThisShip, ship, Selection.ThisShip.PrimaryWeapons);
                if (shotInfo.IsShotAvailable) result += 1000;
                if (!ship.ShipsBumped.Contains(Selection.ThisShip)) result += 500;
                if (shotInfo.Range <= 3) result += 250;

                result += ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost);
            }

            return result;
        }

        private bool FilterTargets(GenericShip ship)
        {
            return FilterTargetsByRange(ship, 0, 3);
        }

        private void AcquireLock()
        {
            ActionsHolder.AcquireTargetLock(
                HostShip,
                TargetShip,
                DecisionSubPhase.ConfirmDecision,
                DecisionSubPhase.ConfirmDecision
            );
        }
    }

    public class GyroCockpit : GenericAbility
    {
        // After you gain a stress token, you may spend 2 charges to gain an evade token.
        // When you drop a device, you may spend 1 charge to set the template with its middle line aligned with the hashmark on your ship's left or right side instead of your rear guides

        Direction selectedDirection = Direction.Bottom;

        public override void ActivateAbility()
        {
            HostShip.OnTokenIsAssigned += RegisterEvadeAbility;
            HostShip.BeforeBombWillBeDropped += RegisterDeviceDropAbility;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnTokenIsAssigned -= RegisterEvadeAbility;
            HostShip.BeforeBombWillBeDropped -= RegisterDeviceDropAbility;
        }

        private void RegisterEvadeAbility(GenericShip ship, GenericToken token)
        {
            if (token.GetType() == typeof(StressToken))
            {
                RegisterAbilityTrigger(TriggerTypes.OnTokenIsAssigned, AskUseEvadeAbility);
            }
        }

        private void AskUseEvadeAbility(object sender, EventArgs e)
        {
            if (HostShip.State.Charges >= 2)
            {
                AskToUseAbility(
                    descriptionShort: "Do you want to spend 2 charges to gain an evade token",
                    useByDefault: NeverUseByDefault,
                    useAbility: UseEvadeAbility,
                    imageHolder: HostShip
                );
            }
            else
            {
                Triggers.FinishTrigger();
            }
        }

        private void UseEvadeAbility(object sender, EventArgs e)
        {
            HostShip.Tokens.AssignToken(new EvadeToken(HostShip), DecisionSubPhase.ConfirmDecision);
            HostShip.SpendCharges(2);
        }

        private void RegisterDeviceDropAbility()
        {
            if (HostShip.State.Charges > 0)
            {
                RegisterAbilityTrigger(TriggerTypes.BeforeBombWillBeDropped, AskToUseDeviceDropAbility);
            }
        }

        private void AskToUseDeviceDropAbility(object sender, EventArgs e)
        {
            AskForDecision(
                descriptionShort: "Gyro-Cockpit",
                descriptionLong: "Spend 1 ship charge to drop device using left or right side instead of rear guides?",
                imageHolder: HostShip,
                decisions: new()
                {
                    { "Left", UseDeviceAbilityLeft },
                    { "Right", UseDeviceAbilityRight }
                },
                tooltips: new(),
                defaultDecision: "No",
                callback: Triggers.FinishTrigger,
                showSkipButton: true
            );
        }

        private void UseDeviceAbility()
        {
            HostShip.OnGetBombTemplateDirection += GetDeviceDirection;
            HostShip.SpendCharge();
            Triggers.FinishTrigger();
        }

        private void UseDeviceAbilityLeft(object sender, EventArgs e)
        {
            selectedDirection = Direction.Left;
            UseDeviceAbility();
        }

        private void UseDeviceAbilityRight(object sender, EventArgs e)
        {
            selectedDirection = Direction.Right;
            UseDeviceAbility();
        }

        private void GetDeviceDirection(ref Direction direction)
        {
            HostShip.OnGetBombTemplateDirection -= GetDeviceDirection;
            direction = selectedDirection;
            selectedDirection = Direction.Bottom;
        }
    }
}
