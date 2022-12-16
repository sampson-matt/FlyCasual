using System.Collections.Generic;
using Upgrade;
using System;
using Abilities.SecondEdition;
using System.Linq;
using SubPhases;
using Ship;
using Tokens;
using BoardTools;
using Conditions;

namespace Ship
{
    namespace SecondEdition.RogueClassStarfighter
    {
        public class MagnaGuardProtector : RogueClassStarfighter
        {
            public MagnaGuardProtector() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "MagnaGuard Protector",
                    4,
                    40,
                    limited: 2,
                    pilotTitle: "Implacable Escort",
                    abilityText: "Setup: After placing forces, assign the Guarded condition to 1 friendly ship other than MagnaGuard Protector.",
                    abilityType: typeof(Abilities.SecondEdition.MagnaGuardProtectorAbility),
                    extraUpgradeIcons: new List<UpgradeType>() { UpgradeType.Talent },
                    factionOverride: Faction.Separatists
                );

                ShipInfo.ActionIcons.SwitchToDroidActions();

                DeadToRights oldAbility = (DeadToRights)ShipAbilities.First(n => n.GetType() == typeof(DeadToRights));
                oldAbility.DeactivateAbility();
                ShipAbilities.Remove(oldAbility);
                ShipAbilities.Add(new NetworkedCalculationsAbility());

                ImageUrl = "https://infinitearenas.com/xw2/images/pilots/magnaguardprotector.png";
            }
        }
    }
}

namespace Abilities.SecondEdition
{
    public class MagnaGuardProtectorAbility : GenericAbility
    {
        protected virtual string Prompt
        {
            get
            {
                return "Assign the Guarded condition to 1 friendly ship other than MagnaGuard Protector.";
            }
        }
        public override void ActivateAbility()
        {
            Phases.Events.OnSetupEnd += RegisterMagnaGuardProtectorAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnSetupEnd -= RegisterMagnaGuardProtectorAbility;
        }

        private void RegisterMagnaGuardProtectorAbility()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = HostShip.ShipId + ": Assign \"Guarded\" condition",
                TriggerType = TriggerTypes.OnSetupEnd,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = SelectMagnaGuardProtectorTarget,
            });
        }

        private void SelectMagnaGuardProtectorTarget(object Sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                  AssignGuarded,
                  CheckRequirements,
                  GetAiGuardedPriority,
                  HostShip.Owner.PlayerNo,
                  "Guarded",
                  Prompt,
                  HostUpgrade
            );
        }

        protected virtual void AssignGuarded()
        {
            // Remove Guarded from all friendly ships
            foreach (var kvp in Roster.AllShips)
            {
                GenericShip ship = kvp.Value;
                ship.Tokens.RemoveCondition(typeof(Guarded));
            }
            TargetShip.Tokens.AssignCondition(new Guarded(TargetShip) { SourceUpgrade = HostUpgrade });
            SelectShipSubPhase.FinishSelection();
        }

        protected virtual bool CheckRequirements(GenericShip ship)
        {
            var match = ship.Owner.PlayerNo == HostShip.Owner.PlayerNo
                && ship.PilotInfo.PilotName != "MagnaGuard Protector";
            return match;
        }

        private int GetAiGuardedPriority(GenericShip ship)
        {
            int result = 0;

            result += (ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost));

            return result;
        }
    }
}

namespace Conditions
{
    public class Guarded : GenericToken
    {
        public GenericUpgrade SourceUpgrade;
        public Guarded(GenericShip host) : base(host)
        {
            Name = ImageName = "Guarded Condition";
            Temporary = false;
            Tooltip = "https://infinitearenas.com/xw2/images/conditions/guarded.png";
        }

        public override void WhenAssigned()
        {
            Host.OnAttackStartAsDefender += CheckConditions;
        }

        public override void WhenRemoved()
        {
            Host.OnAttackStartAsDefender -= CheckConditions;
        }

        private void CheckConditions()
        {
            if(!Board.IsShipInArcByType(Combat.Attacker, Host, Arcs.ArcType.Bullseye))
            {
                Host.AfterGotNumberOfDefenceDice += RollExtraDice;
            }
        }

        private void RollExtraDice(ref int count)
        {
            int extraDice = Board.GetShipsInArcAtRange(Combat.Attacker, Combat.ArcForShot.ArcType, new UnityEngine.Vector2(Combat.ChosenWeapon.WeaponInfo.MinRange, Combat.ChosenWeapon.WeaponInfo.MaxRange), Team.Type.Enemy)
                .FindAll(s => s.PilotInfo.PilotName.Equals("MagnaGuard Protector") && (s.Tokens.HasToken<CalculateToken>() || s.Tokens.HasToken<EvadeToken>())).Count;
            if(extraDice>0)
            {
                Messages.ShowInfo(Host.PilotInfo.PilotName + " is \"Guarded\" and gains +" + extraDice + " attack die");
                count += extraDice;
            }
            
            Host.AfterGotNumberOfAttackDice -= RollExtraDice;
        }
    }
}