using System;
using System.Collections.Generic;
using Tokens;
using Ship;
using SubPhases;
using Upgrade;
using Conditions;
using BoardTools;

namespace Ship
{
    namespace SecondEdition.XiClassLightShuttle
    {
        public class AgentTierny : XiClassLightShuttle
        {
            public AgentTierny() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Agent Tierny",
                    3,
                    49,
                    pilotTitle: "Persuasive Recruiter",
                    isLimited: true,
                    extraUpgradeIcon: Upgrade.UpgradeType.Talent,
                    abilityType: typeof(Abilities.SecondEdition.AgentTiernyAbility)
                );
                ShipInfo.ActionIcons.RemoveActions(typeof(ActionsList.TargetLockAction));                    
                ShipInfo.ActionIcons.AddActions(new Actions.ActionInfo(typeof(ActionsList.TargetLockAction)));
                ImageUrl = "https://infinitearenas.com/xw2legacy/images/pilots/agenttierny.png";
            }
            
        }
    }
}

namespace Abilities.SecondEdition
{
    public class AgentTiernyAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnSetupEnd += RegisterAgentTiernyAbility;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnSetupEnd -= RegisterAgentTiernyAbility;
        }

        private void RegisterAgentTiernyAbility()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = HostShip.ShipId + ": Assign \"Broken Trust\" condition",
                TriggerType = TriggerTypes.OnSetupEnd,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = SelectAgentTiernyTarget,
            });
        }

        private void SelectAgentTiernyTarget(object Sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                  AssignBrokenTrust,
                  CheckRequirements,
                  GetAiPriority,
                  HostShip.Owner.PlayerNo,
                  "Broken Trust",
                  "Assign the Broken Trust condition to 1 enemy ship.",
                  HostUpgrade,
                  showSkipButton: false
            );
        }

        protected virtual void AssignBrokenTrust()
        {
            // Remove decoyed from all enemy ships
            foreach (var kvp in Roster.AllShips)
            {
                GenericShip ship = kvp.Value;
                ship.Tokens.RemoveCondition(typeof(BrokenTrust));
            }
            TargetShip.Tokens.AssignCondition(new BrokenTrust(TargetShip) { SourceUpgrade = HostUpgrade });
            SelectShipSubPhase.FinishSelection();
        }

        protected virtual bool CheckRequirements(GenericShip ship)
        {
            return !Tools.IsSameTeam(ship, HostShip);
        }

        private int GetAiPriority(GenericShip ship)
        {
            return ship.PilotInfo.Cost;
        }
    }
}

namespace Conditions
{
    public class BrokenTrust : GenericToken
    {
        public GenericUpgrade SourceUpgrade;

        public BrokenTrust(GenericShip host) : base(host)
        {
            Name = ImageName = "Broken Trust Condition";
            Temporary = false;
            Tooltip = "https://infinitearenas.com/xw2/images/conditions/brokentrust.png";
        }

        public override void WhenAssigned()
        {
            Host.OnCheckIsFriendly += TreatAsAllied;
            Host.OnAttackStartAsAttacker += CheckAlliedStress;
            GenericShip.OnFaceupCritCardReadyToBeDealtGlobal += CheckRemoveBrokenTrust;
        }


        public override void WhenRemoved()
        {
            Host.OnCheckIsFriendly -= TreatAsAllied;
            Host.OnAttackStartAsAttacker -= CheckAlliedStress;
            GenericShip.OnFaceupCritCardReadyToBeDealtGlobal -= CheckRemoveBrokenTrust;
        }

        private void CheckRemoveBrokenTrust(GenericShip ship, GenericDamageCard crit, EventArgs e)
        {
           
            if (Combat.Defender != null && (Tools.IsSameShip(Combat.Defender, Host) || Tools.IsSameShip(Combat.Attacker, Host)))
            {
                Triggers.RegisterTrigger
                (
                    new Trigger()
                    {
                        Name = "Remove Broken Trust",
                        TriggerType = TriggerTypes.OnFaceupCritCardIsDealt,
                        TriggerOwner = Host.Owner.PlayerNo,
                        EventHandler = (object sender, EventArgs e) => RemoveBrokenTrust(Host)
                    }
                );
            }
        }

        private void RemoveBrokenTrust(GenericShip host)
        {
            Messages.ShowInfo(Host.PilotInfo.PilotName + " removed Broken Trust condition.");
            if(Host.Tokens.HasToken(typeof(BrokenTrust)))
            {
                Host.Tokens.RemoveCondition(typeof(BrokenTrust));
            }
            Triggers.FinishTrigger();
        }

        private void CheckAlliedStress()
        {
            foreach (GenericShip ship in Roster.AllShips.Values)
            {
                if (Tools.IsSameTeam(ship, Host) && !Tools.IsFriendly(ship, Host) && !ship.IsStressed)
                {
                    ShotInfo shotInfo = new ShotInfo(Host, ship, Combat.ChosenWeapon);
                    if (shotInfo.InArc)
                    {
                        Triggers.RegisterTrigger
                        (
                            new Trigger()
                            {
                                Name = "Broken Trust",
                                TriggerType = TriggerTypes.OnAttackStart,
                                TriggerOwner = Host.Owner.PlayerNo,
                                EventHandler = (object sender, EventArgs e) => AssignStress(ship)
                            }
                        );
                        
                    }
                }
            }
        }

        private void AssignStress(GenericShip ship)
        {
            Messages.ShowInfo(ship.PilotInfo.PilotName + " gains one stress token from Broken Trust condition.");
            ship.Tokens.AssignToken(typeof(StressToken), Triggers.FinishTrigger);
        }

        private void TreatAsAllied(GenericShip ship, ref bool friendly)
        {            
            friendly = false;
        }


    }
}

